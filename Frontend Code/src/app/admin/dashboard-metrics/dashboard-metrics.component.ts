import { Component, OnInit } from '@angular/core';
import { AdminService } from '../../services/admin.service';
import { CommonModule } from '@angular/common';
import { AdminHeaderComponent } from "../admin-header/admin-header.component";

@Component({
  selector: 'app-dashboard-metrics',
  standalone: true,
  imports: [CommonModule, AdminHeaderComponent],
  templateUrl: './dashboard-metrics.component.html',
  styleUrl: './dashboard-metrics.component.css'
})
export class DashboardMetricsComponent implements OnInit {
  metrics: any = {};

  constructor(private adminService: AdminService) {}

  ngOnInit(): void {
    this.loadMetrics();
  }

  loadMetrics() {
    this.adminService.getDashboardMetrics().subscribe((data: any) => {
      this.metrics = data;
    });
  }
}