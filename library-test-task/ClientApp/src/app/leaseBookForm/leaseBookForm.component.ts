import {Component, Inject, OnInit} from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { HttpClient } from "@angular/common/http";

@Component({
  selector: 'app-lease-book-form',
  templateUrl: './leaseBookForm.component.html',
})
export class LeaseBookFormComponent implements OnInit {
  customers: Object = [];
  books: Object = [];
  customerId: number = 0;
  bookId: number = 0;

  leaseBookForm = this.formBuilder.group({
    customerId: 0,
    bookId: 0,
    planEndDate: '',
  });

  ngOnInit() {
    this.http.get(this.baseUrl + 'customer/listNames').subscribe(result => {
      this.customers = result;
    });
    this.http.get(this.baseUrl + 'book/list').subscribe(result => {
      this.books = result;
    });
  }

  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string,
    private router: Router,
    private formBuilder: FormBuilder,
  ) {
  }

  onSubmit(): void {
    this.http.post(this.baseUrl + 'bookrent/leaseOne', this.leaseBookForm.value).subscribe(result => {
      this.router.navigate(['/']);
    }, error => {
      alert(error.error);
      console.error(error)
    });
  }
}

interface Customer {
  id: number,
  lastName: string,
  firstName: string,
  patronymic: string,
}
