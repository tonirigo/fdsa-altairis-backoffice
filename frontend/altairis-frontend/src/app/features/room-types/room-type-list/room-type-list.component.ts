import { ChangeDetectionStrategy, Component, effect, inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { FormsModule } from '@angular/forms';
import { HotelStore } from '../../../core/stores/hotel.store';
import { RoomTypeStore } from '../../../core/stores/room-type.store';

@Component({
  selector: 'app-room-type-list',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    MatTableModule, MatSelectModule, MatFormFieldModule, MatButtonModule,
    MatIconModule, MatProgressSpinnerModule, FormsModule,
  ],
  templateUrl: './room-type-list.component.html',
  styleUrl: './room-type-list.component.scss',
})
export class RoomTypeListComponent implements OnInit {
  hotelStore = inject(HotelStore);
  roomTypeStore = inject(RoomTypeStore);
  router = inject(Router);
  private route = inject(ActivatedRoute);

  selectedHotelId: number | null = null;
  displayedColumns = ['name', 'categoryName', 'totalRooms', 'capacity'];

  private pendingHotelId: number | null = null;
  private hotelLoadedEffect = effect(() => {
    const hotels = this.hotelStore.hotels();
    if (hotels.length > 0 && this.pendingHotelId) {
      this.selectedHotelId = this.pendingHotelId;
      this.pendingHotelId = null;
      this.roomTypeStore.getByHotel(this.selectedHotelId!);
    }
  });

  ngOnInit() {
    this.selectedHotelId = null;
    this.roomTypeStore.reset();

    const hotelIdParam = this.route.snapshot.queryParamMap.get('hotelId');
    if (hotelIdParam) {
      this.pendingHotelId = +hotelIdParam;
    }
    this.hotelStore.loadAll();
  }

  onHotelChange() {
    if (this.selectedHotelId) {
      this.roomTypeStore.getByHotel(this.selectedHotelId);
    }
  }

  getSelectedHotelName(): string | null {
    if (!this.selectedHotelId) return null;
    const hotel = this.hotelStore.hotels().find(h => h.id === this.selectedHotelId);
    return hotel?.name ?? null;
  }
}
