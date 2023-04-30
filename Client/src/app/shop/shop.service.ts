import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Brand } from '../shared/models/brand';
import { Pagination } from '../shared/models/pagination';
import { Product } from '../shared/models/product';
import { ProductType } from '../shared/models/productType';
import { ShopParams } from '../shared/models/shopParams';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  baseUrl = environment.apiUrl;
  brands: Brand[] = [];
  products: Product[] = [];
  productTypes: ProductType[] = [];
  pagination?: Pagination<Product[]>;
  shopParams: ShopParams = new ShopParams();
  productCache: Map<string, Pagination<Product[]>> = new Map<string, Pagination<Product[]>>();

  constructor(private http: HttpClient) { }

  getBrands() {
    if (this.brands.length > 0) {
      return of(this.brands);
    }

    return this.http.get<Brand[]>(this.baseUrl + 'products/brands').pipe(
      map(response => {
        this.brands = response;
        return response;
      })
    );
  }

  getProduct(id: number) {
    const product = [...this.productCache.values()]
      .reduce((acc, paginatedResult) => {
        return { ...acc, ...paginatedResult.data.find(x => x.id === id) }
      }, {} as Product)

    if (Object.keys(product).length !== 0)
      return of(product);

    return this.http.get<Product>(this.baseUrl + 'products/' + id);
  }

  getProducts(useCache: boolean) {
    if (useCache === false) {
      this.productCache = new Map();
    }

    if (useCache === true && this.productCache.size > 0) {
      if (this.productCache.has(Object.values(this.shopParams).join('-'))) {
        this.pagination = this.productCache.get(Object.values(this.shopParams).join('-'));

        if (this.pagination != null)
          return of(this.pagination);
      }
    }

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

    return this.http.get<Pagination<Product[]>>(this.baseUrl + 'products', { params })
      .pipe(
        map(response => {
          this.productCache.set(Object.values(this.shopParams).join('-'), response);
          this.pagination = response;
          return this.pagination;
        })
      );
  }

  getProductTypes() {
    if (this.productTypes.length > 0) {
      return of(this.productTypes);
    }

    return this.http.get<ProductType[]>(this.baseUrl + 'products/types').pipe(
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