import { Injectable, inject, signal } from '@angular/core';
import { Api } from '../../api/generated/api';
import { HotelDto } from '../../api/generated/models/hotel-dto';
import { CreateHotelDto } from '../../api/generated/models/create-hotel-dto';
import { HotelDtoPagedResult } from '../../api/generated/models/hotel-dto-paged-result';
import { apiHotelsGet } from '../../api/generated/fn/hotels/api-hotels-get';
import { apiHotelsSearchGet } from '../../api/generated/fn/hotels/api-hotels-search-get';
import { apiHotelsIdGet } from '../../api/generated/fn/hotels/api-hotels-id-get';
import { apiHotelsPost } from '../../api/generated/fn/hotels/api-hotels-post';
import { from, catchError, throwError } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class HotelStore {
  private api = inject(Api);

  hotels = signal<HotelDto[]>([]);
  totalCount = signal(0);
  loading = signal(false);
  error = signal<string | null>(null);

  getAll(page = 1, pageSize = 10) {
    this.loading.set(true);
    this.error.set(null);
    this.api.invoke(apiHotelsGet, { page, pageSize }).then((result: HotelDtoPagedResult) => {
      this.hotels.set(result.items ?? []);
      this.totalCount.set(result.totalCount ?? 0);
      this.loading.set(false);
    }).catch((err) => {
      this.error.set(err?.message ?? 'An unexpected error occurred');
      this.loading.set(false);
    });
  }

  loadAll() {
    this.loading.set(true);
    this.error.set(null);
    this.api.invoke(apiHotelsGet, { page: 1, pageSize: 1000 }).then((result: HotelDtoPagedResult) => {
      this.hotels.set(result.items ?? []);
      this.totalCount.set(result.totalCount ?? 0);
      this.loading.set(false);
    }).catch((err) => {
      this.error.set(err?.message ?? 'An unexpected error occurred');
      this.loading.set(false);
    });
  }

  getById(id: number) {
    this.error.set(null);
    return from(this.api.invoke(apiHotelsIdGet, { id })).pipe(
      catchError((err) => {
        this.error.set(err?.message ?? 'An unexpected error occurred');
        return throwError(() => err);
      })
    );
  }

  search(query: string, page = 1, pageSize = 10) {
    this.loading.set(true);
    this.error.set(null);
    this.api.invoke(apiHotelsSearchGet, { query, page, pageSize }).then((result: HotelDtoPagedResult) => {
      this.hotels.set(result.items ?? []);
      this.totalCount.set(result.totalCount ?? 0);
      this.loading.set(false);
    }).catch((err) => {
      this.error.set(err?.message ?? 'An unexpected error occurred');
      this.loading.set(false);
    });
  }

  create(hotel: CreateHotelDto) {
    this.error.set(null);
    return from(this.api.invoke(apiHotelsPost, { body: hotel })).pipe(
      catchError((err) => {
        this.error.set(err?.message ?? 'An unexpected error occurred');
        return throwError(() => err);
      })
    );
  }
}
