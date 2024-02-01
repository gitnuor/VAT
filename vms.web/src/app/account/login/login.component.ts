import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/account/auth.service';
import { FormGroup } from '@angular/forms/src/model';
import { FormBuilder, Validators, AbstractControl } from '@angular/forms';
import { Router, ActivatedRoute, Params } from "@angular/router";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  title:string;

  loginForm: FormGroup;
  constructor(
    private authService:AuthService,
    private fb: FormBuilder,
    private router: Router,
  ) { }

  ngOnInit() {
    this.createForm();
    this.title = "Login";
  }
  onSubmit() {
    
    let demo = {
      "email": "support@hoxro.com",
      "password": "Password1@"
    }
    this.loginForm.setValue(demo);

    this.authService.login(this.loginForm.value).subscribe(res => {
      if (res.error) {
        
      } else {
        this.router.navigate(['dashboard']);
      }
    },
      err => {
        const obj = JSON.parse(err._body);
        console.log(obj);
      },
    );
  }
  createForm() {
    this.loginForm = this.fb.group({
      email: [
        '',
        Validators.compose([Validators.required, Validators.minLength(3), this.emailValidation()])
      ],
      password: [
        '',
        Validators.compose([Validators.required, Validators.minLength(3)])
      ]
    })
  }
  emailValidation() {
    return (control: AbstractControl) => {
      const emailRegexp = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
      let email = emailRegexp.test(control.value);
      return email ? null : { 'invalid': control.value }
    }
  }
}
