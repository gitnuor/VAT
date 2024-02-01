import { NgModule } from '@angular/core';
import { Routes, RouterModule} from '@angular/router';
import { LoginComponent } from '../app/account/login/login.component';
import { AppComponent } from './app.component';
import { LayoutComponent } from './layout/layout.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { MaterialComponent } from './material/material.component';


const appRoutes: Routes = [
    { path: 'login', component: LoginComponent },
    {
      path: 'dashboard',
      component: LayoutComponent,
      children: [
        {
          outlet: 'layout',
          path: '',
          component: DashboardComponent
        }
      ]
    },
    {
      path: 'material',
      component: LayoutComponent,
      children: [
        {
          outlet: 'layout',
          path: '',
          component: MaterialComponent
        }
      ]
    },
    {
      path: '',
      redirectTo: '/dashboard',
      pathMatch: 'full'
    }
  ];


@NgModule({
    imports: [
        RouterModule.forRoot(appRoutes)
    ],
    exports: [RouterModule],
    providers: []
})

export class AppRoutingModule { }

