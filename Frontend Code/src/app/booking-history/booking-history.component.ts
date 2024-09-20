import { Component, OnInit } from '@angular/core';
import { BookingService } from '../services/booking.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HeaderComponent } from '../header/header.component';

@Component({
  selector: 'app-booking-history',
  standalone: true,
  imports: [CommonModule,FormsModule,HeaderComponent],
  templateUrl: './booking-history.component.html',
  styleUrl: './booking-history.component.css'
})

export class BookingHistoryComponent implements OnInit {

  pastBookings: any[] = [];
  upcomingBookings: any[] = [];

  constructor(private bookingService: BookingService) {}

  ngOnInit(): void {
    this.getBookingsByUserId(parseInt(localStorage.getItem('userId')!, 10)); // Replace with dynamic userID as needed
  }

  getBookingsByUserId(userID: number): void {
    this.bookingService.getBookingsByUserId(userID)
      .subscribe((bookings: any[]) => {
        const currentDate = new Date();
        bookings.forEach(booking => {
          const departureDate = new Date(booking.transportSchedule.departureTime);
          if (departureDate < currentDate) {
            this.pastBookings.push(booking);
          } else {
            this.upcomingBookings.push(booking);
          }
        });
      });
  }
  cancelBooking(bookingID: number) {
    if (confirm('Are you sure you want to cancel this booking?')) {
      this.bookingService.cancelBooking(bookingID).subscribe(
        (response: any) => {
          alert('Booking cancelled successfully.');

        },
        (error: any) => {
          alert('Failed to cancel booking. Please try again.');
        }
      );
    }
  }
}

