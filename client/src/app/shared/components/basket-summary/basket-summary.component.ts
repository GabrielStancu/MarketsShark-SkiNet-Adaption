import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { Observable } from 'rxjs';
import { BasketService } from 'src/app/basket/basket.service';
import { IBasket, IBasketItem } from '../../models/basket';
import { IOrderItem } from '../../models/order';
import { CommonModule} from '@angular/common';

@Component({
  selector: 'app-basket-summary',
  templateUrl: './basket-summary.component.html',
  styleUrls: ['./basket-summary.component.scss']
})
export class BasketSummaryComponent implements OnInit {
  @Output() decrement: EventEmitter<IBasketItem> = new EventEmitter<IBasketItem>();
  @Output() increment: EventEmitter<IBasketItem> = new EventEmitter<IBasketItem>();
  @Output() remove: EventEmitter<IBasketItem> = new EventEmitter<IBasketItem>();
  @Input() isBasket = true;
  @Input() items: IBasketItem[] | IOrderItem[] = [];
  @Input() isOrder = false;

  constructor() { }

  ngOnInit(): void {
  }

  decrementItemQuantity(item: IBasketItem): void{
    this.decrement.emit(item);
  }

  incrementItemQuantity(item: IBasketItem): void{
    this.increment.emit(item);
  }

  removeBasketItem(item: IBasketItem): void{
    this.remove.emit(item);
  }

}
