import { Routes } from '@angular/router';
import { OrderComponent } from './pages/order/order.component';
import {DashboardComponent } from './pages/dashboard/dashboard.component';


export const routes: Routes = [
    {
      path: 'dashboard',
      component: DashboardComponent
    },
    {
        path: 'orders/table/:id',
        component: OrderComponent
    },
    {
        path: '',
        redirectTo: 'dashboard',
        pathMatch: 'full',
    }

];
