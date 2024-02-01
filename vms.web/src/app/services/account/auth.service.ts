import { Injectable } from '@angular/core';
import { Http, Response } from "@angular/http";
import { environment } from '../../../environments/environment';
import { Headers } from '@angular/http';
import { Observable } from "rxjs/Observable";
import { HttpClient } from "./../http-client.service";
import { Router } from "@angular/router";
import { GlobalEmitterService } from '../global-emitter.service';
import { DataStorageModel } from '../../models/data-storage-model';

@Injectable()
export class AuthService {
  constructor(
    private http: Http,
    private httpClient: HttpClient,
    private router: Router,
    private _globalEmitterService:GlobalEmitterService
  ) {
   
   
  }
ngOnInit(){
 
}

  base_url = environment.client.base_url;

  
  userInfo:any;
  ipInfo:any;
   
  
  login(user): Observable<any> {
    let loginPost = {
      username: user.email,
      password: user.password,
      grant_type: 'password'
    }

    let header = new Headers()
    header.set('Content-Type', 'application/x-www-form-urlencoded');
    //header.set('Content-Type', 'application/json');
    let body = new URLSearchParams();
    body.set('username', user.email);
    body.set('password', user.password);
    body.set('grant_type', 'password');
    DataStorageModel.userEmail = loginPost.username;
    let data = body.toString();
    return this.http.post(this.base_url + '/token', data, { headers: header }).map(result => {
      console.log(result);
      
      this.setToken(result);

      return { status: result.status, json: result.json()} || {};
    }).catch((err) => {
      return Observable.throw(err);
    })
  }
  
  setToken(result){
    localStorage.setItem('user', result["_body"])
    this.httpClient.setTokenAndHeader();
  }
 
}
