import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
  { path: 'dashboard', title: 'Dashboard | Altairis', loadComponent: () => import('./features/dashboard/dashboard.component').then(m => m.DashboardComponent) },
  { path: 'hotels', title: 'Hotels | Altairis', loadComponent: () => import('./features/hotels/hotel-list/hotel-list.component').then(m => m.HotelListComponent) },
  { path: 'hotels/create', title: 'New Hotel | Altairis', loadComponent: () => import('./features/hotels/hotel-create/hotel-create.component').then(m => m.HotelCreateComponent) },
  { path: 'room-categories', title: 'Room Categories | Altairis', loadComponent: () => import('./features/room-categories/room-category-list.component').then(m => m.RoomCategoryListComponent) },
  { path: 'room-types', title: 'Room Types | Altairis', loadComponent: () => import('./features/room-types/room-type-list/room-type-list.component').then(m => m.RoomTypeListComponent) },
  { path: 'room-types/create', title: 'New Room Type | Altairis', loadComponent: () => import('./features/room-types/room-type-create/room-type-create.component').then(m => m.RoomTypeCreateComponent) },
  { path: 'inventory', title: 'Inventory | Altairis', loadComponent: () => import('./features/inventory/inventory-list.component').then(m => m.InventoryListComponent) },
  { path: 'reservations', title: 'Reservations | Altairis', loadComponent: () => import('./features/reservations/reservation-list/reservation-list.component').then(m => m.ReservationListComponent) },
  { path: 'reservations/create', title: 'New Reservation | Altairis', loadComponent: () => import('./features/reservations/reservation-create/reservation-create.component').then(m => m.ReservationCreateComponent) },
];
