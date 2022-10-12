import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class OrdersService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getOrder(id: number) {
    return this.http.get(this.baseUrl + 'order/getorder/' + id);
  }

  getOrders() {
    return this.http.get(this.baseUrl + 'order/getorders');
  }

}
