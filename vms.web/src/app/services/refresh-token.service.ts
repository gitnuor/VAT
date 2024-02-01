import { Injectable } from '@angular/core';
import { GlobalEmitterService } from '../services/global-emitter.service';

@Injectable()
export class RefreshTokenService {

  constructor(private _globalEmitterService:GlobalEmitterService) { }
  setToken(result){
    localStorage.setItem('user', result["_body"])
   this._globalEmitterService.sendRefreshToken(result)
  }
}
