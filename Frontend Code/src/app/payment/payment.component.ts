import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BookingService } from '../services/booking.service';
import { HeaderComponent } from '../header/header.component';

@Component({
  selector: 'app-payment',
  standalone: true,
  imports: [FormsModule, CommonModule,HeaderComponent],
  templateUrl: './payment.component.html',
  styleUrls: ['./payment.component.css'] // Corrected from styleUrl to styleUrls
})
export class PaymentComponent implements OnInit {
  scheduleId: number = 0;
  selectedSeats: Map<number, string> = new Map();
  totalAmount: number = 0;
  seatCount: number = 0;
  userId: string | null = localStorage.getItem('userId');

  paymentData = {
    name: '',
    cardNumber: '',
    expiryDate: '',
    cvv: ''
};


  constructor(private route: ActivatedRoute,private bookingService: BookingService,private router:Router) {}

  ngOnInit(): void {
    // Retrieve query parameters
    this.route.queryParams.subscribe(params => {
      this.scheduleId = +params['scheduleId']; 
      this.totalAmount = +params['totalAmount']; 

      // Parse selectedSeats from query parameter
      const selectedSeatsString = params['selectedSeats'];
      if (selectedSeatsString) {
        this.selectedSeats = new Map(
          selectedSeatsString.split(',')
            .map((item: { split: (arg0: string) => [any, ...any[]]; }) => {
              const [id, ...seatParts] = item.split('-');
              const seatNumber = seatParts.join('-'); // Combine the remaining parts as seat number
              return id && seatNumber ? [+id, seatNumber] : null; // Ensure both id and seatNumber are present
            })
            .filter((item: null) => item !== null) // Remove any null values
        );
      }
    });
    this.seatCount = this.selectedSeats.size;
    
  }

  handlePayment(){
    const data = {
      'userId': this.userId,
      'scheduleId': this.scheduleId,
      'bookingDate': new Date(),
      'numberOfSeats': this.selectedSeats.size,
      'totalAmount': this.totalAmount,
      'BookingStatus': "Booked",
      'PaymentDate': new Date(),
      'PaymentMethod': "Dedit Card",
      'AmountPaid': this.totalAmount,
      'PaymentStatus': "Success",
      'seatIds': Array.from(this.selectedSeats.keys())
    }
    this.bookingService.createBooking(data).subscribe(
      (response) => {
        console.log("Payment made successfully")
      }
    )
    this.router.navigate(['/payed'])
  }
}



