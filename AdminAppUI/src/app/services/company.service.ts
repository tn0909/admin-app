// company.service.ts

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CompanyRequest } from '../models/company-request.model';
import { SearchCompany } from '../models/search-company.model';
import { CompanyResponse } from '../models/company-response.model';

@Injectable({
  providedIn: 'root'
})
export class CompanyService {
  private apiUrl = 'http://localhost:5076/api/companies'; // Update with your API URL

  constructor(private http: HttpClient) { }

  getCompany(id: number): Observable<CompanyRequest> {
    return this.http.get<CompanyRequest>(`${this.apiUrl}/${id}`);
  }

  createCompany(company: CompanyRequest): Observable<CompanyRequest> {
    return this.http.post<CompanyRequest>(this.apiUrl, company);
  }

  searchCompanies(searchParams: SearchCompany): Observable<CompanyResponse[]> {
    return this.http.post<CompanyResponse[]>(`${this.apiUrl}/search`, searchParams);
  }
}
