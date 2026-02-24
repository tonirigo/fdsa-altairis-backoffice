import { Injectable, inject, signal } from '@angular/core';
import { Api } from '../../api/generated/api';
import { RoomTypeDto } from '../../api/generated/models/room-type-dto';
import { CreateRoomTypeDto } from '../../api/generated/models/create-room-type-dto';
import { apiHotelsHotelIdRoomTypesGet } from '../../api/generated/fn/room-types/api-hotels-hotel-id-room-types-get';
import { apiHotelsHotelIdRoomTypesPost } from '../../api/generated/fn/room-types/api-hotels-hotel-id-room-types-post';
import { from, catchError, throwError } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class RoomTypeStore {
  private api = inject(Api);

  roomTypes = signal<RoomTypeDto[]>([]);
  loading = signal(false);
  error = signal<string | null>(null);

  reset() {
    this.roomTypes.set([]);
    this.loading.set(false);
  }

  getByHotel(hotelId: number) {
    this.loading.set(true);
    this.error.set(null);
    this.api.invoke(apiHotelsHotelIdRoomTypesGet, { hotelId }).then((data: Array<RoomTypeDto>) => {
      this.roomTypes.set(data);
      this.loading.set(false);
    }).catch((err) => {
      this.error.set(err?.message ?? 'An unexpected error occurred');
      this.loading.set(false);
    });
  }

  create(hotelId: number, roomType: CreateRoomTypeDto) {
    this.error.set(null);
    return from(this.api.invoke(apiHotelsHotelIdRoomTypesPost, { hotelId, body: roomType })).pipe(
      catchError((err) => {
        this.error.set(err?.message ?? 'An unexpected error occurred');
        return throwError(() => err);
      })
    );
  }
}
