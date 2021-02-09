import {Component, Inject, Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {Router} from "@angular/router";

@Component({
  selector: 'app-customers',
  templateUrl: './customers.component.html'
})
@Injectable()
export class CustomersComponent {
  public customers: Customer[];

  constructor(private http: HttpClient,  @Inject('BASE_URL') private baseUrl: string, private router: Router) {
    this.http.get<Customer[]>(baseUrl + 'customer/list').subscribe(result => {
      this.customers = result;
    }, error => console.error(error));
  }

  delete(id) {
    this.http.post(this.baseUrl + 'customer/delete', id).subscribe(result => {
      this.router.navigateByUrl('/', {skipLocationChange: true}).then(() => {
        this.router.navigate(['/customers']);
      });
    }, error => console.error(error));
  }

  openForm() {
    this.router.navigate(['/customerForm']);
  }
}

interface Customer {
  id: number,
  lastName: string,
  firstName: string,
  patronymic: string,
  birthDate: string,
  address: string,
}
