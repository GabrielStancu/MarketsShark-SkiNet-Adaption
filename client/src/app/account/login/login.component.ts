import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { AccountService } from '../account.service';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  returnUrlUser: string;
  returnUrlAdmin: string;

  constructor(private accountService: AccountService, private router: Router, private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void{
    console.log('login reached...');
    this.returnUrlUser = this.activatedRoute.snapshot.queryParams.returnUrl || '/shop';
    this.returnUrlAdmin = '/test-error';
    this.createLoginForm();
  }

  createLoginForm(): void{
    this.loginForm = new FormGroup({
      email: new FormControl('', [Validators.required, Validators
        .pattern('^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$')]),
      password: new FormControl('', Validators.required)
    });
  }

  onSubmit(): void{
    this.accountService.login(this.loginForm.value).subscribe(() => {
      if (this.loginForm.value.email === this.accountService.adminEmail){
        this.router.navigateByUrl(this.returnUrlAdmin);
      } else {
        this.router.navigateByUrl(this.returnUrlUser);
      }
    }, error => {
      console.log(error);
    });
  }

}
