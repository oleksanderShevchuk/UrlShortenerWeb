import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { DescriptionService } from '../../services/description/description.service';
import { SharedService } from '../../shared/shared.service';
import { Description } from '../../models/description/description.model';
import { AccountService } from '../../account/account.service';
import { DescriptionEditDto } from '../../models/description/description-edit-dto.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-description',
  templateUrl: './description.component.html',
  styleUrl: './description.component.scss'
})
export class DescriptionComponent implements OnInit {
  description: Description = {} as Description;
  descriptionEditDto: DescriptionEditDto = {} as DescriptionEditDto; 
  isAdmin: boolean = false;
  editingEnabled: boolean = false;
  editedContent: string = '';

  constructor(
    private descriptionService: DescriptionService,
    private sharedService: SharedService,
    private accountService: AccountService,
    private router: Router
  ) {}

  ngOnInit(): void {
    const id = 1;
    this.descriptionService.getDescription(id).subscribe(
      (data) => {
        this.description = data;
      },
      (error) => {
        console.error('Error fetching description:', error);
      }
    );
    // Check if the current user is an admin
    this.isAdmin = this.accountService.getUserRole() === 'Admin';
  }

  enableEditing(): void {
    this.editedContent = this.description.content;
    this.editingEnabled = true;
  }

  saveChanges(): void {
    this.descriptionEditDto = {
      id: this.description.id,
      content: this.editedContent,
      lastUpdatedTime: this.description.lastUpdatedTime,
    };
    this.descriptionService.editDescription(this.descriptionEditDto).subscribe(
      () => {
        console.log('Description updated successfully');
        this.sharedService.showNotification(true, "Success", "Successfully update description");
        this.editingEnabled = false; // Disable editing mode
        this.description.content = this.editedContent;
        this.router.navigate([this.router.url]);
      },
      (error) => {
        console.error('Error updating description:', error);
        this.sharedService.showNotification(false, "Error", "Failed to update description");
      }
    );
  }
}