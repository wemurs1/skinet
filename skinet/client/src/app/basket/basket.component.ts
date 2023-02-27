import { Component } from '@angular/core';
import { BasketItem } from '../shared/models/basket';
import { BasketService } from './basket.service';

@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styleUrls: ['./basket.component.scss'],
})
export class BasketComponent {
  constructor(public basketService: BasketService) {}

  incrementQuantity(item: BasketItem) {
    this.basketService.addItemToBasket(item);
  }

  removeItem(id: number, quantity = 1) {
    this.basketService.removeitemFromBasket(id, quantity);
  }
}
