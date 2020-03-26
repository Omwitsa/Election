import { HttpService } from './../../services/http.service';
import { Component, OnInit, Inject } from '@angular/core';

@Component({
  selector: 'app-list-users',
  templateUrl: './list-users.component.html',
  styleUrls: ['./list-users.component.css']
})
export class ListUsersComponent implements OnInit {
  users = [];
  constructor(private httpService: HttpService, @Inject('BASE_URL') baseUrl: string) { 
    this.getUsers();
  }

  ngOnInit() {
    
  }

  private getUsers(){
    this.httpService.getUsers().subscribe(result => {
      this.users = result.data;
    });
  }
}
