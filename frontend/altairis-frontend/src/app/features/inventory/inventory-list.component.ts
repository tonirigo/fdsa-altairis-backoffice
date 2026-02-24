import { ChangeDetectionStrategy, Component, effect, inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTooltipModule } from '@angular/material/tooltip';
import { DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HotelStore } from '../../core/stores/hotel.store';
import { AvailabilityStore } from '../../core/stores/availability.store';
import { InventoryCell } from '../../api/generated/models/inventory-cell';
import { formatDateToISO } from '../../shared/utils/date.util';

@Component({
  selector: 'app-inventory-list',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    MatSelectModule, MatFormFieldModule, MatButtonModule,
    MatIconModule, MatProgressSpinnerModule, MatTooltipModule,
    DatePipe, FormsModule,
  ],
  templateUrl: './inventory-list.component.html',
  styleUrl: './inventory-list.component.scss',
})
export class InventoryListComponent implements OnInit {
  hotelStore = inject(HotelStore);
  availabilityStore = inject(AvailabilityStore);
  router = inject(Router);
  private route = inject(ActivatedRoute);

  selectedHotelId: number | null = null;
  private weekStart: Date = this.getMonday(new Date());
  private pendingHotelId: number | null = null;
  private hotelLoadedEffect = effect(() => {
    const hotels = this.hotelStore.hotels();
    if (hotels.length > 0 && this.pendingHotelId) {
      this.selectedHotelId = this.pendingHotelId;
      this.pendingHotelId = null;
      this.loadGrid();
    }
  });

  ngOnInit() {
    this.selectedHotelId = null;
    this.weekStart = this.getMonday(new Date());
    this.availabilityStore.reset();

    const hotelIdParam = this.route.snapshot.queryParamMap.get('hotelId');
    if (hotelIdParam) {
      this.pendingHotelId = +hotelIdParam;
    }
    this.hotelStore.loadAll();
  }

  onHotelChange() {
    if (this.selectedHotelId) {
      this.loadGrid();
    }
  }

  prevWeek() {
    this.weekStart = new Date(this.weekStart.getTime() - 7 * 86400000);
    this.loadGrid();
  }

  nextWeek() {
    this.weekStart = new Date(this.weekStart.getTime() + 7 * 86400000);
    this.loadGrid();
  }

  goToday() {
    this.weekStart = this.getMonday(new Date());
    this.loadGrid();
  }

  isToday(dateStr: string): boolean {
    const now = new Date();
    const today = `${now.getFullYear()}-${String(now.getMonth()+1).padStart(2,'0')}-${String(now.getDate()).padStart(2,'0')}`;
    return dateStr.split('T')[0] === today;
  }

  getOccupancyPct(cell: InventoryCell): number {
    if (!cell.totalRooms) return 0;
    return ((cell.totalRooms - (cell.availableRooms ?? 0)) / cell.totalRooms) * 100;
  }

  getCellTooltip(roomName: string, cell: InventoryCell): string {
    if (!cell.totalRooms) return `${roomName}: No inventory`;
    const occ = Math.round(this.getOccupancyPct(cell));
    return `${roomName}: ${cell.availableRooms} available of ${cell.totalRooms} (${occ}% occupied)`;
  }

  getSelectedHotelName(): string | null {
    if (!this.selectedHotelId) return null;
    const hotel = this.hotelStore.hotels().find(h => h.id === this.selectedHotelId);
    return hotel?.name ?? null;
  }

  private loadGrid() {
    if (!this.selectedHotelId) return;
    const from = formatDateToISO(this.weekStart);
    const to = formatDateToISO(new Date(this.weekStart.getTime() + 6 * 86400000));
    this.availabilityStore.getHotelGrid(this.selectedHotelId, from, to);
  }

  private getMonday(d: Date): Date {
    const date = new Date(d);
    const day = date.getDay();
    const diff = date.getDate() - day + (day === 0 ? -6 : 1);
    date.setDate(diff);
    date.setHours(0, 0, 0, 0);
    return date;
  }

}
