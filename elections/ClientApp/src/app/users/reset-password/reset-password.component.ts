import { HttpService } from './../../services/http.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.css']
})
export class ResetPasswordComponent implements OnInit {
  user = {
    userName: "",
    oldPassword: "",
    newPassword: "",
    confirmpassword: ""
  };
  constructor(private httpService: HttpService) {}

  onSubmit() {
    debugger
    this.httpService.resetPassword(this.user)
    // this.router.navigate(['/dashboard']);
  }

  ngOnInit() {}
}
