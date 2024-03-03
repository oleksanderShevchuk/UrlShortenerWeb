import { Component, Input, OnInit } from '@angular/core';
import { UrlShortenerService } from '../../../services/url-shortener/url-shortener.service';
import { ShortUrl } from '../../../models/short-url/short-url.model';
import { AccountService } from '../../../account/account.service';
import { AdminService } from '../../../services/admin/admin.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-short-url-list',
  templateUrl: './short-url-list.component.html',
  styleUrl: './short-url-list.component.scss'
})
export class ShortUrlListComponent implements OnInit{
  shortUrls: ShortUrl[] = [];
  newUrl: string = '';

  constructor(
    private urlShortenerService: UrlShortenerService,
    private accountService: AccountService,
    private adminService: AdminService,
    private router: Router,
    private toastr: ToastrService,
  ) { }

  ngOnInit(): void {
    this.fetchShortUrls();
  }

  fetchShortUrls(): void {
    this.urlShortenerService.getList().subscribe(shortUrls => {
      this.shortUrls = shortUrls;
    });
  }

  deleteUrl(id: number): void {
    const confirmed = window.confirm('Are you sure you want to delete this URL?');
    if (confirmed) {
    this.urlShortenerService.delete(id).subscribe((response) => {
        // Handle successful response
        this.toastr.success('Short URL deleted successfully!', 'Success');
        console.log("Short URL deleted successfully:", response);
        // Refresh the list of URLs after deletion
        this.fetchShortUrls();
    });
    }
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
    this.router.navigate(['/short-url-info', id]);
  }
  
  onDeleteAllUrls(): void {
    if (confirm('Are you sure you want to delete all URLs?')) {
      this.adminService.deleteAllUrls().subscribe(
        (response) => {
          console.log('All URLs deleted successfully.');
          this.toastr.success('All URLs deleted successfully!', 'Success');
          // Refresh the list of URLs after deletion
          this.fetchShortUrls();
        },
        (error) => {
          this.toastr.error('Error deleting all URLs. Please try again later.', 'Error');
          console.error('Error deleting all URLs.', error);
        }
      );
    }
  }
  isAdmin(): boolean {
    const userRole = this.accountService.getUserRole();
    return userRole === 'Admin';
  }
}