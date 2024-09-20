import { Component, OnInit } from '@angular/core';
import { ProviderService } from '../services/provider.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-provider-form',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './provider-form.component.html',
  styleUrls: ['./provider-form.component.css']
})
export class ProviderFormComponent implements OnInit {
  providerData: any = {}; // Plain object to store form data

  constructor(private providerService: ProviderService) {}

  ngOnInit(): void {
    this.providerData.providerID = 0; // Set default value, can be handled by backend
  }

  onSubmit(form: any): void {
    if (form.valid) {
      this.providerService.postProviderData(this.providerData).subscribe(
        response => {
          console.log('Successfully Registered', response);
        },
        error => {
          console.error('Something Went Wrong :(', error);
        }
      );
    }
  }
}
