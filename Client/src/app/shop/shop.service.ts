import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IBrand } from '../shared/models/brand';
import { IPagination, Pagination } from '../shared/models/pagination';
import { IProduct } from '../shared/models/product';
import { IProductType } from '../shared/models/productType';
import { ShopParams } from '../shared/models/shopParams';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  baseUrl = environment.apiUrl;
  brands: IBrand[] = [];
  products: IProduct[] = [];
  productTypes: IProductType[] = [];
  pagination: Pagination = new Pagination();
  shopParams: ShopParams = new ShopParams();

  constructor(private http: HttpClient) { }

  getBrands() {
    if (this.brands.length > 0) {
      return of(this.brands);
    }

    return this.http.get<IBrand[]>(this.baseUrl + 'products/brands').pipe(
      map(response => {
        this.brands = response;
        return response;
      })
    );
  }

  getProduct(id: number) {
    const product = this.products.find(p => p.id === id);

    if (product) {
      return of(product);
    }

    return this.http.get<IProduct>(this.baseUrl + 'products/' + id);
  }

  getProducts() {
    let params = new HttpParams();

    if (this.shopParams.brandId !== 0) {
      params = params.append('brandId', this.shopParams.brandId.toString());
    }

    if (this.shopParams.productTypeId !== 0) {
      params = params.append('typeId', this.shopParams.productTypeId.toString());
    }

    if (this.shopParams.search) {
      params = params.append('search', this.shopParams.search);
    }

    params = params.append('sort', this.shopParams.sort);
    params = params.append('pageIndex', this.shopParams.pageNumber.toString());
    params = params.append('pageSize', this.shopParams.pageSize.toString());

    return this.http.get<IPagination>(this.baseUrl + 'products', { observe: 'response', params })
      .pipe(
        map(response => {
          this.products = [...this.products, ...response.body.data];
          return response.body;
        })
      );
  }

  getProductTypes() {
    if (this.productTypes.length > 0) {
      return of(this.productTypes);
    }

    return this.http.get<IProductType[]>(this.baseUrl + 'products/types').pipe(
      map(response => {
        this.productTypes = response;
        return response;
      })
    );
  }

  getShopParams() {
    return this.shopParams;
  }

  setShopParams(shopParams: ShopParams) {
    this.shopParams = shopParams;
  }
}