import { HttpClient, HttpParams } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';
import { Component, OnInit } from '@angular/core';
import { TransportScheduleService } from '../services/transport-schedule.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-search',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit {

  from: string = '';
  to: string = '';
  date: string = '';
  mode: string = 'bus';
  transportSchedules: any[] = [];
  errorMessage: string = '';
  minDate: string = ''; // Added to store minimum date
  buttonText: string = 'Search Buses'; // Property to store button text

  constructor(private transportScheduleService: TransportScheduleService, private http: HttpClient,private router:Router) { }

  ngOnInit(): void {
    // Set minimum date to today's date (YYYY-MM-DD format)
    this.minDate = new Date().toISOString().split('T')[0];
    // Update button text initially
    this.updateButtonText();
  }

  // Method to update the button text based on the selected mode
  updateButtonText(): void {
    switch (this.mode) {
      case 'bus':
        this.buttonText = 'Search Buses';
        break;
      case 'train':
        this.buttonText = 'Search Trains';
        break;
      case 'flight':
        this.buttonText = 'Search Flights';
        break;
      default:
        this.buttonText = 'Search'; // Default text if no mode is selected
        break;
    }
  }

  // Method to search schedules
  searchSchedules(): void {
    // Validate form fields
    if (!this.from || !this.to || !this.date || !this.mode) {
      this.errorMessage = 'All fields are required.';
      return;
    }

    // Validate date (Ensure date is not in the past)
    const selectedDate = new Date(this.date);
    const today = new Date();
    today.setHours(0, 0, 0, 0); // Reset time portion for accurate comparison

    if (selectedDate < today) {
      this.errorMessage = 'The date cannot be in the past.';
      return;
    }

    // Call the service to get transport schedules
    this.transportScheduleService.searchSchedules(this.from, this.to, this.date, this.mode)
      .pipe(
        catchError(error => {
          // Handle error here
          console.error('Error occurred:', error);
          this.errorMessage = 'No schedules found or an error occurred.';
          this.transportSchedules = [];
          return throwError(error);
        })
      )
      .subscribe(
        (data) => {
          this.transportSchedules = data;
          this.errorMessage = '';
        },
        (error) => {
          this.errorMessage = 'No schedules found or an error occurred.';
          this.transportSchedules = [];
        }
      );
  }

  // Watch for changes in the mode to update the button text accordingly
  onModeChange(): void {
    this.updateButtonText();
  }
  
  selectSchedule(schedule: any): void {
    // Store selected schedule data and navigate to booking component
    this.router.navigate(['/booking'], { queryParams: { scheduleId: schedule.scheduleID } });
  }
}
