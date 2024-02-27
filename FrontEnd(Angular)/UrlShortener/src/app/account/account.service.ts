import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private endPoint = environment.endpoint;

  constructor(private http: HttpClient) { }

  // login(email: string, password: string, rememberMe: boolean): Observable<any> {
  //   debugger
  //   const body = { Email: email, Password: password, RememberMe: rememberMe };

  //   return this.http.post<any>(`${this.endPoint}/Identity/Account/Login`, body).pipe(
  //     catchError(error => {
  //       return throwError(error);
  //     })
  //   );
  // }

  // login(email: string, password: string, rememberMe: boolean): Observable<any> {
  //   debugger
  //   const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
  //   //const body = JSON.stringify("/");
  //   const body = { returnUrl: JSON.stringify("/"), Email: email, Password: password, RememberMe: rememberMe };

  //   return this.http.post<any>(`${this.endPoint}/Identity/Account/Login`, body, { headers }).pipe(
  //     catchError(error => {
  //       return throwError(error);
  //     })
  //   );
  // }

  
  login(email: string, password: string, rememberMe: boolean): Observable<any> {
    debugger
    const formData = new FormData();
    formData.append('Input.Email', email);
    formData.append('Input.Password', password);
    formData.append('Input.RememberMe', rememberMe.toString());
    // Include the __RequestVerificationToken if needed
    formData.append('__RequestVerificationToken', 'CfDJ8Lkm5o0rKc9Hjz03jzAqufVbFfGFd_48kFG9Ad_gp5CIdJIgiPUOBrCYWHjOz8Y9sSgeL4SG2P2JY7xSei5ZQkdrhc1sNweUyOxS-IFMu2DKdCczDCrI74lEuVU6QpHEjxiZSnvobWteelkLDLutETk');

    return this.http.post('https://localhost:5104/Identity/Account/Login', formData);
  }

  // register(email: string, password: string): Observable<any> {
  //   debugger
  //   return this.http.post<any>(`${this.endPoint}/Identity/Account/Register`, { email, password });
  // }

    public register(): Observable<any> {
      debugger
      const returnUrl = "http://localhost:4200";
      const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
      const body = JSON.stringify(returnUrl);
  
      return this.http.post<any>(`${this.endPoint}/Identity/Account/Register?returnUrl=${returnUrl}`, body, { headers }).pipe(
        catchError(error => {
          return throwError(error); 
        })
      );
    } 
  } 

