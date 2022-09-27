import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { IBrand } from '../shared/models/brand';
import { IProduct } from '../shared/models/product';
import { IProductType } from '../shared/models/productType';
import { ShopParams } from '../shared/models/shopParams';
import { ShopService } from './shop.service';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})
export class ShopComponent implements OnInit {
  @ViewChild('search', { static: false }) searchTerm: ElementRef;
  brands: IBrand[];
  products: IProduct[];
  productTypes: IProductType[];
  shopParams: ShopParams = new ShopParams();
  sortOptions = [
    { name: 'Alphabetical', value: "name" },
    { name: 'Price: Low to High', value: 'priceAsc' },
    { name: 'Price: High to Low', value: 'priceDesc' }
  ];
  totalCount: number = 0;

  constructor(private shopService: ShopService) { }

  ngOnInit(): void {
    this.getBrands();
    this.getProductTypes();
    this.getProducts();
  }

  getBrands() {
    this.shopService.getBrands().subscribe({
      next: (response) => this.brands = [{ id: 0, name: 'All' }, ...response],
      error: (error) => console.log(error)
    });
  }

  getProducts() {
    this.shopService.getProducts(this.shopParams).subscribe({
      next: (response) => {
        this.products = response.data;
        this.shopParams.pageNumber = response.pageIndex;
        this.shopParams.pageSize = response.pageSize;
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
    this.shopParams.brandId = brandId;
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }

  onPageChanged(event: any) {
    if (this.shopParams.pageNumber === event)
      return;

    this.shopParams.pageNumber = event;
    this.getProducts();
  }

  onProductTypeSelected(productTypeId: number) {
    this.shopParams.productTypeId = productTypeId;
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.shopParams = new ShopParams();
    this.getProducts();
  }

  onSearch() {
    this.shopParams.search = this.searchTerm.nativeElement.value;
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }

  onSortSelected(sort: string) {
    this.shopParams.sort = sort;
    this.getProducts();
  }
}
