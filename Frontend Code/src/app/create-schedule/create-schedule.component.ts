import { Component, OnInit } from '@angular/core';
import { TransportScheduleService } from '../services/transport-schedule.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HeaderComponent } from '../header/header.component';

@Component({
  selector: 'app-create-schedule',
  standalone: true,
  imports: [CommonModule, FormsModule,HeaderComponent],
  templateUrl: './create-schedule.component.html',
  styleUrls: ['./create-schedule.component.css']
})
export class CreateScheduleComponent implements OnInit {

  schedule = {
    providerID: '',
    modeID: '',
    transportName: '',
    origin: '',
    destination: '',
    departureTime: '',
    arrivalTime: '',
    seatCapacity: 0,
    price: 0,
    ticketTypes: [
      { ticketTypeName: '', numberOfSeats: 0, price: 0 }
    ]
  };

  minDateTime: string = '';

  constructor(private transportScheduleService: TransportScheduleService) {}

  ngOnInit(): void {
    this.initializeForm();
    this.setMinDateTime();
  }

  initializeForm(): void {
    const providerID = localStorage.getItem('providerId');
    const transportName = localStorage.getItem('TransportName');
    const providerType = localStorage.getItem('providerType');

    if (providerID) {
      this.schedule.providerID = providerID;
    }

    if (transportName) {
      this.schedule.transportName = transportName;
    }

    if (providerType === 'Bus') {
      this.schedule.modeID = '1';
    } else if (providerType === 'Train') {
      this.schedule.modeID = '2';
    } else if (providerType === 'Flight') {
      this.schedule.modeID = '3';
    }

    this.calculateAveragePrice(); // Initial calculation
  }

  setMinDateTime(): void {
    const now = new Date();
    this.minDateTime = now.toISOString().slice(0, 16); // Format: YYYY-MM-DDTHH:MM
  }

  addTicketType(): void {
    this.schedule.ticketTypes.push({ ticketTypeName: '', numberOfSeats: 0, price: 0 });
    this.calculateAveragePrice(); // Recalculate average price
  }

  removeTicketType(index: number): void {
    this.schedule.ticketTypes.splice(index, 1);
    this.calculateAveragePrice(); // Recalculate average price
  }

  updateTicketType(): void {
    this.calculateAveragePrice(); // Recalculate average price
  }

  calculateAveragePrice(): void {
    const totalPrices = this.schedule.ticketTypes.reduce((sum, ticketType) => sum + ticketType.price, 0);
    const numberOfTickets = this.schedule.ticketTypes.length;
    this.schedule.price = numberOfTickets > 0 ? totalPrices / numberOfTickets : 0;
  }

  onSubmit(): void {
    this.transportScheduleService.createSchedule(this.schedule)
      .subscribe(
        response => {
          console.log('Schedule created successfully', response);
          alert('Schedule created successfully!');
        },
        error => {
          console.error('Error creating schedule', error);
          alert('There was an error creating the schedule.');
        }
      );
  }
}
