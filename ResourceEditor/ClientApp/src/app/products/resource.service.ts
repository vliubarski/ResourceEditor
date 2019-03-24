import { Injectable } from '@angular/core';
import { Product } from './product';
import { Observable, of, BehaviorSubject, throwError } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, tap, map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ResourceService {
  private products: Product[];
  private productsUrl = 'api/SampleData/products';

  constructor(private http: HttpClient) { }

  getProducts(lookFor: string): Observable<Product[]> {
    //if (this.products) {
    //  return of(this.products);
    //}
    return this.http.get<Product[]>(this.productsUrl+'/'+lookFor)
      .pipe(
        tap(data => console.log(JSON.stringify(data))),
        tap(data => this.products = data),
        catchError(this.handleError)
      );
  }

  private handleError(err) {
    // in a real world app, we may send the server to some remote logging infrastructure
    // instead of just logging it to the console
    let errorMessage: string;
    if (err.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      errorMessage = `An error occurred: ${err.error.message}`;
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      errorMessage = `Backend returned code ${err.status}: ${err.body.error}`;
    }
    console.error(err);
    return throwError(errorMessage);
  }
}
