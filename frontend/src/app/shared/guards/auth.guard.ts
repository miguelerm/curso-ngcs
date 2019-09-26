import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private readonly http: HttpClient) {

  }
  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot):
    Observable<boolean | UrlTree> |
    Promise<boolean | UrlTree> | boolean |
    UrlTree {

      console.log('guard: ', {next, state});
      const permissions = next.data.permissions;
      return this.http.get<boolean>('/api/authentication/has-access', {params: {permissions}});

  }

}
