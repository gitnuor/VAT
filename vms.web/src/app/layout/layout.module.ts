import { NgModule, ModuleWithProviders } from "@angular/core";
import { CommonModule, DatePipe } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { ReactiveFormsModule } from "@angular/forms";
import { RouterModule } from "@angular/router";
import { LeftpanelComponent } from "./leftpanel/leftpanel.component";
import { HeaderComponent } from "./header/header.component";
import { FooterComponent } from "./footer/footer.component";
import { LayoutComponent } from "./layout.component";

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    RouterModule,
    
  ],
  declarations: [
    LeftpanelComponent,
    HeaderComponent,
    FooterComponent,
    LayoutComponent
  ],
 exports:[],
 providers:[]
})
export class LayoutModule { }
