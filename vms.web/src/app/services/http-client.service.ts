import { Injectable } from '@angular/core';
import { RequestOptions, Headers } from '@angular/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs/Observable';
import { AuthenticatedHttpService } from '../services/authenticated-http.service';
import { GlobalEmitterService } from '../services/global-emitter.service';
import { Router } from '@angular/router';



@Injectable()
export class HttpClient {

    public BASE_URL: string;

    constructor(private _globalEmitterService: GlobalEmitterService,
        public http: AuthenticatedHttpService,
        private _router: Router) {
        this.BASE_URL = environment.client.base_url;
        this.setTokenAndHeader();
        this._globalEmitterService.emitRefreshToken.subscribe(res => {
            this.setTokenAndHeader();

        })
    }
    


    token: string;
    //  = 'OrkEW4RxWgmvs1M4t2NOLDXzKo-8R9YKyFhSXt35G_diZqLFcxjy7CUJkddCidEZA-UZmhAmtZmaHC_wPZ6i5qCfDHjmRXAeLtLQvi8n5bJNP3smqFQuP02cyXsQWXMwnMyAq57LRmvEX2-2DbR-i7IeYgsiuj2mg_z6_EVluv17ICZtT1q_NyQbUaJb30WGte5ruGaVd4RP8cj3Yi-WeqFjX2dDOfwaGA_cyVJhDPG4zsREvdOyhO8dHf6XUDQzm_vDVrHspqJpXSDJ8fJUs2bCn10uvFkPoqEJ3VSYXr4o1sPjRkwy7iseRbrl-jWrHJytAHrD_hCd7B90mw8TNGJtd_q7lqxts5FnRJ80Py21Jrv6UAdN3mZXQ6TJazDl8WsxI_DqzkrdPYkD7MbV2wgEA3a2bT05koedJEYrWSkIO-4HoFg2Uv_vXg_w2qwP_FFjIG-1pNZ70I7MZzNrKmiAONJNi8wD7Kb9VDSleeI'

    private rewrite(url: string) {

    }

    

    headers;


    setToken() {
        let user = JSON.parse(localStorage.getItem('user'));
        if (user.access_token) {
            this.token = user.access_token
        }
    }

    setTokenAndHeader() {
        let userStr = localStorage.getItem('user');
        if (userStr != undefined && userStr.length > 0) {
            let user = JSON.parse(localStorage.getItem('user'))
            if (user != undefined) {
                //console.log(user.access_token);
                this.token = user.access_token;
                this.createAuthorizationHeaders();
            }
        }
    }

    createAuthorizationHeaders() {
        let headers = new Headers();
        headers.set('Content-Type', 'application/x-www-form-urlencoded');
        // headers.set('Content-Type', 'application/json');
        headers.append('Authorization', 'bearer ' + this.token)
        this.headers = { headers };
    }

    appendAuthorizationHeader(headers: Headers) {
        this.setToken();
        headers.append('Authorization', 'bearer ' + this.token)
        return headers
    }

    get(url: string) {
        return this.http.get(this.BASE_URL + url, this.getHeaders());
    }

    post(url: string, data: any) {
        const headers = new Headers();
        // headers.set('Content-Type', 'application/json');
        return this.http.post(this.BASE_URL + url, data, this.headers);
    }

    postJson(url: string, data: any, token = '') {
        const headers = new Headers();
        return this.http.post(this.BASE_URL + url + '?', data, this.getHeaders());
    }
    put(url: string, data: any) {
        return this.http.put(this.BASE_URL + url, data, this.getHeaders());
    }

    delete(url: string, data?: any) {
        let headers = new Headers({ 'content-type': 'application/json' });
        headers = this.appendAuthorizationHeader(headers);
        const options = new RequestOptions({ headers: headers, body: data });
        return this.http.delete(this.BASE_URL + url, options);
    }

    getHeaders() {
        const headers = new Headers();
        this.setToken();
        headers.set('Content-Type', 'application/json');
        headers.append('Authorization', 'bearer ' + this.token);
        return { headers };
    }

    get BaseUrl () {
        return this.BASE_URL;
    }

    // getHeadersGet() {
        
    // }

   
      
}
