import { ChangeDetectionStrategy, Component, inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { FormsModule } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { HotelStore } from '../../../core/stores/hotel.store';

@Component({
  selector: 'app-hotel-list',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    MatTableModule, MatButtonModule, MatInputModule, MatFormFieldModule,
    MatIconModule, MatProgressSpinnerModule, MatPaginatorModule, FormsModule, DatePipe,
  ],
  templateUrl: './hotel-list.component.html',
  styleUrl: './hotel-list.component.scss',
})
export class HotelListComponent implements OnInit {
  hotelStore = inject(HotelStore);
  router = inject(Router);
  searchQuery = '';
  pageSize = 10;
  currentPage = 1;
  displayedColumns = ['name', 'country', 'city', 'address', 'createdAt'];

  ngOnInit() {
    this.loadHotels();
  }

  onSearch() {
    this.currentPage = 1;
    this.loadHotels();
  }

  onPageChange(event: PageEvent) {
    this.currentPage = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.loadHotels();
  }

  private loadHotels() {
    if (this.searchQuery.trim()) {
      this.hotelStore.search(this.searchQuery.trim(), this.currentPage, this.pageSize);
    } else {
      this.hotelStore.getAll(this.currentPage, this.pageSize);
    }
  }
}
