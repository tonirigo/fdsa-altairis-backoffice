import { Injectable, inject, signal } from '@angular/core';
import { Api } from '../../api/generated/api';
import { HotelInventoryGridDto } from '../../api/generated/models/hotel-inventory-grid-dto';
import { apiHotelsHotelIdAvailabilityGet } from '../../api/generated/fn/availability/api-hotels-hotel-id-availability-get';

@Injectable({ providedIn: 'root' })
export class AvailabilityStore {
  private api = inject(Api);

  grid = signal<HotelInventoryGridDto | null>(null);
  loading = signal(false);
  error = signal<string | null>(null);

  reset() {
    this.grid.set(null);
    this.loading.set(false);
  }

  getHotelGrid(hotelId: number, from: string, to: string) {
    this.loading.set(true);
    this.error.set(null);
    this.api.invoke(apiHotelsHotelIdAvailabilityGet, { hotelId, from, to }).then((data: HotelInventoryGridDto) => {
      this.grid.set(data);
      this.loading.set(false);
    }).catch((err) => {
      this.error.set(err?.message ?? 'An unexpected error occurred');
      this.loading.set(false);
    });
  }
}
