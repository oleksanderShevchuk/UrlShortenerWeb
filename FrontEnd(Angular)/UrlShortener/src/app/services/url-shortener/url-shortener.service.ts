import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { ShortUrl } from '../../models/short-url/short-url.model';
import { environment } from '../../../environments/environment';
import { AccountService } from '../../account/account.service';


@Injectable({
  providedIn: 'root'
})
export class UrlShortenerService {
  private endPoint: string = environment.endpoint;

  constructor(private httpClient: HttpClient,
    private accountService: AccountService,) {}

  public getList(): Observable<ShortUrl[]> {
    return this.httpClient.get<ShortUrl[]>(`${this.endPoint}/api/ShortUrl/get`).pipe(
      catchError(error => {
        console.error('Error getting short URLs:', error);
        return throwError(error);
      })
    );
  }

  public getShortUrlById(id: number): Observable<ShortUrl> {
    return this.httpClient.get<ShortUrl>(`${this.endPoint}/api/ShortUrl/get-by-id/${id}`)
      .pipe(
        catchError(error => {
          console.error('Error getting short URL by ID:', error);
          return throwError(error);
        })
      );
  }

  public create(originalUrl: string): Observable<any> {
    const body = JSON.stringify( originalUrl );
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.httpClient.post<any>(`${this.endPoint}/api/ShortUrl/create`, body, { headers }).pipe(
      catchError(error => {
        console.error('Error creating short URL:', error);
        return throwError(error); 
      })
    );
  } 

  public delete(id: number): Observable<any> {
    // Obtain the JWT token from the account service
    const token = this.accountService.getJWT();
    if (!token) {
      throw new Error('Authentication token is missing');
    }
    // Construct the request headers with the authorization token
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    return this.httpClient.delete<any>(`${this.endPoint}/api/ShortUrl/delete/${id}`, { headers })
      .pipe(
        catchError(error => {
          console.error('Error deleting short URL:', error);
          return throwError(error);
        })
      );
  }

  checkUrlExists(originalUrl: string): Observable<any> {
    const body = JSON.stringify( originalUrl );
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.httpClient.post<any>(`${this.endPoint}/api/ShortUrl/check-url-exists`, body, { headers }).pipe(
      catchError(error => {
        console.error('Error checking by existing short URL:', error);
        return throwError(error); 
      })
    );
  } 
}
