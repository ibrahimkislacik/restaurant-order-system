import {Injectable} from '@angular/core';
import {CartItemModel} from "../models/cart-item.model";

@Injectable({
    providedIn: 'root'
})
export class CartService {

    constructor() {
    }

    addToCart(cartItemModel: CartItemModel): void {
        let cartItems = JSON.parse(localStorage.getItem('cart')) || [];
        cartItems.push(cartItemModel);
        localStorage.setItem('cart', JSON.stringify(cartItems));
    }

    removeFromCart(cartItemModel: CartItemModel): void {
        let cartItems = JSON.parse(localStorage.getItem('cart')) || [];
        cartItems = cartItems.filter(item => item.cartId !== cartItemModel.cartId);
        localStorage.setItem('cart', JSON.stringify(cartItems));
    }

    getCartItems(): CartItemModel[] {
        return JSON.parse(localStorage.getItem('cart')) || [];
    }

    clearCart(): void {
        localStorage.removeItem('cart');
    }
}
