import { Injectable, inject, signal } from '@angular/core';
import { Api } from '../../api/generated/api';
import { RoomCategoryDto } from '../../api/generated/models/room-category-dto';
import { CreateRoomCategoryDto } from '../../api/generated/models/create-room-category-dto';
import { apiRoomCategoriesGet } from '../../api/generated/fn/room-categories/api-room-categories-get';
import { apiRoomCategoriesPost } from '../../api/generated/fn/room-categories/api-room-categories-post';
import { from, catchError, throwError } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class RoomCategoryStore {
  private api = inject(Api);

  categories = signal<RoomCategoryDto[]>([]);
  loading = signal(false);
  error = signal<string | null>(null);

  getAll() {
    this.loading.set(true);
    this.error.set(null);
    this.api.invoke(apiRoomCategoriesGet).then((data: Array<RoomCategoryDto>) => {
      this.categories.set(data);
      this.loading.set(false);
    }).catch((err) => {
      this.error.set(err?.message ?? 'An unexpected error occurred');
      this.loading.set(false);
    });
  }

  create(category: CreateRoomCategoryDto) {
    this.error.set(null);
    return from(this.api.invoke(apiRoomCategoriesPost, { body: category })).pipe(
      catchError((err) => {
        this.error.set(err?.message ?? 'An unexpected error occurred');
        return throwError(() => err);
      })
    );
  }
}
