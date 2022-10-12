import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IOrder } from 'src/app/shared/models/order';
import { BreadcrumbService } from 'xng-breadcrumb';
import { OrdersService } from '../orders.service';

@Component({
  selector: 'app-order-details',
  templateUrl: './order-details.component.html',
  styleUrls: ['./order-details.component.scss']
})
export class OrderDetailsComponent implements OnInit {
  order: IOrder;

  constructor(
    private activatedRoute: ActivatedRoute,
    private breadcrumbService: BreadcrumbService,
    private ordersService: OrdersService) {
    this.breadcrumbService.set('@OrderDetails', '');
  }

  ngOnInit(): void {
    this.ordersService.getOrder(+this.activatedRoute.snapshot.paramMap.get('id'))
      .subscribe((order: IOrder) => {
        this.order = order;
        this.breadcrumbService.set('@OrderDetails', `Order ${order.id} - ${order.status}`);
      }, error => {
        console.log(error);
      })
  }

}
