import { Component, OnInit } from '@angular/core';
import { UrlShortenerService } from '../../../services/url-shortener/url-shortener.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-create-short-url',
  templateUrl: './create-short-url.component.html',
  styleUrls: ['./create-short-url.component.scss'] 
})
export class CreateShortUrlComponent implements OnInit{
  urlForm!: FormGroup;
  originalUrl!: string;
  successMessage: string | null = null;
  errorMessage: string | null = null;

  constructor(private fb: FormBuilder, 
    private urlShortenerService: UrlShortenerService,
    private toastr: ToastrService) { 
    this.originalUrl = ''; 
  }

  ngOnInit(): void {
    this.urlForm = this.fb.group({
      originalUrl: ['', [Validators.required, Validators.pattern('https?://.+')]]
    });
  }

  onSubmit() {
    if (this.urlForm.valid) {
      const originalUrl = this.urlForm.value.originalUrl;
      this.createShortUrl(originalUrl);
    } else {
      // Mark all form controls as touched to trigger error messages
      this.markFormGroupTouched(this.urlForm);
    }
  }

  createShortUrl(originalUrl: string) {
    this.urlShortenerService.create(originalUrl).subscribe(
      response => {
        // Handle successful response
        this.toastr.success('Short URL created successfully!', 'Success');
        console.log("Short URL created successfully:", response);
      },
      error => {
        // Handle error response
        console.error("Error creating short URL:", error);
        this.toastr.error('Error creating short URL. Please try again later.', 'Error');
        if (error.status === 400) {
          // Handle validation errors from the backend
          const validationErrors = error.error.errors;
          console.log("Validation errors:", validationErrors);
        } else {
          // Handle other types of errors (e.g., server errors)
          console.error("Server error occurred:", error.error);
        }
      }
    );
  }
  
// Helper function to mark all form controls as touched
markFormGroupTouched(formGroup: FormGroup) {
  Object.values(formGroup.controls).forEach(control => {
    control.markAsTouched();
    if (control instanceof FormGroup) {
      this.markFormGroupTouched(control);
    }
  });
}
}