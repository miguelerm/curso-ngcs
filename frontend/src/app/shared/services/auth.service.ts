import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  public user: IAuthentication;

  constructor(private readonly http: HttpClient) { }

  public load(): Promise<any> {

    return new Promise((resolve, reject) => {

      const success = (result: IAuthentication) => {
        if (result.isAuthenticated) {
          this.user = result;
          resolve(result);
        } else {
          reject('user not authenticated');
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
  name: string;
  claims: {
    name: string;
    givenname: string;
    emailaddress: string;
  };
}
