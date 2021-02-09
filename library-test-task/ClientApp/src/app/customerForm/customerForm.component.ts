import {Component, Inject} from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { HttpClient } from "@angular/common/http";

@Component({
  selector: 'app-customer-form',
  templateUrl: './customerForm.component.html',
})
export class CustomerFormComponent {
  customerForm = this.formBuilder.group({
    lastName: '',
    firstName: '',
    patronymic: '',
    birthDate: '',
    address: '',
  });

  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string,
    private router: Router,
    private formBuilder: FormBuilder,
  ) {
  }

  onSubmit(): void {
    this.http.post(this.baseUrl + 'customer/add', this.customerForm.value).subscribe(result => {
      this.router.navigate(['/customers']);
    }, error => console.error(error));
  }
}
