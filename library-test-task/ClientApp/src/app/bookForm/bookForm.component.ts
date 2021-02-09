import {Component, Inject} from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { HttpClient } from "@angular/common/http";

@Component({
  selector: 'app-book-form',
  templateUrl: './bookForm.component.html',
})
export class BookFormComponent {
  bookForm = this.formBuilder.group({
    isbn: 0,
    title: '',
    author: '',
    publishYear: 0,
    total: 0,
  });
  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string,
    private router: Router,
    private formBuilder: FormBuilder,
  ) {}

  onSubmit(): void {
    this.http.post(this.baseUrl + 'book/add', this.bookForm.value).subscribe(result => {
      this.router.navigate(['/books']);
    }, error => console.error(error));
  }
}
