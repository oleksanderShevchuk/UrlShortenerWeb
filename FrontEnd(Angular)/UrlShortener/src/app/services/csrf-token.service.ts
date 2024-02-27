import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CsrfTokenService {

  constructor(private http: HttpClient) { }

  getCsrfToken(): Observable<string> {
    // Adjust the URL to match your server's endpoint for retrieving the CSRF token
    return this.http.get<string>('your-csrf-token-endpoint-url');
  }
}