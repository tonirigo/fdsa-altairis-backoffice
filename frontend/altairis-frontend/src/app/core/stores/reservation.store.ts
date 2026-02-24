import { Injectable, inject, signal } from '@angular/core';
import { Api } from '../../api/generated/api';
import { ReservationDto } from '../../api/generated/models/reservation-dto';
import { CreateReservationDto } from '../../api/generated/models/create-reservation-dto';
import { ReservationDtoPagedResult } from '../../api/generated/models/reservation-dto-paged-result';
import { UpdateReservationStatusDto } from '../../api/generated/models/update-reservation-status-dto';
import { apiReservationsGet } from '../../api/generated/fn/reservations/api-reservations-get';
import { apiReservationsPost } from '../../api/generated/fn/reservations/api-reservations-post';
import { apiReservationsIdStatusPatch } from '../../api/generated/fn/reservations/api-reservations-id-status-patch';
import { from, catchError, throwError } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ReservationStore {
  private api = inject(Api);

  reservations = signal<ReservationDto[]>([]);
  totalCount = signal(0);
  loading = signal(false);
  error = signal<string | null>(null);

  reset() {
    this.reservations.set([]);
    this.totalCount.set(0);
    this.loading.set(false);
  }

  loadFiltered(
    hotelId?: number,
    fromDate?: string,
    toDate?: string,
    status?: 'Pending' | 'Confirmed' | 'Cancelled',
    page = 1,
    pageSize = 20
  ) {
    this.loading.set(true);
    this.error.set(null);
    this.api.invoke(apiReservationsGet, {
      hotelId: hotelId ?? undefined,
      from: fromDate ?? undefined,
      to: toDate ?? undefined,
      status: status ?? undefined,
      page,
      pageSize,
    }).then((result: ReservationDtoPagedResult) => {
      this.reservations.set(result.items ?? []);
      this.totalCount.set(result.totalCount ?? 0);
      this.loading.set(false);
    }).catch((err) => {
      this.error.set(err?.message ?? 'An unexpected error occurred');
      this.loading.set(false);
    });
  }

  updateStatus(id: number, status: 'Pending' | 'Confirmed' | 'Cancelled') {
    this.error.set(null);
    const body: UpdateReservationStatusDto = { status };
    return from(this.api.invoke(apiReservationsIdStatusPatch, { id, body })).pipe(
      catchError((err) => {
        this.error.set(err?.message ?? 'An unexpected error occurred');
        return throwError(() => err);
      })
    );
  }

  create(reservation: CreateReservationDto) {
    this.error.set(null);
    return from(this.api.invoke(apiReservationsPost, { body: reservation })).pipe(
      catchError((err) => {
        this.error.set(err?.message ?? 'An unexpected error occurred');
        return throwError(() => err);
      })
    );
  }
}
