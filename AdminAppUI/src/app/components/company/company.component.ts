import { Component, OnInit } from '@angular/core';
import { CompanyService } from '../../services/company.service';
import { SearchCompany } from '../../models/search-company.model';
import { NgFor, NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { CompanyResponse } from '../../models/company-response.model';

@Component({
  selector: 'app-company',
  standalone: true,
  imports: [NgIf, NgFor, FormsModule],
  templateUrl: './company.component.html',
  styleUrl: './company.component.css'
})
export class CompanyComponent implements OnInit {
  companies: CompanyResponse[] = [];

  searchParams = new SearchCompany();

  constructor(private router: Router, private companyService: CompanyService) { }

  ngOnInit(): void {
    this.searchCompanies();
  }

  searchCompanies() {
    this.companyService.searchCompanies(this.searchParams).subscribe(
      data => {
        this.companies = data;
      },
      error => {
        console.log('Error searching companies:', error);
      }
    );
  }

  createCompany(): void {
    this.router.navigate(['/company-detail']);
  }

  editCompany(id: number): void {
    this.router.navigate(['/company-detail']);
  }

  deleteCompany(id: number): void {
    this.router.navigate(['/company-detail']);
  }
}
