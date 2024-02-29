import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { ShortUrl } from '../../models/short-url.model';
import { environment } from '../../../environments/environment';


@Injectable({
  providedIn: 'root'
})
export class UrlShortenerService {
  private endPoint: string = environment.endpoint;

  constructor(private httpClient: HttpClient) {}

  public getList(): Observable<ShortUrl[]> {
    return this.httpClient.get<ShortUrl[]>(`${this.endPoint}/api/ShortUrl/get`).pipe(
      catchError(error => {
        console.error('Error getting short URLs:', error);
        return throwError(error);
      })
    );
  }

  public getById(id: number): Observable<ShortUrl> {
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
    return this.httpClient.delete<any>(`${this.endPoint}/api/ShortUrl/delete/${id}`)
      .pipe(
        catchError(error => {
          console.error('Error deleting short URL:', error);
          return throwError(error);
        })
      );
  } 
}
