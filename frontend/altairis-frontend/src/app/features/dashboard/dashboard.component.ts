import { ChangeDetectionStrategy, Component, inject, OnInit } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import { DecimalPipe } from '@angular/common';
import { DashboardStore } from '../../core/stores/dashboard.store';
import { ReservationsByStatusDto } from '../../api/generated/models/reservations-by-status-dto';

@Component({
  selector: 'app-dashboard',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [MatCardModule, MatProgressSpinnerModule, MatIconModule, DecimalPipe],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss',
})
export class DashboardComponent implements OnInit {
  dashboardStore = inject(DashboardStore);
  ngOnInit() {
    this.dashboardStore.getSummary();
  }

  getDonutGradient(status: ReservationsByStatusDto): string {
    const total = (status.pending ?? 0) + (status.confirmed ?? 0) + (status.cancelled ?? 0);
    if (total === 0) return 'conic-gradient(#e0e0e0 0deg 360deg)';
    const pendingDeg = ((status.pending ?? 0) / total) * 360;
    const confirmedDeg = pendingDeg + ((status.confirmed ?? 0) / total) * 360;
    return `conic-gradient(#ff9800 0deg ${pendingDeg}deg, #4caf50 ${pendingDeg}deg ${confirmedDeg}deg, #f44336 ${confirmedDeg}deg 360deg)`;
  }

  getTotalReservationsByStatus(status: ReservationsByStatusDto): number {
    return (status.pending ?? 0) + (status.confirmed ?? 0) + (status.cancelled ?? 0);
  }
}
