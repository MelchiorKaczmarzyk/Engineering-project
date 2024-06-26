import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http'
import { HomeComponent } from './home/home.component';
import { SessionList } from './session/session-list.component';
import { Navbar } from './navbar/navbar.component';
import { ClickedOutsideDirective } from './clickOutside.directive';
import { App } from './app.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { SessionCreate } from './session/session-create.component';

@NgModule({
  declarations: [
    HomeComponent,
    SessionList,
    SessionCreate,
    Navbar,
    App,
    ClickedOutsideDirective
  ],  
  imports: [  
    BrowserModule,
    FormsModule,
    HttpClientModule,
    FontAwesomeModule
  ],
  bootstrap: [
    App,
    SessionList,
    SessionCreate,
    Navbar
  ]
})
export class AppModule { }