import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from './auth-service.service';
import { inject } from '@angular/core';

export const roleGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService); // Using `inject` to access services
  const router = inject(Router);
  const requiredRole = route.data?.['requiredRole']; // Access route data to get the required role

  if (authService.hasRole(requiredRole)) {
    return true;
  } else {
    router.navigate(['/forbidden']); // Redirect to forbidden if the role doesn't match
    return false;
  }
};