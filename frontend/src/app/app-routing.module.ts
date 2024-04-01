import {NgModule} from '@angular/core';
import {Routes, RouterModule} from '@angular/router';
import {AdminComponent} from './theme/layout/admin/admin.component';
import {GuestComponent} from './theme/layout/guest/guest.component';
import {AuthenticationGuard} from "./guards/authentication.guard";

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
                loadComponent: () => import('./pages/dashboard/dashboard.component'),
            },
            {
                path: 'customer',
                loadChildren: () =>
                    import('./pages/customer/customer.module').then(
                        (m) => m.CustomerModule,
                    ),
                canActivate: [AuthenticationGuard],
                data: {permissions: ['User']},
            },
            {
                path: 'categories',
                loadChildren: () =>
                    import('./pages/category/category.module').then(
                        (m) => m.CategoryModule,
                    ),
                canActivate: [AuthenticationGuard],
                data: {permissions: ['Admin']},
            },
            {
                path: 'products',
                loadChildren: () =>
                    import('./pages/product/product.module').then(
                        (m) => m.ProductModule,
                    ),
                canActivate: [AuthenticationGuard],
                data: {permissions: ['Admin']},
            },
            {
                path: 'orders',
                loadChildren: () =>
                    import('./pages/order/order.module').then(
                        (m) => m.OrderModule,
                    ),
                canActivate: [AuthenticationGuard],
                data: {permissions: ['Admin']},
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
                    import('./pages/authentication/authentication.module').then(
                        (m) => m.AuthenticationModule,
                    ),
            }
        ],
    },
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule],
})
export class AppRoutingModule {
}
