import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { Product } from '../models/product';

@Injectable({
  providedIn: 'root'
})

export class ProductService {

  private productApi = 'https://localhost:44322/api/v1/Seller';  // URL to web api

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient) { }

  /** GET products from the server */
  getProducts(): Observable<Product[]> {
    
    return this.http.get<Product[]>(this.productApi)
      .pipe(
        tap(_ => console.log('fetched products')),
        catchError(this.handleError<Product[]>('getProducts', []))
      );
  
    }
    
    private handleError<T>(operation = 'operation', result?: T) {
      return (error: any): Observable<T> => {
  
        // TODO: send the error to remote logging infrastructure
        console.error(error); // log to console instead
  
        console.log(`${operation} failed: ${error.message}`);
  
        // Let the app keep running by returning an empty result.
        return of(result as T);
      };
    }
  
}
