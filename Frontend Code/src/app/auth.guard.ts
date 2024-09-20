import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from './auth-service.service';
import { inject } from '@angular/core';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService); // Using `inject` to access services
  const router = inject(Router);

  if (authService.isAuthenticated()) {
    return true;
  } else {
    router.navigate(['/login']); // Redirect to login if not authenticated
    return false;
  }
};
