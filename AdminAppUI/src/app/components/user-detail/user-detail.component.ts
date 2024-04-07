import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { User } from '../../models/user.model';
import { CompanyResponse } from '../../models/company-response.model';
import { UserService } from '../../services/user.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CompanyService } from '../../services/company.service';
import { SearchCompany } from '../../models/search-company.model';
import { NgFor } from '@angular/common';

@Component({
  selector: 'app-user-detail',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule, NgFor],
  templateUrl: './user-detail.component.html',
  styleUrl: './user-detail.component.css'
})
export class UserDetailComponent implements OnInit {
  user = new User();
  companies: CompanyResponse[] = [];

  constructor(
    private router: Router,
    private userService: UserService,
    private companyService: CompanyService) { }

  ngOnInit(): void {
    this.loadCompanies();
  }

  createUser(): void {
    this.userService.createUser(this.user).subscribe(
      data => {
        this.router.navigate(['/users']);
      },
      error => {
        console.log(error);
      }
    );
  }

  displayCompany(company?: any): string | undefined {
    return company ? company.name : undefined;
  }

  loadCompanies() {
    this.companyService.searchCompanies(new SearchCompany()).subscribe(
      data => {
        this.companies = data;
      },
      error => {
        console.log('Error searching companies:', error);
      }
    );
  }
}
