import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BookingService {
  private baseUrl = 'https://localhost:7038/api'; // Adjust URL as needed

  constructor(private http: HttpClient) { }

  // Get available seats based on schedule ID
  getSeats(scheduleId: number): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/seatallocations/schedule/${scheduleId}`);
  }

  // Post booking data
  createBooking(bookingData: any): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/Bookings/create`, bookingData);
  }
  getBookingsByUserId(userID: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/BookingsController1/${userID}`);
  }
  cancelBooking(bookingID: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/Cancellations/cancel-booking/${bookingID}`);
  }
}
