import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Basket, BasketItem, BasketTotals } from '../shared/models/basket';
import { DeliveryMethod } from '../shared/models/deliveryMethod';
import { Product } from '../shared/models/product';

@Injectable({
  providedIn: 'root'
})
export class BasketService {
  baseUrl = environment.apiUrl;
  private basketSource = new BehaviorSubject<Basket | null>(null);
  basketSource$ = this.basketSource.asObservable();
  private basketTotalSource = new BehaviorSubject<BasketTotals | null>(null);
  basketTotalSource$ = this.basketTotalSource.asObservable();
  shipping: number = 0;

  constructor(private http: HttpClient) { }

  createPaymentIntent() {
    return this.http.post<Basket>(this.baseUrl + 'payment/' + this.getCurrentBasketValue()?.id, {})
      .pipe(
        map((basket) => {
          this.basketSource.next(basket);
        })
      );
  }

  getCurrentBasketValue() {
    return this.basketSource.value;
  }

  getBasket(id: string) {
    return this.http.get<Basket>(this.baseUrl + 'basket?id=' + id)
      .pipe(
        map((basket) => {
          this.basketSource.next(basket);
          this.calculateTotals();
        })
      );
  }

  setBasket(basket: Basket) {
    return this.http.post<Basket>(this.baseUrl + 'basket', basket).subscribe({
      next: response => {
        this.basketSource.next(response);
        this.calculateTotals();
      },
      error: error => console.log(error)
    });
  }

  setShippingPrice(deliveryMethod: DeliveryMethod) {
    let basket = this.getCurrentBasketValue();
    if (basket) {
      basket.deliveryMethodId = deliveryMethod.id;
      basket.shippingPrice = deliveryMethod.price;
      this.setBasket(basket);
    }
  }

  deleteBasket(basket: Basket) {
    return this.http.delete(this.baseUrl + 'basket?id=' + basket.id).subscribe({
      next: () => {
        this.deleteLocalBasket();
      }
    })
  }

  deleteLocalBasket() {
    this.basketSource.next(null);
    this.basketTotalSource.next(null);
    localStorage.removeItem('basket_id');
  }

  deleteBasketOrdered(id: string) {
    this.basketSource.next(null);
    this.basketTotalSource.next(null);
    localStorage.removeItem('basket_id');
  }

  addItemToBasket(item: Product | BasketItem, quantity = 1) {
    if (this.isProduct(item))
      item = this.mapProductItemToBasketItem(item);

    const basket = this.getCurrentBasketValue() ?? this.createBasket();
    basket.items = this.addOrUpdateItem(basket.items, item, quantity);
    this.setBasket(basket);
  }

  removeItemFromBasket(id: number, quantity = 1) {
    const basket = this.getCurrentBasketValue();

    if (!basket)
      return;

    const item = basket.items.find(x => x.id === id);

    if (item) {
      item.quantity -= quantity;

      if (item.quantity === 0)
        basket.items = basket.items.filter(x => x.id !== id);

      if (basket.items.length > 0)
        this.setBasket(basket);
      else
        this.deleteBasket(basket);
    }
  }

  private addOrUpdateItem(items: BasketItem[], itemToAdd: BasketItem, quantity: number): BasketItem[] {
    const item = items.find(x => x.id === itemToAdd.id);

    if (item) {
      item.quantity += quantity;
    } else {
      itemToAdd.quantity = quantity;
      items.push(itemToAdd);
    }
    return items;
  }

  private calculateTotals() {
    const basket = this.getCurrentBasketValue();
    if (!basket) return;
    const subtotal = basket.items.reduce((a, b) => (b.price * b.quantity) + a, 0);
    const total = subtotal + basket.shippingPrice;
    this.basketTotalSource.next({ shipping: basket.shippingPrice, total, subtotal });
  }

  private createBasket(): Basket {
    const basket = new Basket();
    localStorage.setItem('basket_id', basket.id);
    return basket;
  }

  private mapProductItemToBasketItem(item: Product): BasketItem {
    return {
      id: item.id,
      productName: item.name,
      price: item.price,
      quantity: 0,
      pictureUrl: item.pictureUrl,
      brand: item.productBrand,
      type: item.productType
    }
  }

  private isProduct(item: Product | BasketItem): item is Product {
    return (item as Product).productBrand !== undefined;
  }
}