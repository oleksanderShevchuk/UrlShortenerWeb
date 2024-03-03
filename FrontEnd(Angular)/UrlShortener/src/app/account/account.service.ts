import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ReplaySubject, map, of } from 'rxjs';
import { environment } from '../../environments/environment';
import { User } from '../models/account/user.model';
import { Router } from '@angular/router';
import { Login } from '../models/account/login.model';
import { Register } from '../models/account/register.model';
import { isPlatformBrowser } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private userSource = new ReplaySubject<User | null>(1);
  user$ = this.userSource.asObservable();
  private isAuthenticated: boolean = false;

  constructor(
    private http: HttpClient, 
    private router: Router, 
    @Inject(PLATFORM_ID) private platformId: Object,) { }
  
  refreshUser(jwt: string | null) {
    if (jwt === null) {
      this.userSource.next(null);
      return of(undefined);
    }

    let headers = new HttpHeaders();
    headers = headers.set('Authorization', 'Bearer ' + jwt);

    return this.http.get<User>(`${environment.endpoint}/api/account/refresh-user-token`, {headers}).pipe(
      map((user: User) => {
        if (user) {
          this.setUser(user);
        }
      })
    )
  }

  login(model: Login) {
    return this.http.post<User>(`${environment.endpoint}/api/account/login`, model)
    .pipe(
      map((user: User) => {
        if (user) {
          // Assuming the JWT token is returned from the server as a property named 'token'
          const jwt = user.jwt;
          if (jwt) {
            user.jwt = jwt; // Set the JWT token in the user object
          }
          this.setUser(user);
        }
      })
    );
  }

  logout() {
    localStorage.removeItem(environment.userKey);
    this.userSource.next(null);
    this.router.navigateByUrl('/');
    this.isAuthenticated = false;
  }

  register(model: Register) {
    return this.http.post(`${environment.endpoint}/api/account/register`, model);
  }

  getJWT() {
    if (isPlatformBrowser(this.platformId)) {
      const key = localStorage.getItem(environment.userKey);
      if (key) {
        const user: User = JSON.parse(key);
        return user.jwt;
      }
    }
    return null;
  }

  private setUser(user: User) {
    localStorage.setItem(environment.userKey, JSON.stringify(user));
    this.userSource.next(user);
    this.isAuthenticated = true;
  }

  public isAuthenticatedUser(): boolean {
    return this.isAuthenticated;
  }

  getUserId(): string | null {
    const userStr = localStorage.getItem(environment.userKey);
    if (userStr) {
      const user: User = JSON.parse(userStr);
      if (user.id) {
        return user.id;
      }
    }
    return null;
  }
  getUserRole(): string | null {
    if (typeof localStorage !== 'undefined') {
      const userStr = localStorage.getItem(environment.userKey);
      if (userStr) {
        try {
          const user: User = JSON.parse(userStr);
          if (user && user.role) {
            return user.role;
          }
        } catch (error) {
          console.error('Error parsing user data from localStorage:', error);
        }
      }
    } else {
      console.error('localStorage is not available.');
    }
    return null;
  }  
}