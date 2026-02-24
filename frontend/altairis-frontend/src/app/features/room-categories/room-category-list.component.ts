import { ChangeDetectionStrategy, Component, inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSnackBar } from '@angular/material/snack-bar';
import { FormsModule } from '@angular/forms';
import { RoomCategoryStore } from '../../core/stores/room-category.store';

@Component({
  selector: 'app-room-category-list',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    MatTableModule, MatButtonModule, MatIconModule,
    MatProgressSpinnerModule, MatInputModule, MatFormFieldModule, FormsModule,
  ],
  templateUrl: './room-category-list.component.html',
  styleUrl: './room-category-list.component.scss',
})
export class RoomCategoryListComponent implements OnInit {
  categoryStore = inject(RoomCategoryStore);
  private router = inject(Router);
  private snackBar = inject(MatSnackBar);
  showForm = false;
  newName = '';
  submitting = false;

  ngOnInit() {
    this.categoryStore.getAll();
  }

  onAdd() {
    this.showForm = true;
    this.newName = '';
  }

  onSave() {
    if (this.newName.trim()) {
      this.submitting = true;
      this.categoryStore.create({ name: this.newName.trim() }).subscribe({
        next: () => {
          this.submitting = false;
          this.snackBar.open('Category created successfully', 'Close', { duration: 3000 });
          this.showForm = false;
          this.newName = '';
          this.categoryStore.getAll();
        },
        error: () => {
          this.submitting = false;
          this.snackBar.open('Failed to create category', 'Close', { duration: 5000 });
        }
      });
    }
  }
}
