import { HttpService } from './../../services/http.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  user = {
    userName: "",
    password: ""
  };
  showErrorMessage=false;
  constructor(private httpService: HttpService, private router: Router) { }

  onSubmit() {
    // this.httpService.login(this.user)
    this.router.navigate(['/dashboard']);
  }

  ngOnInit() {
    
  }
  
  authenticateUser() {
    this.httpService.login(this.user.userName)
      .subscribe((result) => {
        if (result) {
          // this.router.navigate(['/dashboard']);
        } else {
          this.showErrorMessage = true;
        }
      });
  }
}

