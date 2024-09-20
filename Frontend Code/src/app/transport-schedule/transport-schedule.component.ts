import { Component } from '@angular/core';
import { TransportScheduleService } from '../services/transport-schedule.service';
import { CommonModule } from '@angular/common';
import { FormArray, FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { HeaderComponent } from '../header/header.component';

@Component({
  selector: 'app-transport-schedule',
  standalone: true,
  imports: [CommonModule, FormsModule,ReactiveFormsModule,HeaderComponent],
  templateUrl: './transport-schedule.component.html',
  styleUrls: ['./transport-schedule.component.css']
})
export class TransportScheduleComponent {
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

  constructor(private transportScheduleService: TransportScheduleService) { }

  addTicketType(): void {
    this.schedule.ticketTypes.push({ ticketTypeName: '', numberOfSeats: 0, price: 0 });
  }

  removeTicketType(index: number): void {
    this.schedule.ticketTypes.splice(index, 1);
  }

  onSubmit(): void {
    // Call the service to send the form data to the backend API
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