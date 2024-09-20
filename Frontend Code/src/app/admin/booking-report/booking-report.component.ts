import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { AdminService } from '../../services/admin.service';
import { CommonModule } from '@angular/common';
import { AdminHeaderComponent } from "../admin-header/admin-header.component";


@Component({
  selector: 'app-booking-report',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, AdminHeaderComponent],
  templateUrl: './booking-report.component.html',
  styleUrl: './booking-report.component.css'
})
export class BookingReportComponent implements OnInit {
  reportForm: FormGroup;
  bookings: any[] = [];

  constructor(private fb: FormBuilder, private adminService: AdminService) {
    this.reportForm = this.fb.group({
      startDate: ['', Validators.required],
      endDate: ['', Validators.required]
    });
  }

  ngOnInit(): void {
  }

  fetchReport() {
    if (this.reportForm.valid) {
      const { startDate, endDate } = this.reportForm.value;
      this.adminService.getBookingReport(startDate, endDate).subscribe((data: any) => {
        // Ensure reportContent is parsed from a JSON string into an object
        if (data && data.reportContent) {
          try {
            this.bookings = JSON.parse(data.reportContent);
          } catch (error) {
            console.error("Error parsing reportContent:", error);
          }
        }
      });
    }
  }
  
}
