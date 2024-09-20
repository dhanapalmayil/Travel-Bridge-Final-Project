import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TransportScheduleService {

  private apiUrl = 'https://localhost:7038/api/TransportSchedules'; // Update with your actual API URL

  constructor(private http: HttpClient) { }

  // Method to call the search API
  searchSchedules(from: string, to: string, date: string, mode: string): Observable<any> {
    // Prepare query parameters
    let params = new HttpParams()
      .set('from', from)
      .set('to', to)
      .set('date', date)
      .set('mode', mode);

    // Make GET request to search schedules
    return this.http.get(`${this.apiUrl}/search`, { params });
  }
  postSchedule(schedule: any) {
    return this.http.post(this.apiUrl, schedule);
  }
  createSchedule(schedule: any): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.post(`${this.apiUrl}/createschedule`, schedule, { headers });
  }
  getGroupedDetailsByProvider(providerId: number): Observable<any> {
    return this.http.get(`${this.apiUrl}\/GetGroupedDetailsByProvider/${providerId}`);
  }
 
}
