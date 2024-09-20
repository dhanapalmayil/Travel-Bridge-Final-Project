import { Component, OnInit, Inject, PLATFORM_ID } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule, isPlatformBrowser } from '@angular/common';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  userName: string | null = null;
  isDropdownOpen = false;
  hasProviderId = false;
  hasadmin=false;

  constructor(
    private router: Router,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {}

  ngOnInit() {
    if (isPlatformBrowser(this.platformId)) {
      this.userName = localStorage.getItem('userName');
      this.hasProviderId = !!localStorage.getItem('providerId'); // Set flag based on presence of providerId
      this.hasadmin=!!localStorage.getItem('role');
    }
    
  }

  toggleDropdown() {
    this.isDropdownOpen = !this.isDropdownOpen;
  }

  goToHome() {
    this.router.navigate(['/home']);
  }

  goToLogin() {
    this.router.navigate(['/login']);
  }

  goToRegister() {
    this.router.navigate(['/register']);
  }

  goToBooking() {
    this.router.navigate(['/booking-history']);
  }

  goToSchedule() {
    this.router.navigate(['/create-schedule']);
  }
  goToScheduleHistory() {
    this.router.navigate(['/schedule-list']);
  }

  logout() {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.removeItem('userName');
      localStorage.removeItem('providerId');
      this.userName = null;
      this.isDropdownOpen = false;
      this.router.navigate(['/home']);
    }
  }
}
