import { Component } from '@angular/core';
import { CompanyService } from '../../services/company.service';
import { Router } from '@angular/router';
import { CompanyRequest } from '../../models/company-request.model';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-company-detail',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './company-detail.component.html',
  styleUrl: './company-detail.component.css'
})
export class CompanyDetailComponent {
  company = new CompanyRequest();
  
  constructor(private router: Router, private companyService: CompanyService) { }
  
  createCompany(): void {
    this.companyService.createCompany(this.company).subscribe(
      data => {
        this.router.navigate(['/companies']);
      },
      error => {
        console.log(error);
      }
    );
  }
}
