import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, Subject, catchError, throwError } from 'rxjs';
import { ShortUrl } from '../models/short-url.model';
import { environment } from '../../environments/environment';


@Injectable({
  providedIn: 'root'
})
export class UrlShortenerService {
  private endPoint: string = environment.endpoint;

  constructor(private httpClient: HttpClient) {}
  
  public getList(): Subject<ShortUrl[]> {
    const result: Subject<ShortUrl[]> = new Subject<ShortUrl[]>();

    this.httpClient.get(this.endPoint + '/ShortUrl/Get').subscribe(
      (data: any) => {
      result.next(data);
    },
    err => {
      result.next([]);
    }
  );
    return result;
  }

  public create(originalUrl: string): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const body = JSON.stringify(originalUrl);

    return this.httpClient.post<any>(`${this.endPoint}/ShortUrl/Create`, body, { headers }).pipe(
      catchError(error => {
        return throwError(error); 
      })
    );
  } 

}
