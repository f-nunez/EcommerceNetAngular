import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Order } from '../shared/models/order';

@Injectable({
  providedIn: 'root'
})
export class OrdersService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getOrder(id: number) {
    return this.http.get<Order>(this.baseUrl + 'order/getorder/' + id);
  }

  getOrders() {
    return this.http.get<Order[]>(this.baseUrl + 'order/getorders');
  }

}
