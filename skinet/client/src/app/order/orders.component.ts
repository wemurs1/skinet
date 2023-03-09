import { Component, OnInit } from '@angular/core';
import { Order } from '../shared/models/order';
import { OrderService } from './order.service';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.scss'],
})
export class OrdersComponent implements OnInit {
  orders: Order[] = [];

  constructor(private ordersService: OrderService) {}

  ngOnInit(): void {
    this.getOrders();
  }

  getOrders() {
    this.ordersService
      .getUserOrders()
      .subscribe({ next: (orders) => (this.orders = orders) });
  }
}
