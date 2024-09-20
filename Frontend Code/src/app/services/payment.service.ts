import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PaymentService {
  processPayment(value: any) {
    throw new Error('Method not implemented.');
  }
  private apiUrl = 'https://yourapiurl.com/api/payments';  // Change this to your actual API endpoint

  constructor(private http: HttpClient) {}

  // Method to save the payment
  makePayment(payment: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/create`, payment);
  }

  // Method to get booking details based on BookingID
  getBookingDetails(bookingId: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/booking/${bookingId}`);
  }
}
