import { Injectable, inject, signal } from '@angular/core';
import { Api } from '../../api/generated/api';
import { DashboardDto } from '../../api/generated/models/dashboard-dto';
import { apiDashboardGet } from '../../api/generated/fn/dashboard/api-dashboard-get';

@Injectable({ providedIn: 'root' })
export class DashboardStore {
  private api = inject(Api);

  summary = signal<DashboardDto | null>(null);
  loading = signal(false);
  error = signal<string | null>(null);

  getSummary() {
    this.loading.set(true);
    this.error.set(null);
    this.api.invoke(apiDashboardGet).then((data: DashboardDto) => {
      this.summary.set(data);
      this.loading.set(false);
    }).catch((err) => {
      this.error.set(err?.message ?? 'An unexpected error occurred');
      this.loading.set(false);
    });
  }
}
