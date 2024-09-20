import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { AdminService } from '../../services/admin.service';
import { CommonModule } from '@angular/common';
import { AdminHeaderComponent } from "../admin-header/admin-header.component";

@Component({
  selector: 'app-providers',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, AdminHeaderComponent],
  templateUrl: './providers.component.html',
  styleUrls: ['./providers.component.css']
})
export class ProvidersComponent implements OnInit {
  providers: any[] = [];
  selectedProvider: any = null;
  providerForm: FormGroup;
  errorMessage: string | null = null;

  constructor(private adminService: AdminService, private fb: FormBuilder) {
    this.providerForm = this.fb.group({
      providerID: [''],
      providerName: ['', Validators.required],
      contactEmail: ['', [Validators.required, Validators.email]],
      contactPhone: ['', Validators.required],
      providerType: ['', Validators.required],
      transportName: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.loadProviders();
  }

  loadProviders() {
    this.adminService.getProviders().subscribe(
      (data: any[]) => {
        this.providers = data;
      },
      (error) => {
        console.error('Error loading providers:', error);
        this.errorMessage = 'Failed to load providers.';
      }
    );
  }

  selectProvider(provider: any) {
    this.selectedProvider = provider;
    this.providerForm.patchValue({
      providerID: provider.providerID,
      providerName: provider.providerName,
      contactEmail: provider.contactEmail,
      contactPhone: provider.contactPhone,
      providerType: provider.providerType,
      transportName: provider.transportName
    });
  }

  updateProvider() {
    if (this.providerForm.valid) {
      const providerId = this.providerForm.value.providerID;
      const updatedData = {
        providerName: this.providerForm.value.providerName,
        contactEmail: this.providerForm.value.contactEmail,
        contactPhone: this.providerForm.value.contactPhone,
        providerType: this.providerForm.value.providerType,
        transportName: this.providerForm.value.transportName
      };
      this.adminService.updateProvider(providerId, updatedData).subscribe(
        () => {
          this.loadProviders();  // Refresh the provider list
          this.selectedProvider = null;  // Reset form
        },
        (error) => {
          console.error('Error updating provider:', error);
          this.errorMessage = 'Failed to update provider.';
        }
      );
    }
  }

  deleteProvider(providerId: number) {
    this.adminService.deleteProvider(providerId).subscribe(
      () => {
        this.loadProviders();
      },
      (error) => {
        console.error('Error deleting provider:', error);
        this.errorMessage = 'Failed to delete provider.';
      }
    );
  }
}
