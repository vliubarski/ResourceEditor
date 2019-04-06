import { Injectable } from '@angular/core';
import { Product } from './product';
import { Observable, of, BehaviorSubject, throwError } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, tap, map } from 'rxjs/operators';
import { RequestOptions } from '@angular/http';
@Injectable({
  providedIn: 'root'
})
export class ResourceService {
  //private products: Product[];
  private getProductsUrl = 'api/SampleData/products';
  private saveProductsUrl = 'api/SampleData/saveProducts';

  constructor(private http: HttpClient) { }

  getProducts(lookFor: string): Observable<Product[]> {
    //if (this.products) {
    //  return of(this.products);
    //}
    return this.http.get<Product[]>(this.getProductsUrl + '/' + lookFor)
      .pipe(
        tap(data => console.log(JSON.stringify(data))),
        //tap(data => this.products = data),
        catchError(this.handleError)
      );
  }

  saveProducts(newResource: Product): Observable<Product> {
    //let headers = new Headers({ 'Content-Type': 'application/json' });
    const options = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    };

    return this.http.post<Product>(this.saveProductsUrl, newResource, options)
      .pipe( catchError(this.handleError) );
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
      errorMessage = `Backend returned code ${err.status}: ${err.message}`;
    }
    console.error(errorMessage);
    return throwError(errorMessage);
  }
}
