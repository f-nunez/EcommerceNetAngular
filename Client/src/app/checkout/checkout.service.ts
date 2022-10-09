import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IDeliveryMethod } from '../shared/models/deliveryMethod';

@Injectable({
  providedIn: 'root'
})
export class CheckoutService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getDeliveryMethods() {
    return this.http.get(this.baseUrl + 'order/getdeliverymethods').pipe(
      map((deliveryMethods: IDeliveryMethod[]) => {
        return deliveryMethods.sort((a, b) => {
          return b.price - a.price;
        })
      })
    );
  }
}
