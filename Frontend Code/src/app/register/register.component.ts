import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  user = {
    userName: '',
    email: '',
    passwordHash: '',
    phoneNumber: '',
    userRole: 'Customer', // Default to 'Customer'
  };

  errorMessage: string | null = null;
  successMessage: string | null = null;

  constructor(private userService: UserService, private router: Router) {}

  onSubmit(): void {
    console.log('Form submitted', this.user);
    if (this.user.userName && this.user.email && this.user.passwordHash && this.user.userRole) {
      this.userService.register(this.user).subscribe({
        next: (response) => {
          console.log('Registration successful', response);
          this.successMessage = 'Registration successful! Redirecting to login...';
          this.errorMessage = null; // Clear any previous errors
          setTimeout(() => this.router.navigate(['/login']), 2000); // Redirect after 2 seconds
        },
        error: (error) => {
          this.errorMessage = 'Registration failed: ' + (error.error?.message || 'An unknown error occurred.');
          this.successMessage = null; // Clear any previous success messages
          console.error('Error occurred during registration', error);
        },
      });
    }
  }
}