import { HttpService } from './http.service';
import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class InterceptorService implements HttpInterceptor{
  constructor(private httpService: HttpService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    let newHeaders = {
      // 'Authorization': `Bearer ${ 'token' }`,
      'Content-Type':  'application/json'
    };
    
    req = req.clone({
      setHeaders: newHeaders
    });

    return next.handle(req);
  }
}
