import { Component, OnInit } from '@angular/core';
import { UrlShortenerService } from '../../../services/url-shortener/url-shortener.service';
import { ShortUrl } from '../../../models/short-url.model';
import { Router } from '@angular/router';
import { AccountService } from '../../../account/account.service';

@Component({
  selector: 'app-short-url-list',
  templateUrl: './short-url-list.component.html',
  styleUrl: './short-url-list.component.scss'
})
export class ShortUrlListComponent implements OnInit{
  shortUrls: ShortUrl[] = [];
  newUrl: string = '';
  urlExistsError: boolean = false;

  constructor(
    private urlShortenerService: UrlShortenerService,
    private router: Router,
    private accountService: AccountService,
  ) { }

  ngOnInit(): void {
    this.fetchShortUrls();
  }

  fetchShortUrls(): void {
    this.urlShortenerService.getList().subscribe(shortUrls => {
      this.shortUrls = shortUrls;
    });
  }

  deleteShortUrl(id: number): void {
    this.urlShortenerService.delete(id).subscribe(() => {
      this.shortUrls = this.shortUrls.filter(url => url.id !== id);
    }, error => {
      console.error('Error deleting short URL:', error);
    });
  }

  // Optional: Add a method to navigate to the Short URL Info view
  navigateToUrlInfo(id: number): void {
    this.router.navigate(['/url-info', id]);
  }

  isAuthorized(): boolean {
    return this.accountService.isAuthenticatedUser();
  }

  createUrl(): void {
    // Check if the URL is not empty
    if (this.newUrl.trim() === '') {
      return; // Do nothing if the URL is empty
    }

    // Send a request to the backend API to add the new URL
    this.urlShortenerService.create(this.newUrl).subscribe(
      () => {
        // Clear the input field and reset the URL exists error
        this.newUrl = '';
        this.urlExistsError = false;
        // Refresh the list of URLs after addition
        this.fetchShortUrls();
      },
      error => {
        // If an error occurs, check if it's due to URL already existing
        if (error.status === 400 && error.error && error.error.ModelState && error.error.ModelState[""]) {
          this.urlExistsError = true; // Set the URL exists error flag
        }
      }
    );
  }

  canDelete(shortUrl: ShortUrl): boolean {
    // Implement logic to check if user can delete the URL
    // Example: Check if the user is the creator of the URL
    return shortUrl.createdBy === this.accountService.getUserId();
  }

  viewDetails(id: number): void {
    // Navigate to the URL details page with the specified ID
    this.router.navigate(['/url-info', id]);
  }

  deleteUrl(id: number): void {
    // Implement logic to delete the URL
    // Example: Call a method in the UrlShortenerService to delete the URL
    this.urlShortenerService.delete(id).subscribe(() => {
      // Refresh the list of URLs after deletion
      this.fetchShortUrls();
    });
  }
}