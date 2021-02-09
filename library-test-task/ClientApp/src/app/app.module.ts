import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { BookRentComponent } from './bookRent/bookRent.component';
import { CustomersComponent } from './customers/customers.component';
import { BooksComponent } from "./books/books.component";
import { BookFormComponent } from "./bookForm/bookForm.component";
import { CustomerFormComponent } from "./customerForm/customerForm.component";
import { LeaseBookFormComponent } from "./leaseBookForm/leaseBookForm.component";

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    BookRentComponent,
    BooksComponent,
    CustomersComponent,
    BookFormComponent,
    CustomerFormComponent,
    LeaseBookFormComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({appId: 'ng-cli-universal'}),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      {path: '', component: BookRentComponent, pathMatch: 'full'},
      {path: 'books', component: BooksComponent},
      {path: 'customers', component: CustomersComponent},
      {path: 'bookForm', component: BookFormComponent},
      {path: 'customerForm', component: CustomerFormComponent},
      {path: 'leaseBookForm', component: LeaseBookFormComponent},
    ]),
    ReactiveFormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
