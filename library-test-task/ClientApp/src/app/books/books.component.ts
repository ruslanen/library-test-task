import {Component, Inject, Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-books',
  templateUrl: './books.component.html'
})
@Injectable()
export class BooksComponent {
  public books: Book[];

  constructor(private http: HttpClient,  @Inject('BASE_URL') private baseUrl: string, private router: Router) {
    this.http.get<Book[]>(baseUrl + 'book/list').subscribe(result => {
      this.books = result;
    }, error => console.error(error));
  }

  delete(id) {
    this.http.post(this.baseUrl + 'book/delete', id).subscribe(result => {
      this.router.navigateByUrl('/', {skipLocationChange: true}).then(() => {
        this.router.navigate(['/books']);
      });
    }, error => console.error(error));
  }

  openForm() {
    this.router.navigate(['/bookForm']);
  }
}

interface Book {
  id: number,
  isbn: string,
  title: string,
  author: string,
  publishYear: string,
  address: string,
  total: number,
  free: number,
}
