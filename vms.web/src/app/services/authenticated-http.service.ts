import { Injectable } from '@angular/core';
import { Request, XHRBackend, RequestOptions, Response, Http, Headers } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/map';

import { environment } from '../../environments/environment';
import { RefreshTokenService } from './refresh-token.service';

@Injectable()
export class AuthenticatedHttpService extends Http {

    constructor(private _refreshTokenService:RefreshTokenService,backend: XHRBackend, defaultOptions: RequestOptions) {
        super(backend, defaultOptions);
    }

    request(url: string | Request, options?: RequestOptions): Observable<Response> {
        return super.request(url, options)
            .catch((error: Response) => {
                if ((error.status === 401 ) && (window.location.href.match(/\?/g) || []).length < 2) {
                    let header = new Headers();
                    let base_url = environment.client.base_url;
                     header.set('Content-Type', 'application/x-www-form-urlencoded');
                     let user = JSON.parse(localStorage.getItem('user'));
                     let body = new URLSearchParams();
                     body.set('grant_type', 'refresh_token');
                     body.set('refreshtoken', user.access_token);
                     let data = body.toString();
                     return this.post(base_url + '/token', data, { headers: header }).map(result => {
                         this._refreshTokenService.setToken(result);
                       return { status: result.status, json: result.json()} || {};
                     }).catch((err) => {
                       return Observable.throw(err);
                     })
                   
                   
                }
                return Observable.throw(error);
            });

         
    }
    
}