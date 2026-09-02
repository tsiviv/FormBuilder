import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../environments/environment';
import { CreateFormRequest, FormTemplate } from '../models/form.models';

@Injectable({ providedIn: 'root' })
export class FormsService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/forms`;

  createForm(request: CreateFormRequest): Observable<FormTemplate> {
    return this.http.post<FormTemplate>(this.baseUrl, request);
  }

  getForms(): Observable<FormTemplate[]> {
    return this.http.get<FormTemplate[]>(this.baseUrl);
  }

  getFormById(id: number): Observable<FormTemplate> {
    return this.http.get<FormTemplate>(`${this.baseUrl}/${id}`);
  }
}
