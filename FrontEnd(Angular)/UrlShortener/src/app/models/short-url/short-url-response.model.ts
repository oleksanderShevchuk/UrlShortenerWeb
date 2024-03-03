export interface ShortUrlResponse {
    id: number;
    originalUrl: string;
    shortenedUrl: string;
    code: string;
    createdBy: string;
    createdDate: Date;
  }