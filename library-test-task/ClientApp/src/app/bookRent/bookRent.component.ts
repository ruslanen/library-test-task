import {Component, Inject, Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {Router} from "@angular/router";
import {FormBuilder} from "@angular/forms";

@Component({
  selector: 'app-bookRent',
  templateUrl: './bookRent.component.html',
})
@Injectable()
export class BookRentComponent {
  public bookRent: BookRent[];

  bookRentFilterForm = this.formBuilder.group({
    nameFilter: '',
    bookFilter: '',
    rentStatusFilter: '',
  });

  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string,
    private router: Router,
    private formBuilder: FormBuilder) {
    this.http.get<BookRent[]>(baseUrl + 'bookRent/list').subscribe(result => {
      this.bookRent = result;
    }, error => console.error(error));
  }

  releaseBook(id) {
    this.http.post(this.baseUrl + 'bookRent/releaseOne', id).subscribe(result => {
      // TODO: Нормально обновлять компонент
      location.reload();
    }, error => console.error(error));
  }

  openLeaseBookForm() {
    this.router.navigate(['/leaseBookForm']);
  }

  onFilterSubmit() {
    this.http.get<BookRent[]>(this.baseUrl + 'bookRent/list', { params: this.bookRentFilterForm.value}).subscribe(result => {
      this.bookRent = result;
    },error => console.error(error));
  }

  resetFilter() {
    this.http.get<BookRent[]>(this.baseUrl + 'bookRent/list').subscribe(result => {
      this.bookRent = result;
    },error => console.error(error));
  }
}

interface BookRent {
  id: number,
  customerId: number,
  customer: object,
  bookId: number,
  book: object,
  startRent: string,
  planEndRent: string,
  factEndRent: string,
}
