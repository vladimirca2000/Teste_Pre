import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../shared/services/auth.service';
import { Router, ActivatedRoute } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  standalone: false
})
export class LoginComponent implements OnInit {
  form: FormGroup;
  isLoading = false;
  hidePassword = true;
  returnUrl: string = '';

  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar
  ) {
    this.form = this.formBuilder.group({
      username: ['', [Validators.required]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  ngOnInit(): void {
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/home';

    // Se já está logado, redireciona para home
    if (this.authService.isAuthenticated()) {
      this.router.navigate(['/home']);
    }
  }

  login(): void {
    if (this.form.invalid) {
      this.snackBar.open('Preencha todos os campos corretamente', 'Fechar', { duration: 3000 });
      return;
    }

    this.isLoading = true;

    this.authService.login(this.form.value).subscribe({
      next: (response) => {
        if (response.success) {
          this.snackBar.open('Login realizado com sucesso!', 'Fechar', { duration: 3000 });
          this.router.navigate([this.returnUrl]);
        } else {
          this.snackBar.open(response.message || 'Erro ao fazer login', 'Fechar', { duration: 3000 });
        }
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Erro no login:', error);
        this.snackBar.open('Erro ao fazer login. Verifique suas credenciais.', 'Fechar', { duration: 3000 });
        this.isLoading = false;
      }
    });
  }

  togglePasswordVisibility(): void {
    this.hidePassword = !this.hidePassword;
  }

  get username() { return this.form.get('username'); }
  get password() { return this.form.get('password'); }
}
