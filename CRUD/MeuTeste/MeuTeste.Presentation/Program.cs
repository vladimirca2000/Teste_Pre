using FluentValidation;
using MeuTeste.Application.Interfaces;
using MeuTeste.Application.Services;
using MeuTeste.Application.Services.Authentication;
using MeuTeste.Application.Validators;
using MeuTeste.Domain.Interfaces.Services;
using MeuTeste.Domain.Interfaces.UnitOfWork;
using MeuTeste.Infrastructure.Data.Context;
using MeuTeste.Infrastructure.Data.UnitOfWork;
using MeuTeste.Infrastructure.Services.Caching;
using MeuTeste.Presentation.Configuration;
using MeuTeste.Presentation.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using StackExchange.Redis;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ============================================
// CONFIGURAÇÃO DE LOGGING COM SERILOG
// ============================================
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/app-.txt", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .Enrich.WithEnvironmentName()
    .CreateLogger();

builder.Host.UseSerilog();

try
{
    // ============================================
    // CONFIGURAÇÃO DO BANCO DE DADOS
    // ============================================
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

    builder.Services.AddDbContext<MeuTesteDbContext>(options =>
        options.UseSqlServer(connectionString));

    // ============================================
    // CONFIGURAÇÃO DE REDIS CACHE
    // ============================================
    var redisConnection = builder.Configuration.GetConnectionString("Redis") ?? "localhost:6379";
    try
    {
        var redis = ConnectionMultiplexer.Connect(redisConnection);
        builder.Services.AddSingleton<IConnectionMultiplexer>(redis);
        builder.Services.AddScoped<ICacheService, RedisCacheService>();
        Log.Information("? Redis conectado com sucesso");
    }
    catch (Exception ex)
    {
        Log.Warning($"? Não foi possível conectar ao Redis: {ex.Message}. Cache desabilitado.");
        // Fallback: usar cache em memória se Redis não estiver disponível
        builder.Services.AddMemoryCache();
    }

    // ============================================
    // CONFIGURAÇÃO DE AUTENTICAÇÃO JWT
    // ============================================
    var jwtSettings = builder.Configuration.GetSection("Jwt");
    var secretKey = jwtSettings["SecretKey"];
    var issuer = jwtSettings["Issuer"];
    var audience = jwtSettings["Audience"];

    if (string.IsNullOrEmpty(secretKey))
        throw new InvalidOperationException("JWT SecretKey is not configured");

    var key = Encoding.UTF8.GetBytes(secretKey);

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

    // ============================================
    // REGISTRAR FLUENT VALIDATION
    // ============================================
    builder.Services.AddValidatorsFromAssembly(typeof(CategoryInputDtoValidator).Assembly);
    builder.Services.AddValidatorsFromAssembly(typeof(ProductInputDtoValidator).Assembly);

    // ============================================
    // REGISTRAR SERVIÇOS (JWT, AUTENTICAÇÃO)
    // ============================================
    builder.Services.AddScoped<IJwtTokenService>(sp =>
        new JwtTokenService(secretKey, issuer ?? "MeuTesteApi", audience ?? "MeuTesteClient"));
    builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

    // ============================================
    // REGISTRAR SERVIÇOS DA APPLICATION
    // ============================================
    builder.Services.AddScoped<ICategoryService, CategoryService>();
    builder.Services.AddScoped<IProductService, ProductService>();

    // ============================================
    // REGISTRAR UNIT OF WORK
    // ============================================
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

    // ============================================
    // ADICIONAR CONTROLLERS
    // ============================================
    builder.Services.AddControllers();

    // ============================================
    // CONFIGURAR API VERSIONING
    // ============================================
    builder.Services.AddApiVersioningConfig();

    // ============================================
    // CONFIGURAÇÃO DO SWAGGER/OPENAPI
    // ============================================
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "MeuTeste API",
            Version = "v1",
            Description = "API REST com Arquitetura em Camadas, JWT, Cache Redis, Rate Limiting e Versionamento",
            Contact = new Microsoft.OpenApi.Models.OpenApiContact
            {
                Name = "Seu Nome",
                Email = "seu.email@example.com"
            },
            License = new Microsoft.OpenApi.Models.OpenApiLicense
            {
                Name = "MIT",
                Url = new Uri("https://opensource.org/licenses/MIT")
            }
        });

        // Adicionar segurança JWT ao Swagger
        c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\""
        });

        c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
        {
            {
                new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Reference = new Microsoft.OpenApi.Models.OpenApiReference
                    {
                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    });

    // ============================================
    // CONFIGURAÇÃO CORS
    // ============================================
    builder.Services.AddCors(options =>
    {
        // Policy para Development - Permite todas as requ sitições de localhost
        options.AddPolicy("AllowLocalhostDev",
            builder =>
            {
                builder
                    .SetIsOriginAllowed(origin =>
                    {
                        // Aceita qualquer porta em localhost (http e https)
                        return origin.Contains("localhost") || 
                               origin.Contains("127.0.0.1") ||
                               origin.Contains("::1"); // IPv6 localhost
                    })
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });

        // Policy para produção - Apenas Angular
        options.AddPolicy("AllowAngular",
            builder =>
            {
                builder.WithOrigins(
                    "http://localhost:4200",
                    "https://localhost:4200",
                    "http://localhost:4201",
                    "https://localhost:4201",
                    "http://localhost:4202",
                    "https://localhost:4202"
                )
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials();
            });

        // Policy para testes - Permite tudo (APENAS DESENVOLVIMENTO!)
        options.AddPolicy("AllowAll",
            builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
    });

    var app = builder.Build();

    // ============================================
    // APLICAR MIGRATIONS E SEED DATA
    // ============================================
    try
    {
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<MeuTesteDbContext>();

            // Criar banco de dados se não existir
            await dbContext.Database.EnsureCreatedAsync();

            // Aplicar migrations
            await dbContext.Database.MigrateAsync();

            Log.Information("? Banco de dados criado e migrations aplicadas com sucesso!");
        }
    }
    catch (Exception ex)
    {
        Log.Error(ex, "? Erro ao criar/migrar banco de dados");
    }

    // ============================================
    // CONFIGURAR PIPELINE HTTP
    // ============================================
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "MeuTeste API v1");
            c.RoutePrefix = "swagger"; // Acessar em /swagger
        });
    }

    // ============================================
    // APLICAR MIDDLEWARES
    // ============================================
    app.UseMiddleware<ErrorHandlingMiddleware>();
    app.UseMiddleware<ValidationExceptionMiddleware>();
    app.UseMiddleware<RateLimitMiddleware>();

    app.UseHttpsRedirection();

    // ============================================
    // USAR CORS
    // ============================================
    app.UseCors("AllowLocalhostDev");

    // ============================================
    // AUTENTICAÇÃO E AUTORIZAÇÃO
    // ============================================
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    Log.Information("?? Aplicação iniciada com sucesso!");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "?? Aplicação encerrada com erro fatal");
}
finally
{
    Log.CloseAndFlush();
}
