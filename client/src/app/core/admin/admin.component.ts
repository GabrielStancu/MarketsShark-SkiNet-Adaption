import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-test-error',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss']
})
export class AdminComponent implements OnInit {
  baseUrl = environment.apiUrl;
  validationErrors: any;
  enabled = 'enabled';
  disabled = 'disabled';
  vendors = ['emag', 'cel.ro', 'pcgarage'];
  enabledVendors = [false, false, false];

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.getCurrentEnabledVendors();
  }

  private getCurrentEnabledVendors(): void{
    for (const vendor in this.enabledVendors){
      if (localStorage.getItem(this.vendors[vendor]) === null || localStorage.getItem(this.vendors[vendor]) === this.disabled){
        this.enabledVendors[vendor] = false;
      } else {
        this.enabledVendors[vendor] = true;
      }
    }
  }

  get404Error(): void {
    this.http.get(this.baseUrl + 'products/42').subscribe(response => {
      console.log(response);
    }, error => {
      console.log(error);
    });
  }

  get500Error(): void {
    this.http.get(this.baseUrl + 'buggy/servererror').subscribe(response => {
      console.log(response);
    }, error => {
      console.log(error);
    });
  }

  get400Error(): void {
    this.http.get(this.baseUrl + 'buggy/badrequest').subscribe(response => {
      console.log(response);
    }, error => {
      console.log(error);
    });
  }

  get400ValidationError(): void {
    this.http.get(this.baseUrl + 'products/fortytwo').subscribe(response => {
      console.log(response);
    }, error => {
      console.log(error);
      this.validationErrors = error.errors;
    });
  }

  enableVendor(id: number): void{
    if (localStorage.getItem(this.vendors[id]) === null){
      localStorage.setItem(this.vendors[id], this.enabled);
    } else if (localStorage.getItem(this.vendors[id]) === this.enabled){
      localStorage.setItem(this.vendors[id], this.disabled);
    } else if (localStorage.getItem(this.vendors[id]) === this.disabled){
      localStorage.setItem(this.vendors[id], this.enabled);
    }
  }
}
