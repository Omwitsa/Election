import { HttpService } from './../../services/http.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  user = {
    Username: "gjggk",
    Password: "67789",
    ConfirmPassword: "5678"
  };
  constructor(private httpService: HttpService) { }

  ngOnInit() {
  }

  onSubmit(){
    this.httpService.registerUsers(this.user).subscribe(result => {
      //this.users = result.data;
    });
  }

}
