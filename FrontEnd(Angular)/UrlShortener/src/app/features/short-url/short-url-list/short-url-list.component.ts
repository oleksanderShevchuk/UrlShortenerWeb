import { Component, OnInit } from '@angular/core';
import { UrlShortenerService } from '../../../services/url-shortener.service';
import { ShortUrl } from '../../../models/short-url.model';

@Component({
  selector: 'app-short-url-list',
  templateUrl: './short-url-list.component.html',
  styleUrl: './short-url-list.component.scss'
})
export class ShortUrlListComponent implements OnInit{
  shortUrls: ShortUrl[] = [];

  constructor(private urlShortenerService: UrlShortenerService) { }

  ngOnInit(): void {
    this.urlShortenerService.getList().subscribe(shortUrls => {
      this.shortUrls = shortUrls;
    });
  }
}
