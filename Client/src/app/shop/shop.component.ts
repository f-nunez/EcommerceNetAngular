import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Brand } from '../shared/models/brand';
import { Product } from '../shared/models/product';
import { ProductType } from '../shared/models/productType';
import { ShopParams } from '../shared/models/shopParams';
import { ShopService } from './shop.service';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})
export class ShopComponent implements OnInit {
  @ViewChild('search') searchTerm?: ElementRef;
  brands: Brand[] = [];
  products: Product[] = [];
  productTypes: ProductType[] = [];
  shopParams: ShopParams;
  sortOptions = [
    { name: 'Alphabetical', value: "name" },
    { name: 'Price: Low to High', value: 'priceAsc' },
    { name: 'Price: High to Low', value: 'priceDesc' }
  ];
  totalCount: number = 0;

  constructor(private shopService: ShopService) {
    this.shopParams = this.shopService.getShopParams();
  }

  ngOnInit(): void {
    this.getProducts(true);
    this.getBrands();
    this.getProductTypes();
  }

  getBrands() {
    this.shopService.getBrands().subscribe({
      next: (response) => this.brands = [{ id: 0, name: 'All' }, ...response],
      error: (error) => console.log(error)
    });
  }

  getProducts(useCache: boolean = false) {
    this.shopService.getProducts(useCache).subscribe({
      next: (response) => {
        this.products = response.data;
        this.totalCount = response.count;
      },
      error: (error) => console.log(error)
    });
  }

  getProductTypes() {
    this.shopService.getProductTypes().subscribe({
      next: (response) => this.productTypes = [{ id: 0, name: 'All' }, ...response],
      error: (error) => console.log(error)
    });
  }

  onBrandSelected(brandId: number) {
    const shopParams = this.shopService.getShopParams();
    shopParams.brandId = brandId;
    shopParams.pageNumber = 1;
    this.shopService.setShopParams(shopParams);
    this.shopParams = shopParams;
    this.getProducts();
  }

  onPageChanged(event: any) {
    const shopParams = this.shopService.getShopParams();
    if (shopParams.pageNumber === event)
      return;

    shopParams.pageNumber = event;
    this.shopService.setShopParams(shopParams);
    this.shopParams = shopParams;
    this.getProducts(true);
  }

  onProductTypeSelected(productTypeId: number) {
    const shopParams = this.shopService.getShopParams();
    shopParams.productTypeId = productTypeId;
    shopParams.pageNumber = 1;
    this.shopService.setShopParams(shopParams);
    this.shopParams = shopParams;
    this.getProducts();
  }

  onReset() {
    if (this.searchTerm)
      this.searchTerm.nativeElement.value = '';

    this.shopParams = new ShopParams();
    this.shopService.setShopParams(this.shopParams);
    this.getProducts();
  }

  onSearch() {
    const shopParams = this.shopService.getShopParams();
    shopParams.search = this.searchTerm?.nativeElement.value;
    shopParams.pageNumber = 1;
    this.shopService.setShopParams(shopParams);
    this.shopParams = shopParams;
    this.getProducts();
  }

  onSortSelected(sort: string) {
    const shopParams = this.shopService.getShopParams();
    shopParams.sort = sort;
    this.shopService.setShopParams(shopParams);
    this.shopParams = shopParams;
    this.getProducts();
  }
}
