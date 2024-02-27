import { Component } from '@angular/core';
import { AccountService } from '../account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
  email!: string;
  password!: string;

  constructor(private accountService: AccountService) { }

  register(): void {
    this.accountService.register().subscribe(
      response => {
        // Handle successful registration
        console.log('Registration successful:', response);
      },
      error => {
        // Handle registration error
        console.error('Registration error:', error);
      }
    );
  }
}
