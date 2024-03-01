import { Component, Input, OnInit } from '@angular/core';
import { UrlShortenerService } from '../../../services/url-shortener/url-shortener.service';
import { ShortUrl } from '../../../models/short-url.model';
import { Router } from '@angular/router';
import { AccountService } from '../../../account/account.service';
import { environment } from '../../../../environments/environment';
import { catchError, of } from 'rxjs';
import { ShortUrlInfoComponent } from '../short-url-info/short-url-info.component';
import { AdminService } from '../../../services/admin/admin.service';

@Component({
  selector: 'app-short-url-list',
  templateUrl: './short-url-list.component.html',
  styleUrl: './short-url-list.component.scss'
})
export class ShortUrlListComponent implements OnInit{
  shortUrls: ShortUrl[] = [];
  newUrl: string = '';
  urlExistsError: boolean = false;
  allUrlsDeletedMessageVisible: boolean = false;
  @Input()
  public requiredRoles: string[] = []; 

  constructor(
    private urlShortenerService: UrlShortenerService,
    private router: Router,
    private accountService: AccountService,
    private adminService: AdminService,
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
  deleteUrl(id: number): void {
    // Implement logic to delete the URL
    // Example: Call a method in the UrlShortenerService to delete the URL
    this.urlShortenerService.delete(id).subscribe(() => {
      // Refresh the list of URLs after deletion
      this.fetchShortUrls();
    });
  }

  isAuthorized(): boolean {
    return this.accountService.isAuthenticatedUser();
  }

  canDelete(shortUrl: ShortUrl): boolean {
    const userId = this.accountService.getUserId();
    const userRole = this.accountService.getUserRole();
    return (
      userId !== null &&
      (shortUrl.createdBy === userId || userRole === 'Admin')
    );
  }

  infoUrl(id: number): void {
    debugger
    //this.shortUrlInfo.fetchShortUrlDetails(id);
  }
  
  onDeleteAllUrls(): void {
    if (confirm('Are you sure you want to delete all URLs?')) {
      this.adminService.deleteAllUrls().subscribe(
        () => {
          console.log('All URLs deleted successfully.');
          this.allUrlsDeletedMessageVisible = true; // Show the success message
          // Optionally, refresh the table or perform any other action
          setTimeout(() => {
            this.allUrlsDeletedMessageVisible = false; // Hide the success message after some time
          }, 3000); // Adjust the time (in milliseconds) as needed
        },
        (error) => {
          console.error('Error deleting all URLs:', error);
        }
      );
    }
  }
  isAdmin(): boolean {
    const userRole = this.accountService.getUserRole();
    return userRole === 'Admin';
  }
}