import { NgFor, NgIf } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { User } from '../../models/user.model';
import { SearchUser } from '../../models/search-user.model';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-user',
  standalone: true,
  imports: [NgIf, NgFor, FormsModule],
  templateUrl: './user.component.html',
  styleUrl: './user.component.css'
})
export class UserComponent implements OnInit {
  users: User[] = [];

  searchParams = new SearchUser();

  constructor(private router: Router, private userService: UserService) { }

  ngOnInit(): void {
    this.searchUsers();
  }

  searchUsers() {
    this.userService.searchUsers(this.searchParams).subscribe(
      data => {
        this.users = data;
      },
      error => {
        console.log('Error searching companies:', error);
      }
    );
  }

  createUser(): void {
    this.router.navigate(['/user-detail']);
  }
}
