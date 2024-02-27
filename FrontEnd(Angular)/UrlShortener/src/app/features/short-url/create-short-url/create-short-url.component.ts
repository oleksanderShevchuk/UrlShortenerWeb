import { Component } from '@angular/core';
import { UrlShortenerService } from '../../../services/url-shortener.service';
import { NgModel } from '@angular/forms';

@Component({
  selector: 'app-create-short-url',
  templateUrl: './create-short-url.component.html',
  styleUrls: ['./create-short-url.component.scss'] 
})
export class CreateShortUrlComponent {
  originalUrl!: string;

  constructor(private urlShortenerService: UrlShortenerService) { 
    this.originalUrl = ''; 
  }

  onSubmit(): void {
    debugger
    this.urlShortenerService.create(this.originalUrl).subscribe(
      response => {
        console.log('Short URL created:', response);
        // Handle successful creation, if needed
      },
      error => {
        console.error('Error creating short URL:', error);
        // Handle error, if needed
      }
    );
  }

  
}
