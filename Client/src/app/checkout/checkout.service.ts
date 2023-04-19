import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IDeliveryMethod } from '../shared/models/deliveryMethod';
import { IOrderToCreate } from '../shared/models/order';

@Injectable({
  providedIn: 'root'
})
export class CheckoutService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  createOrder(order: IOrderToCreate) {
    return this.http.post(this.baseUrl + 'order/createorder', order);
  }

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
