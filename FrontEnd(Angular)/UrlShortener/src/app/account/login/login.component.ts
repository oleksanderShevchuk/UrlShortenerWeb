import { Component } from '@angular/core';
import { AccountService } from '../account.service';
import { CsrfTokenService } from '../../services/csrf-token.service';

interface LoginResponse {
  returnUrl: string; // Adjust the type as per your actual response structure
  // Add other properties if needed
}

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})

export class LoginComponent {
  email: string = '';
  password: string = '';
  rememberMe: boolean = false;
  returnUrl: string = '/'; // Default return URL
  csrfToken: string = '';

  constructor(private accountService: AccountService, private csrfTokenService: CsrfTokenService) { }

  // login(): void {
  //   this.accountService.login(this.email, this.password, this.rememberMe).subscribe(
  //     response => {
  //       // Handle successful login
  //       console.log('Login successful:', response);
  //     },
  //     error => {
  //       // Handle login error
  //       console.error('Login error:', error);
  //     }
  //   );
  // }
  onLogin(): void {
    this.accountService.login(this.email, this.password, this.rememberMe).subscribe(
      (response) => {
        console.log('Login successful:', response);
        // Handle successful login, e.g., redirect to another page
      },
      (error) => {
        console.error('Login error:', error);
        // Handle login error
      }
    );
  }


  // onSubmit(): void {
  //   this.accountService.login().subscribe(
  //     response => {
  //       console.log('Login successful:', response);
  //       // Redirect to appropriate page after successful login
  //     },
  //     error => {
  //       console.error('Login error:', error);
  //       this.errorMessage = error.error || 'An error occurred while logging in.';
  //       // Handle error, display error message, etc.
  //     }
  //   );
  // }
}
