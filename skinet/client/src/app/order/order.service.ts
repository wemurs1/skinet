import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Order } from '../shared/models/order';

@Injectable({
  providedIn: 'root',
})
export class OrderService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getUserOrders() {
    return this.http.get<Order[]>(this.baseUrl + 'orders');
  }

  getOrderDetail(id: number) {
    return this.http.get<Order>(this.baseUrl + 'orders/' + id);
  }
}
