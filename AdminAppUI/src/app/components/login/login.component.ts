import { Component } from '@angular/core';
import { NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { LoginRequest } from '../../models/login-request.model';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [NgIf, FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  credentials = new LoginRequest();
  errorMessage: string | null = null;

  constructor(private router: Router, private authService: AuthService) { }

  login(): void {
    this.errorMessage = null;

    this.authService.login(this.credentials).subscribe(
      () => {
        this.router.navigate(['/companies']);
      },
      error => {
        this.errorMessage = 'Invalid username or password';
      }
    );
  }
}
