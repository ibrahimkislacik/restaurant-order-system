import {Injectable} from '@angular/core';

export interface NavigationItem {
    id: string;
    title: string;
    type: 'item' | 'collapse' | 'group';
    translate?: string;
    icon?: string;
    hidden?: boolean;
    url?: string;
    classes?: string;
    exactMatch?: boolean;
    external?: boolean;
    target?: boolean;
    breadcrumbs?: boolean;
    function?: any;
    children?: Navigation[];
}

export interface Navigation extends NavigationItem {
    children?: NavigationItem[];
}

const NavigationItems = [
    {
        id: 'customer',
        title: 'Customer',
        type: 'group',
        icon: 'icon-navigation',
        children: [
            {
                id: 'menu',
                title: 'Menu',
                type: 'item',
                url: '/customer/menu',
                icon: 'feather icon-home',
                classes: 'nav-item',
            },
            {
                id: 'myorders',
                title: 'My Orders',
                type: 'item',
                url: '/customer/orders',
                icon: 'feather icon-box',
                classes: 'nav-item',
            },
        ],
    },
    {
        id: 'management',
        title: 'Admin',
        type: 'group',
        icon: 'icon-group',
        children: [
            {
                id: 'category',
                title: 'Categories',
                type: 'item',
                url: '/categories',
                classes: 'nav-item',
                icon: 'feather icon-file-text',
            },
            {
                id: 'product',
                title: 'Products',
                type: 'item',
                url: '/products',
                classes: 'nav-item',
                icon: 'feather icon-server',
            },
            {
                id: 'order',
                title: 'Orders',
                type: 'item',
                url: '/orders',
                classes: 'nav-item',
                icon: 'feather icon-box',
            },
        ],
    }
];

@Injectable()
export class NavigationItem {
    get() {
        return NavigationItems;
    }
}
