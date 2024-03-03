import { ActivatedRoute } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { ShortUrl } from '../../../models/short-url/short-url.model';
import { UrlShortenerService } from '../../../services/url-shortener/url-shortener.service';
import { SharedService } from '../../../shared/shared.service';

@Component({
  selector: 'app-short-url-info',
  templateUrl: './short-url-info.component.html',
  styleUrls: ['./short-url-info.component.scss']
})
export class ShortUrlInfoComponent implements OnInit {
  shortUrlId: number | null = null;
  shortUrl: ShortUrl | undefined;

  constructor(
    private route: ActivatedRoute,
    private urlShortenerService: UrlShortenerService,
    private sharedService: SharedService,
  ) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id !== null) {
        const numericId = +id;
        if (!isNaN(numericId)) {
          this.shortUrlId = numericId;
          this.fetchShortUrlDetails(numericId);
        } else {
          console.error('Invalid ID parameter:', id);
          // Handle invalid ID
          this.sharedService.showNotification(false, "Error", "Invalid ID");
        }
      } else {
        console.error('ID parameter is null');
        // Handle null ID
        this.sharedService.showNotification(false, "Error", "ID is null");
      }
    });
  }

  fetchShortUrlDetails(id: number): void {
    this.urlShortenerService.getShortUrlById(id).subscribe(shortUrl => {
      this.shortUrl = shortUrl;
    });
  }
}