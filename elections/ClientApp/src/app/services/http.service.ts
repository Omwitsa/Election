import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable()
export class HttpService {
  baseUrl = 'http://localhost:50935/'
  constructor(private http: HttpClient) { }

  public getUsers(): any
  {
    return this.http.get<any>(this.baseUrl + 'api/users/getUsers');
  }

  public registerUsers(data): any
  {
    var url = this.baseUrl + 'api/users/registerUser';
    return this.http.post(url, data);
  }

  public resetPassword(data): any
  {
    var url = this.baseUrl + 'users/resetPassword';
    return this.http.post(url, data);
  }

  public getToken(): any
  {
    return this.http.get<any>(this.baseUrl + 'api/auth/getToken');
  }

  public login(data): any
  {
    // var url = this.baseUrl + 'users/login';
    // return this.http.post(url, data);
  }
}
