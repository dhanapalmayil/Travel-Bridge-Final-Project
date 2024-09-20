import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BookingService } from '../services/booking.service';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { catchError } from 'rxjs/operators';
import { of } from 'rxjs';


@Component({
  selector: 'app-booking',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './booking.component.html',
  styleUrls: ['./booking.component.css']
})
export class BookingComponent implements OnInit {
  bookingForm: FormGroup;
  seats: any[] = [];
  selectedSeats: Map<number, string> = new Map(); // Track selected seat IDs
  scheduleId: number | undefined;
  isSubmitting: boolean = false; // To disable the submit button while processing
  errorMessage: string = ''; // Store error messages
  userId: number | null;
  totalAmount = 0;
  selectedSeatsCount = 0;

  constructor(
    private fb: FormBuilder,
    private bookingService: BookingService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    // Safely get userId from localStorage
    const storedUserId = localStorage.getItem('userId');
    this.userId = storedUserId ? +storedUserId : null; // Convert to number or set to null if not available

    this.bookingForm = this.fb.group({
      userId: [this.userId, Validators.required],
      scheduleId: [null, Validators.required],
      bookingDate: [new Date(), Validators.required],
      numberOfSeats: [1, [Validators.required, Validators.min(1)]],
      totalAmount: [0, Validators.required],
      bookingStatus: ['Booked', Validators.required]
    });
  }

  ngOnInit(): void {
    // Retrieve query params to get the scheduleId
    this.route.queryParams.subscribe(params => {
      this.scheduleId = +params['scheduleId'];
      if (this.scheduleId) {
        this.bookingForm.get('scheduleId')?.setValue(this.scheduleId);
        this.loadSeats(this.scheduleId); // Load seats here only once
      }
    });
  }

  // Load available seats for a specific schedule
  loadSeats(scheduleId: number): void {
    this.bookingService.getSeats(scheduleId)
      .pipe(
        catchError((error) => {
          console.error('Error loading seats:', error);
          this.errorMessage = 'Error loading seats. Please try again later.';
          return of([]); // Return an empty array on error
        })
      )
      .subscribe(data => {
        this.seats = data;
      });
  }

  onSeatSelect(seat: any): void {
    console.log(seat);
    if (seat.isBooked) {
      return; // Don't allow selection of booked seats
    }
    
    // Toggle seat selection locally
    if (this.selectedSeats.has(seat.id)) {
      this.selectedSeats.delete(seat.id);
    } else {
      this.selectedSeats.set(seat.id, seat.number);
    }
  
    // Update seat selection in UI only, without calling API
    this.seats.forEach(seatType => {
      seatType.seats.forEach((s: { id: any; isSelected: boolean; }) => {
        if (s.id === seat.id) {
          s.isSelected = this.selectedSeats.has(seat.id);
        }
      });
    });
  
    // Update total amount
    this.totalAmount = this.seats.reduce((total, seatType) => {
      const selectedSeats = seatType.seats.filter((seat: { isSelected: any; }) => seat.isSelected);
      return total + selectedSeats.length * seatType.price;
    }, 0);
  
    // Update selected seats count
    this.selectedSeatsCount = this.selectedSeats.size;
  
    // Update form controls
    this.bookingForm.patchValue({
      numberOfSeats: this.selectedSeatsCount,
      totalAmount: this.totalAmount
    });
  }
  // Group seats into rows of 10
  getSeatRows(seats: any[]): any[][] {
    const rows = [];
    for (let i = 0; i < seats.length; i += 10) {
      rows.push(seats.slice(i, i + 10));
    }
    return rows;
  }

 
  onSubmit(): void {
    console.log('inside on submit');
    this.errorMessage = ''; // Reset error message before submit

    
    if (this.bookingForm.invalid) {
      this.errorMessage = 'Please fill out all required fields.';
      return;
    }

    if (this.selectedSeats.size === 0) {
      this.errorMessage = 'No seats selected. Please select at least one seat.';
      return;
    }
    this.isSubmitting = true;
    console.log(this.selectedSeats);
    // Convert selectedSeats to query string format
    const selectedSeatsString = Array.from(this.selectedSeats.entries())
      .map(([id, number]) => `${id}-${number}`)
      .join(',');

    this.router.navigate(['/payment'], {
      queryParams: {
        scheduleId: this.scheduleId,
        selectedSeats: selectedSeatsString,
        totalAmount: this.totalAmount
      }
    });
  }
}