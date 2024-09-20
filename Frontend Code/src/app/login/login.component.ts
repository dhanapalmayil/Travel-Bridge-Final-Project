import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UserService } from '../services/user.service'; // Adjust path as needed
import { HttpClient } from '@angular/common/http'; // To make HTTP calls for second API

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  login = {
    email: '',
    password: '',
  };

  errorMessage: string | null = null;
  successMessage: string | null = null;

  constructor(
    private userService: UserService, 
    private router: Router,
    private http: HttpClient // To make HTTP requests for second API
  ) {}

  onSubmit(): void {
    // Check if the email and password match the admin credentials
    if (this.login.email === 'admin@gmail.com' && this.login.password === '123456') {
      localStorage.setItem('userId', 'admin'); // Set some dummy userId for admin
      localStorage.setItem('userName', 'Admin'); // Set admin name
      localStorage.setItem('role', 'admin'); // Optionally, store user role
      
      this.successMessage = 'Admin login successful! Redirecting to dashboard...';
      this.errorMessage = null; // Clear any previous errors
      setTimeout(() => this.router.navigate(['/admin-dashboard']), 2000); // Redirect after 2 seconds
      
    }
    if (this.login.email && this.login.password) {
      this.userService.login(this.login).subscribe({
        next: (response) => {
          console.log('Login successful', response);
          const userId = response.userId;
          const userName = response.userName;

          // Store the userId and userName in local storage
          localStorage.setItem('userId', userId);
          localStorage.setItem('userName', userName);
          localStorage.setItem('token',response.token);
          this.successMessage = 'Login successful! Redirecting...';
          this.errorMessage = null; // Clear any previous errors

          // Call the second API to check TransportProvider existence
          this.checkTransportProviderExists(userId);
          if(response.role==='Admin')
          {
            setTimeout(() => this.router.navigate(['/admin-dashboard']), 2000);
            return;
          }
          setTimeout(() => this.router.navigate(['/home']), 2000);
        },
        error: (error) => {
          this.errorMessage = 'Login failed: ' + (error.error?.message || 'An unknown error occurred.');
          this.successMessage = null; // Clear any previous success messages
          console.error('Error occurred during login', error);
        },
      });
    }
  }

  // Method to call the second API for TransportProvider
  checkTransportProviderExists(userId: string): void {
    const url = `https://localhost:7038/api/TransportProviders/Exists/${userId}`;
    
    this.http.get<{ providerId: string }>(url).subscribe({
      next: (response) => {
        console.log('Raw API response:', response); 
        if (response) {
          // Store the providerId in local storage
          localStorage.setItem('providerId',(response as any).providerID);
          localStorage.setItem('providerName',(response as any).providerName);
          localStorage.setItem('providerName',(response as any).providerName);
          localStorage.setItem('providerType',(response as any).providerType);
          localStorage.setItem('TransportName',(response as any).transportName);
          console.log('Provider ID found:', response.providerId);
        } else {
          console.log('No providerId found for user:', userId);
        }

        // Redirect after the API call
        setTimeout(() => this.router.navigate(['/home']), 2000); // Redirect after 2 seconds
      },
      error: (error) => {
        console.error('Error occurred during TransportProvider check', error);
      },
    });
  }
}
