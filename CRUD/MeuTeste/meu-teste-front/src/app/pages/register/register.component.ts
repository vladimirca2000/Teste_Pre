import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { AuthService } from '../../shared/services/auth.service';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
  standalone: false
})
export class RegisterComponent implements OnInit {
  form: FormGroup;
  isLoading = false;
  hidePassword = true;
  hideConfirmPassword = true;

  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {
    this.form = this.formBuilder.group({
      username: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required]]
    }, {
      validators: this.passwordMatchValidator
    });
  }

  ngOnInit(): void {
    // Se já está logado, redireciona para home
    if (this.authService.isAuthenticated()) {
      this.router.navigate(['/home']);
    }
  }

  register(): void {
    if (this.form.invalid) {
      this.snackBar.open('Preencha todos os campos corretamente', 'Fechar', { duration: 3000 });
      return;
    }

    this.isLoading = true;

    const { username, email, password } = this.form.value;

    this.authService.register({ username, email, password }).subscribe({
      next: (response) => {
        if (response.success) {
          this.snackBar.open(
            'Registro realizado com sucesso! Aguardando aprovação do administrador.',
            'Fechar',
            { duration: 5000 }
          );
          setTimeout(() => {
            this.router.navigate(['/login']);
          }, 2000);
        } else {
          this.snackBar.open(response.message || 'Erro ao registrar', 'Fechar', { duration: 3000 });
        }
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Erro no registro:', error);
        const errorMessage = error.error?.message || 'Erro ao registrar. Tente novamente.';
        this.snackBar.open(errorMessage, 'Fechar', { duration: 3000 });
        this.isLoading = false;
      }
    });
  }

  passwordMatchValidator(control: AbstractControl): ValidationErrors | null {
    const password = control.get('password');
    const confirmPassword = control.get('confirmPassword');

    if (!password || !confirmPassword) {
      return null;
    }

    return password.value === confirmPassword.value ? null : { passwordMismatch: true };
  }

  togglePasswordVisibility(): void {
    this.hidePassword = !this.hidePassword;
  }

  toggleConfirmPasswordVisibility(): void {
    this.hideConfirmPassword = !this.hideConfirmPassword;
  }

  get username() { return this.form.get('username'); }
  get email() { return this.form.get('email'); }
  get password() { return this.form.get('password'); }
  get confirmPassword() { return this.form.get('confirmPassword'); }
}
