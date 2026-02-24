import { ChangeDetectionStrategy, Component, inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatDialog } from '@angular/material/dialog';
import { DatePipe } from '@angular/common';
import { ReservationStore } from '../../../core/stores/reservation.store';
import { HotelStore } from '../../../core/stores/hotel.store';
import { formatDateToISO } from '../../../shared/utils/date.util';
import { ConfirmDialogComponent } from '../../../shared/components/confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-reservation-list',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    MatTableModule, MatButtonModule, MatIconModule, MatProgressSpinnerModule,
    MatSelectModule, MatFormFieldModule, MatInputModule, MatDatepickerModule, MatNativeDateModule,
    MatPaginatorModule, MatTooltipModule, FormsModule, DatePipe,
  ],
  templateUrl: './reservation-list.component.html',
  styleUrl: './reservation-list.component.scss',
})
export class ReservationListComponent implements OnInit {
  reservationStore = inject(ReservationStore);
  hotelStore = inject(HotelStore);
  router = inject(Router);
  private snackBar = inject(MatSnackBar);
  private dialog = inject(MatDialog);

  displayedColumns = ['hotelName', 'roomTypeName', 'guestName', 'checkIn', 'checkOut', 'roomsBooked', 'status', 'actions'];

  selectedHotelId: number | undefined;
  dateFrom: Date | null = null;
  dateTo: Date | null = null;
  selectedStatus: 'Pending' | 'Confirmed' | 'Cancelled' | undefined;
  pageSize = 20;
  private currentPage = 1;

  ngOnInit() {
    this.selectedHotelId = undefined;
    this.dateFrom = null;
    this.dateTo = null;
    this.selectedStatus = undefined;
    this.currentPage = 1;
    this.reservationStore.reset();

    this.hotelStore.loadAll();
    this.loadReservations();
  }

  private loadReservations() {
    const from = this.dateFrom ? formatDateToISO(this.dateFrom) : undefined;
    const to = this.dateTo ? formatDateToISO(this.dateTo) : undefined;
    this.reservationStore.loadFiltered(this.selectedHotelId, from, to, this.selectedStatus, this.currentPage, this.pageSize);
  }

  onFilterChange() {
    this.currentPage = 1;
    this.loadReservations();
  }

  onPageChange(event: PageEvent) {
    this.currentPage = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.loadReservations();
  }

  onConfirm(id: number) {
    this.reservationStore.updateStatus(id, 'Confirmed').subscribe({
      next: () => {
        this.snackBar.open('Reservation confirmed', 'Close', { duration: 3000 });
        this.loadReservations();
      },
      error: () => {
        this.snackBar.open('Failed to confirm reservation', 'Close', { duration: 5000 });
      }
    });
  }

  onCancel(id: number) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Cancel Reservation',
        message: 'Are you sure you want to cancel this reservation? This action cannot be undone.',
        confirmText: 'Cancel Reservation',
      },
    });
    dialogRef.afterClosed().subscribe((confirmed) => {
      if (confirmed) {
        this.reservationStore.updateStatus(id, 'Cancelled').subscribe({
          next: () => {
            this.snackBar.open('Reservation cancelled', 'Close', { duration: 3000 });
            this.loadReservations();
          },
          error: () => {
            this.snackBar.open('Failed to cancel reservation', 'Close', { duration: 5000 });
          }
        });
      }
    });
  }

  getStatusClass(status: string): string {
    const map: Record<string, string> = { 'Pending': 'status-pending', 'Confirmed': 'status-confirmed', 'Cancelled': 'status-cancelled' };
    return map[status] ?? '';
  }
}
