import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { AdminService } from '../../services/admin.service';
import { CommonModule } from '@angular/common';
import { AdminHeaderComponent } from "../admin-header/admin-header.component";

@Component({
  selector: 'app-revenue',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, AdminHeaderComponent],
  templateUrl: './revenue.component.html',
  styleUrl: './revenue.component.css'
})

export class RevenueComponent implements OnInit {
  reportForm: FormGroup;
  revenueReport: any;

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
      this.adminService.getRevenueReport(startDate, endDate).subscribe((data: any) => {
        this.revenueReport = data;
      });
    }
  }
}
