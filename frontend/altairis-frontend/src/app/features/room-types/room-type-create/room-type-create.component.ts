import { ChangeDetectionStrategy, Component, effect, inject, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar } from '@angular/material/snack-bar';
import { HotelStore } from '../../../core/stores/hotel.store';
import { RoomCategoryStore } from '../../../core/stores/room-category.store';
import { RoomTypeStore } from '../../../core/stores/room-type.store';
import { CreateRoomTypeDto } from '../../../api/generated/models/create-room-type-dto';

@Component({
  selector: 'app-room-type-create',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [FormsModule, MatInputModule, MatFormFieldModule, MatSelectModule, MatButtonModule, MatCardModule, MatIconModule],
  templateUrl: './room-type-create.component.html',
  styleUrl: './room-type-create.component.scss',
})
export class RoomTypeCreateComponent implements OnInit {
  hotelStore = inject(HotelStore);
  categoryStore = inject(RoomCategoryStore);
  private roomTypeStore = inject(RoomTypeStore);
  private route = inject(ActivatedRoute);
  private snackBar = inject(MatSnackBar);
  router = inject(Router);

  selectedHotelId: number | null = null;
  roomType: CreateRoomTypeDto = { name: '', capacity: 1, totalRooms: 1 };
  submitting = false;
  private pendingHotelId: number | null = null;
  private hotelLoadedEffect = effect(() => {
    const hotels = this.hotelStore.hotels();
    if (hotels.length > 0 && this.pendingHotelId) {
      this.selectedHotelId = this.pendingHotelId;
      this.pendingHotelId = null;
    }
  });

  ngOnInit() {
    const hotelId = this.route.snapshot.queryParamMap.get('hotelId');
    if (hotelId) {
      this.pendingHotelId = +hotelId;
    }
    this.hotelStore.loadAll();
    this.categoryStore.getAll();
  }

  onSubmit() {
    if (this.selectedHotelId) {
      this.submitting = true;
      this.roomTypeStore.create(this.selectedHotelId, this.roomType).subscribe({
        next: () => {
          this.submitting = false;
          this.snackBar.open('Room type created successfully', 'Close', { duration: 3000 });
          this.router.navigate(['/room-types'], { queryParams: { hotelId: this.selectedHotelId } });
        },
        error: () => {
          this.submitting = false;
          this.snackBar.open('Failed to create room type', 'Close', { duration: 5000 });
        }
      });
    }
  }
}
