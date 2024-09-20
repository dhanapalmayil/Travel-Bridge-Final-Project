import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { AdminService } from '../../services/admin.service';
import { CommonModule } from '@angular/common';
import { AdminHeaderComponent } from '../admin-header/admin-header.component';

@Component({
  selector: 'app-users',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, AdminHeaderComponent],
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {
  users: any[] = [];
  selectedUser: any = null;
  userForm: FormGroup;
  errorMessage: string | null = null;

  constructor(private adminService: AdminService, private fb: FormBuilder) {
    this.userForm = this.fb.group({
      userId: [''],
      userName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      userRole: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers() {
    this.adminService.getUsers().subscribe(
      (data: any[]) => {
        this.users = data;
      },
      (error) => {
        console.error('Error loading users:', error);
        this.errorMessage = 'Failed to load users.';
      }
    );
  }

  selectUser(user: any) {
    this.selectedUser = user;
    this.userForm.patchValue({
      userId: user.userID,
      userName: user.userName,
      email: user.email,
      userRole: user.userRole
    });
  }

  updateUser() {
    if (this.userForm.valid) {
      const userId = this.userForm.value.userId;
      const updatedData = {
        userName: this.userForm.value.userName,
        email: this.userForm.value.email,
        userRole: this.userForm.value.userRole
      };
      this.adminService.updateUser(userId, updatedData).subscribe(
        () => {
          this.loadUsers();  // Refresh the user list
          this.selectedUser = null;  // Reset form
        },
        (error) => {
          console.error('Error updating user:', error);
          this.errorMessage = 'Failed to update user.';
        }
      );
    }
  }

  deleteUser(userId: number) {
    this.adminService.deleteUser(userId).subscribe(
      () => {
        this.loadUsers();
      },
      (error) => {
        console.error('Error deleting user:', error);
        this.errorMessage = 'Failed to delete user.';
      }
    );
  }
}
