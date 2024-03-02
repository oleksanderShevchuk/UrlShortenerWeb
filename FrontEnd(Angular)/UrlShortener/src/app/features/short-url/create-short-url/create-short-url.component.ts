import { Component, OnInit } from '@angular/core';
import { UrlShortenerService } from '../../../services/url-shortener/url-shortener.service';
import { AsyncValidatorFn, FormBuilder, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Observable, map, catchError, of } from 'rxjs';
import { Router } from '@angular/router';

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
    private toastr: ToastrService,
    private router: Router) { 
    this.originalUrl = ''; 
  }

  ngOnInit(): void {
    this.urlForm = this.fb.group({
      originalUrl: ['', [Validators.required, Validators.pattern('https?://.+')]]
    });

     // Subscribe to value changes of the originalUrl control to clear error message
     this.urlForm.get('originalUrl')?.valueChanges.subscribe(() => {
      this.errorMessage = null;
    });
  }

  onSubmit() {
    if (this.urlForm.valid) {
      const originalUrl = this.urlForm.value['originalUrl'];
      this.checkUrlExists(originalUrl);
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
        setTimeout(() => {
          this.router.navigate(['/shortUrl']);
        }, 2000);
      },
      error => {
        // Handle error response
        console.error("Error creating short URL:", error);
        this.toastr.error('Error creating short URL. Please try again later.', 'Error');
        if (error.status === 400) {
           // Display error message for URL already exists
           this.errorMessage = error.error.errors[0].errorMessage;
        } else {
          // Handle other types of errors (e.g., server errors)
          this.toastr.error('Error creating short URL. Please try again later.', 'Error');
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
checkUrlExists(originalUrl: string) {
  this.urlShortenerService.checkUrlExists(originalUrl).subscribe(
    (exists: boolean) => {
      if (exists) {
        this.errorMessage = 'URL already exists.';
      } else {
        this.createShortUrl(originalUrl);
      }
    },
    (error) => {
      console.error("Error checking if URL exists:", error);
      this.toastr.error('Error checking if URL exists. Please try again later.', 'Error');
    }
  );
}
}