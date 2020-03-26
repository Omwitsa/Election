import { InterceptorService } from './services/interceptor.service';
import { HttpService } from './services/http.service';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ListUsersComponent } from './users/list-users/list-users.component';
import { RegisterComponent } from './users/register/register.component';
import { ResetPasswordComponent } from './users/reset-password/reset-password.component';
import { LoginComponent } from './users/login/login.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    DashboardComponent,
    ListUsersComponent,
    RegisterComponent,
    ResetPasswordComponent,
    LoginComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    // ToastrModule.forRoot({
    //   timeOut: 1000,
    //   positionClass: 'toast-top-center',
    //   preventDuplicates: true
    // }),
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      // { path: '', component: DashboardComponent, pathMatch: 'full' },
      { path: '', component: LoginComponent, pathMatch: 'full' },
      { path: 'create-account', component: RegisterComponent },
      { path: 'users', component: ListUsersComponent },
      { path: 'dashboard', component: DashboardComponent },
      { path: 'reset-password', component: ResetPasswordComponent },
    ])
  ],
  providers: [
    HttpService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: InterceptorService,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
