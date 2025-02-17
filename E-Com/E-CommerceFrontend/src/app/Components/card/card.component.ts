import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

@Component({
  selector: 'app-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './card.component.html',
  styleUrl: './card.component.css'
})
export class CardComponent {

  products = [
    {
        name: 'Wireless Headphones',
        brand: 'Sony',
        price: 199.99,
        imageUrl: null,
    },
    {
        name: 'Smartphone',
        brand: 'Samsung',
        price: 999.99,
        imageUrl: null, // No image provided
    },
    {
        name: 'Fitness Tracker',
        brand: 'Fitbit',
        price: 149.99,
        imageUrl: null,
    }
];

currencySymbol = '$';

addToCart(product:any) {
    console.log(`Added ${product.name} to the cart!`);
}

}
