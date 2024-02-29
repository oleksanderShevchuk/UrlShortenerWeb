import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { CsrfTokenService } from '../csrf-token/csrf-token.service';

@Injectable({
  providedIn: 'root'
})
export class DataService {
  constructor(private http: HttpClient, private csrfTokenService: CsrfTokenService) { }

  getData() {
    // Fetch CSRF token
    this.csrfTokenService.getCsrfToken().subscribe(token => {
      // Include token in headers of HTTP request
      const headers = new HttpHeaders().set('X-CSRF-Token', token);

      // Make your HTTP request with the headers
      this.http.get('/api/data', { headers }).subscribe(
        (data) => {
          // Handle successful response
          console.log('Data received:', data);
        },
        (error) => {
          // Handle error response
          console.error('Error:', error);
        }
      );
    });
  }
}
