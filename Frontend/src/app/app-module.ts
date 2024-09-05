import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http'
import { HomeComponent } from './home/home.component';
import { SessionList } from './session/session-list.component';
import { Navbar } from './navbar/navbar.component';
import { ClickedOutsideDirective } from './clickOutside.directive';
import { App } from './app.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { SessionCreate } from './session/session-create.component';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatButtonModule } from '@angular/material/button';
import { MatToolbarModule } from '@angular/material/toolbar';
import {MatTableModule} from '@angular/material/table'; 
import {MatMenuModule} from '@angular/material/menu';
import {MatIconModule} from '@angular/material/icon'; 
import {MatInputModule} from '@angular/material/input'; 
import {MatAutocompleteModule, MatAutocompleteTrigger} from '@angular/material/autocomplete'; 
import {MatDividerModule} from '@angular/material/divider'; 
import {MatCardModule} from '@angular/material/card'
import { LoginRegister } from './login-register.component';
import {MatSelectModule} from '@angular/material/select'
import { MySessionsGm } from './components/my-sessions-gm/my-sessions-gm.component';
import { MySessionsPlayer } from './components/my-sessions-player/my-sessions-player.components';
import { AccountDetails } from './account-details.component';

@NgModule({
  declarations: [
    HomeComponent,
    SessionList,
    SessionCreate,
    LoginRegister,
    Navbar,
    App,
    MySessionsGm,
    MySessionsPlayer,
    AccountDetails,
    ClickedOutsideDirective
  ],  
  imports: [  
    BrowserModule,
    FormsModule,
    HttpClientModule,
    FontAwesomeModule,
    MatSlideToggleModule,
    MatButtonModule,
    MatToolbarModule,
    MatTableModule,
    MatMenuModule,
    MatIconModule,
    MatInputModule,
    MatAutocompleteModule,
    MatAutocompleteTrigger,
    ReactiveFormsModule,
    MatDividerModule,
    MatCardModule,
    MatSelectModule
  ],
  bootstrap: [
    App,
    SessionList,
    SessionCreate,
    LoginRegister,
    Navbar
  ],
  providers: [
    provideAnimationsAsync()
  ]
})
export class AppModule { }