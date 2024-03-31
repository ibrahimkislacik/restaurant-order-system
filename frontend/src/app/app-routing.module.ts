import {NgModule} from '@angular/core';
import {Routes, RouterModule} from '@angular/router';
import {AdminComponent} from './theme/layout/admin/admin.component';
import {GuestComponent} from './theme/layout/guest/guest.component';

const routes: Routes = [
    {
        path: '',
        component: AdminComponent,
        children: [
            {
                path: '',
                redirectTo: 'dashboard',
                pathMatch: 'full',
            },
            {
                path: 'dashboard',
                loadComponent: () => import('./demo/dashboard/dashboard.component'),
            },
            {
                path: 'categories',
                loadChildren: () =>
                    import('./pages/category/category.module').then(
                        (m) => m.CategoryModule,
                    ),
            },
            {
                path: 'products',
                loadChildren: () =>
                    import('./pages/product/product.module').then(
                        (m) => m.ProductModule,
                    ),
            },
            {
                path: 'orders',
                loadChildren: () =>
                    import('./pages/order/order.module').then(
                        (m) => m.OrderModule,
                    ),
            },
        ],
    },
    {
        path: '',
        component: GuestComponent,
        children: [
            {
                path: 'auth',
                loadChildren: () =>
                    import('./demo/pages/authentication/authentication.module').then(
                        (m) => m.AuthenticationModule,
                    ),
            },
        ],
    },
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule],
})
export class AppRoutingModule {
}
