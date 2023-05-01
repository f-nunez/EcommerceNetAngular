import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { DeliveryMethod } from '../shared/models/deliveryMethod';
import { Order, OrderToCreate } from '../shared/models/order';

@Injectable({
  providedIn: 'root'
})
export class CheckoutService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  createOrder(order: OrderToCreate) {
    return this.http.post<Order>(this.baseUrl + 'order/createorder', order);
  }

  getDeliveryMethods() {
    return this.http.get<DeliveryMethod[]>(this.baseUrl + 'order/getdeliverymethods').pipe(
      map((deliveryMethods) => {
        return deliveryMethods.sort((a, b) => {
          return b.price - a.price;
        })
      })
    );
  }
}
