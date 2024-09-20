import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProviderService {
  private apiUrl = 'https://localhost:7038/api/TransportProviders'; // Replace with your API URL

  constructor(private http: HttpClient) {}

  postProviderData(providerData: any): Observable<any> {
    return this.http.post(this.apiUrl, providerData);
  }
}
