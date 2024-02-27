import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Token } from '@angular/compiler';

@Injectable()
export class AuthService {
  private endPoint: string = environment.endpoint;
  private storageKey: string = 'authInfo';

  constructor(
    private httpClient: HttpClient
  ) {}


  
}