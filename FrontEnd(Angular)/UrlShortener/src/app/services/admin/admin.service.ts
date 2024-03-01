import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AccountService } from '../../account/account.service';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  constructor(private http: HttpClient,
    private accountService: AccountService) {  }
  
  deleteAllUrls(): Observable<any> {
    // Obtain the JWT token from the account service
    const token = this.accountService.getJWT();
    
    if (!token) {
      throw new Error('Authentication token is missing');
    }
    
    // Construct the request headers with the authorization token
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });

    // Make the HTTP request with the authorization token included in the headers
    return this.http.post(`${environment.endpoint}/api/Admin/urls/delete-all`, {}, { headers });
  }
}
