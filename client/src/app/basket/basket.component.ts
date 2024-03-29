import { Component } from '@angular/core';
import { BasketService } from './basket.service';
import { BasketItem } from '../shared/models/basket';

@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styleUrl: './basket.component.scss'
})
export class BasketComponent {
  constructor(public basketService: BasketService) { }

  incrementQuantity(item: BasketItem) {
    this.basketService.addItemToBasket(item);
  }

  decrementQuantity(event: { id: number, quantity: number }) {
    this.basketService.removeItemFromBasket(event.id, event.quantity);
  }
}
