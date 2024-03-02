import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable, map } from 'rxjs';
import { SharedService } from '../shared.service';
import { AccountService } from '../../account/account.service';
import { User } from '../../models/account/user.model';

@Injectable({
  providedIn: 'root'
})
export class AuthorizationGuard {
  private authToken: string | null = null;

  constructor(private accountService: AccountService,
    private sharedService: SharedService,
    private router: Router) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> {
    return this.accountService.user$.pipe(
      map((user: User | null) => {
        if (user) {
          return true;
        } else {
          this.sharedService.showNotification(false, 'Restricted Area', 'Leave immediately!');
          this.router.navigate(['account/login'], {queryParams: {returnUrl: state.url}});
          return false;
        }
      })
    );
  }
}