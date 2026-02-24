import { ChangeDetectionStrategy, Component, effect, inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { HotelStore } from '../../../core/stores/hotel.store';
import { RoomTypeStore } from '../../../core/stores/room-type.store';
import { ReservationStore } from '../../../core/stores/reservation.store';
import { CreateReservationDto } from '../../../api/generated/models/create-reservation-dto';
import { formatDateToISO } from '../../../shared/utils/date.util';

@Component({
  selector: 'app-reservation-create',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    FormsModule, MatInputModule, MatFormFieldModule, MatSelectModule,
    MatButtonModule, MatCardModule, MatIconModule, MatDatepickerModule, MatNativeDateModule,
  ],
  templateUrl: './reservation-create.component.html',
  styleUrl: './reservation-create.component.scss',
})
export class ReservationCreateComponent implements OnInit {
  hotelStore = inject(HotelStore);
  roomTypeStore = inject(RoomTypeStore);
  private reservationStore = inject(ReservationStore);
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private snackBar = inject(MatSnackBar);

  selectedHotelId: number | null = null;
  selectedRoomTypeId: number | null = null;
  checkInDate: Date | null = null;
  checkOutDate: Date | null = null;
  guestName = '';
  roomsBooked = 1;
  submitting = false;

  private returnUrl = '/reservations';
  returnLabel = 'Back to Reservations';

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
    const params = this.route.snapshot.queryParamMap;
    const hotelIdParam = params.get('hotelId');
    if (hotelIdParam) {
      this.pendingHotelId = +hotelIdParam;
    }
    const returnUrlParam = params.get('returnUrl');
    if (returnUrlParam) {
      this.returnUrl = returnUrlParam;
      const labelMap: Record<string, string> = {
        '/inventory': 'Back to Inventory',
        '/reservations': 'Back to Reservations',
      };
      this.returnLabel = labelMap[returnUrlParam] ?? 'Go Back';
    }
    this.hotelStore.loadAll();
  }

  goBack() {
    const queryParams: Record<string, number> = {};
    if (this.selectedHotelId && this.returnUrl !== '/reservations') {
      queryParams['hotelId'] = this.selectedHotelId;
    }
    this.router.navigate([this.returnUrl], { queryParams });
  }

  onHotelChange() {
    this.selectedRoomTypeId = null;
    if (this.selectedHotelId) {
      this.roomTypeStore.getByHotel(this.selectedHotelId);
    }
  }

  onSubmit() {
    if (this.selectedRoomTypeId && this.checkInDate && this.checkOutDate) {
      this.submitting = true;
      const payload: CreateReservationDto = {
        roomTypeId: this.selectedRoomTypeId,
        checkIn: formatDateToISO(this.checkInDate),
        checkOut: formatDateToISO(this.checkOutDate),
        guestName: this.guestName,
        roomsBooked: this.roomsBooked,
      };
      this.reservationStore.create(payload).subscribe({
        next: () => {
          this.submitting = false;
          this.snackBar.open('Reservation created successfully', 'Close', { duration: 3000 });
          this.goBack();
        },
        error: () => {
          this.submitting = false;
          this.snackBar.open('Failed to create reservation', 'Close', { duration: 5000 });
        }
      });
    }
  }
}
