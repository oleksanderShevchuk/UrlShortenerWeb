import { ActivatedRoute } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { ShortUrl } from '../../../models/short-url.model';
import { UrlShortenerService } from '../../../services/url-shortener/url-shortener.service';

@Component({
  selector: 'app-short-url-info',
  templateUrl: './short-url-info.component.html',
  styleUrls: ['./short-url-info.component.scss']
})
export class ShortUrlInfoComponent implements OnInit {
  
  shortUrl!: ShortUrl;

  constructor(
    private route: ActivatedRoute,
    private urlShortenerService: UrlShortenerService
  ) { }

  ngOnInit(): void {
    this.urlInfo();
  }

  urlInfo(): void {
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id !== null) {
        const numericId = +id;
        if (!isNaN(numericId)) {
          this.urlShortenerService.getShortUrlById(numericId).subscribe(shortUrl => {
            this.shortUrl = shortUrl;
          });
        } else {
          console.error('Invalid ID parameter:', id);
          // Handle the case when 'id' is not a valid number
          // For example, display an error message to the user
        }
      } else {
        console.error('ID parameter is null');
        // Handle the case when 'id' is null
        // For example, redirect the user to a default page or display a friendly message
      }
    });
  }
  // Public method to fetch ShortUrl details
  fetchShortUrlDetails(id: number): void {
    this.urlShortenerService.getShortUrlById(id).subscribe(shortUrl => {
      this.shortUrl = shortUrl;
    });
  }
}
