import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  private baseUrl = 'https://localhost:7038/api/Admins';

  constructor(private http: HttpClient) {}

  // 1. User Management
  getUsers(): Observable<any> {
    return this.http.get(`${this.baseUrl}/users`);
  }

  getUser(userId: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/users/${userId}`);
  }

  updateUser(userId: number, updateData: any): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/update/${userId}`, updateData);
  }

  deleteUser(userId: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/users/${userId}`);
  }

  // 2. Provider Management
  getProviders(): Observable<any> {
    return this.http.get(`${this.baseUrl}/providers`);
  }

  getProvider(providerId: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/providers/${providerId}`);
  }

  updateProvider(providerId: number, providerData: any): Observable<any> {
    return this.http.put(`${this.baseUrl}/providers/update/${providerId}`, providerData);
  }

  deleteProvider(providerId: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/providers/${providerId}`);
  }

  // 3. Booking Management
  getBookings(): Observable<any> {
    return this.http.get(`${this.baseUrl}/bookings`);
  }

  getBooking(bookingId: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/bookings/${bookingId}`);
  }

  updateBooking(bookingId: number, bookingData: any): Observable<any> {
    return this.http.put(`${this.baseUrl}/bookings/${bookingId}`, bookingData);
  }

  // 4. Audit Logs
  getAuditLogs(): Observable<any> {
    return this.http.get(`${this.baseUrl}/auditlogs`);
  }

  getAuditLog(auditLogId: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/auditlogs/${auditLogId}`);
  }

  // 5. Reports
  getBookingReport(startDate: string, endDate: string): Observable<any> {
    return this.http.get(`${this.baseUrl}/reports/bookings?startDate=${startDate}&endDate=${endDate}`);
  }

  getRevenueReport(startDate: string, endDate: string): Observable<any> {
    return this.http.get(`${this.baseUrl}/reports/revenue?startDate=${startDate}&endDate=${endDate}`);
  }

  

  // 6. Dashboard Metrics
  getDashboardMetrics(): Observable<any> {
    return this.http.get(`${this.baseUrl}/dashboard/metrics`);
  }
}
