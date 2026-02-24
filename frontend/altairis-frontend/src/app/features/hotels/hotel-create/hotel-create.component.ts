import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar } from '@angular/material/snack-bar';
import { HotelStore } from '../../../core/stores/hotel.store';
import { CreateHotelDto } from '../../../api/generated/models/create-hotel-dto';

@Component({
  selector: 'app-hotel-create',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [FormsModule, MatInputModule, MatFormFieldModule, MatButtonModule, MatCardModule, MatIconModule],
  templateUrl: './hotel-create.component.html',
  styleUrl: './hotel-create.component.scss',
})
export class HotelCreateComponent {
  private hotelStore = inject(HotelStore);
  private snackBar = inject(MatSnackBar);
  router = inject(Router);

  hotel: CreateHotelDto = { name: '', country: '', city: '', address: '' };
  submitting = false;

  onSubmit() {
    this.submitting = true;
    this.hotelStore.create(this.hotel).subscribe({
      next: () => {
        this.submitting = false;
        this.snackBar.open('Hotel created successfully', 'Close', { duration: 3000 });
        this.router.navigate(['/hotels']);
      },
      error: () => {
        this.submitting = false;
        this.snackBar.open('Failed to create hotel', 'Close', { duration: 5000 });
      }
    });
  }
}
