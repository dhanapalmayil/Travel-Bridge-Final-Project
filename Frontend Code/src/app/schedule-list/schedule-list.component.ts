import { Component, OnInit } from '@angular/core';
import { TransportScheduleService } from '../services/transport-schedule.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HeaderComponent } from '../header/header.component';

@Component({
  selector: 'app-schedule-list',
  standalone: true,
  imports: [CommonModule,HeaderComponent],
  templateUrl: './schedule-list.component.html',
  styleUrl: './schedule-list.component.css'
})
export class ScheduleListComponent implements OnInit {
  schedules: any[] = [CommonModule,FormsModule];
  selectedScheduleId: number | null = null;

  constructor(private transportService: TransportScheduleService) {}

  ngOnInit(): void {
    this.getSchedules();
  }

  // Fetch schedules from the backend
  getSchedules(): void {
    const providerId = parseInt(localStorage.getItem('providerId')!, 10); // The '10' specifies base-10 (decimal).
    this.transportService.getGroupedDetailsByProvider(providerId).subscribe((response: any) => {
      this.schedules = response;
    });
  }

  // Toggle booking display for a selected schedule
  toggleBookings(scheduleID: number): void {
    if (this.selectedScheduleId === scheduleID) {
      this.selectedScheduleId = null;
    } else {
      this.selectedScheduleId = scheduleID;
    }
  }
}