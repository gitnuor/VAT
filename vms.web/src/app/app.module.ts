import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { LoginComponent } from './account/login/login.component';
import { AuthenticatedHttpService } from './services/authenticated-http.service';
import { Http } from '@angular/http';
import { AppRoutingModule } from "../app/app.routing.module";
import { AuthService } from '../app/services/account/auth.service';
import { RefreshTokenService } from '../app/services/refresh-token.service';
import { GlobalEmitterService } from '../app/services/global-emitter.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { HttpClient } from '../app/services/http-client.service';
import { HttpClientModule } from '@angular/common/http';
import { DashboardComponent } from './dashboard/dashboard.component';
import { MaterialComponent } from './material/material.component';
import { LayoutModule } from '../app/layout/layout.module';
// import { HeaderComponent } from './layout/header/header.component';
// import { FooterComponent } from './layout/footer/footer.component';
// import { LeftpanelComponent } from './layout/leftpanel/leftpanel.component';
//import { LayoutComponent } from "./layout/layout.component";
@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    DashboardComponent,
    MaterialComponent,
    //LayoutComponent
    // HeaderComponent,
    // FooterComponent,
    // LeftpanelComponent
    
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpModule,
    HttpClientModule,
    LayoutModule
  ],
  providers: [
    AuthenticatedHttpService,
    { provide: Http, useClass: AuthenticatedHttpService },
    AuthService,
    RefreshTokenService,
    GlobalEmitterService,
    HttpClient,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
