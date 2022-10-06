import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { PagingHeaderComponent } from './components/paging-header/paging-header.component';
import { PagingFooterComponent } from './components/paging-footer/paging-footer.component';
import { CarouselModule } from 'ngx-bootstrap/carousel';
import { OrderTotalsComponent } from './components/order-totals/order-totals.component';
import { BsDropdownConfig, BsDropdownModule } from 'ngx-bootstrap/dropdown'
import { ReactiveFormsModule } from '@angular/forms';
import { TextInputComponent } from './components/text-input/text-input.component';

@NgModule({
  declarations: [
    PagingHeaderComponent,
    PagingFooterComponent,
    OrderTotalsComponent,
    TextInputComponent
  ],
  imports: [
    CommonModule,
    PaginationModule.forRoot(),
    CarouselModule.forRoot(),
    BsDropdownModule.forRoot(),
    ReactiveFormsModule
  ],
  exports: [
    PaginationModule,
    PagingHeaderComponent,
    PagingFooterComponent,
    CarouselModule,
    OrderTotalsComponent,
    BsDropdownModule,
    ReactiveFormsModule,
    TextInputComponent
  ]
})
export class SharedModule { }
