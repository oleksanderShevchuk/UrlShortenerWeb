import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ReplaySubject, map, of } from 'rxjs';
import { environment } from '../../environments/environment';
import { User } from '../models/user.model';
import { Router } from '@angular/router';
import { Login } from '../models/login.model';
import { Register } from '../models/register.model';
import { isPlatformBrowser } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private userSource = new ReplaySubject<User | null>(1);
  user$ = this.userSource.asObservable();
  private isAuthenticated: boolean = false;
  private userId: string | null = null;

  constructor(
    private http: HttpClient, 
    private router: Router, 
    @Inject(PLATFORM_ID) private platformId: Object) { }
  
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
    this.userId = null;
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
      //return user.userId;
    }
    return null;
  }
}