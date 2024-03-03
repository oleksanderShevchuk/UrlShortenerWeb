import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { DescriptionEditDto } from '../../models/description/description-edit-dto.model';
import { Description } from '../../models/description/description.model';

@Injectable({
  providedIn: 'root'
})
export class DescriptionService {
  constructor(private http: HttpClient) { }

  getDescription(id: number): Observable<Description> {
    return this.http.get<Description>(`${environment.endpoint}/api/Home/description/${id}`);
  }

  editDescription(descriptionDto: DescriptionEditDto): Observable<void> {
    return this.http.put<void>(`${environment.endpoint}/api/Admin/description/edit`, descriptionDto);
  }
}
