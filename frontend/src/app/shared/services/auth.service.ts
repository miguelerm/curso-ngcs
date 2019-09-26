import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  public user: IAuthentication;
  public permissions: string[] = [];

  constructor(private readonly http: HttpClient) { }

  public hasPermission(permission: string): boolean {
    return this.permissions.indexOf(permission) >= 0;
  }

  public hasAnyPermission(permissions: string[]): boolean {
    for (const permission of permissions) {
      if (this.hasPermission(permission)) {
        return true;
      }
    }
    return false;
  }

  public load(): Promise<any> {

    return new Promise((resolve, reject) => {

      const success = (result: IAuthentication) => {
        if (result.isAuthenticated) {
          this.user = result;
          this.permissions = ['edit-book', 'create-book'];

          resolve(result);
        } else {
          this.user = result;
          resolve(result);
          //reject('user not authenticated');
        }
      };

      const fail = (error: any) => {
        reject('error ' + (error || 'unknown'));
      };

      this.http.get<IAuthentication>('/api/authentication', { withCredentials: true})
        .subscribe(success, fail);
    });

  }
}

export interface IAuthentication {
  isAuthenticated: boolean;
  name?: string;
  claims?: {
    name: string;
    givenname: string;
    emailaddress: string;
  };
}
