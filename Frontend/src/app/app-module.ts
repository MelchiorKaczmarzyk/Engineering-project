import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http'
import { SessionList } from './components/session-list/session-list.component';
import { ClickedOutsideDirective } from './directives/click-outside.directive';
import { App } from './app.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
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
import {MatSelectModule} from '@angular/material/select'
import { MySessionsGm } from './components/my-sessions-gm/my-sessions-gm.component';
import { MySessionsPlayer } from './components/my-sessions-player/my-sessions-player.components';
import { SessionCreate } from './components/session-create/session-create.component';
import { AccountDetails } from './components/account-details/account-details.component';
import { LoginRegister } from './components/login-register/login-register.component';
import {MatPaginatorModule} from '@angular/material/paginator'; 
import {MatRadioModule} from '@angular/material/radio'; 
import {MatDatepickerModule} from '@angular/material/datepicker'
import { MatNativeDateModule } from '@angular/material/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { SessionParameters } from './components/session-parameters/session-parameters.component';
import { MenageUsers } from './components/manage-users/manage-users.component';


@NgModule({
  declarations: [
    SessionList,
    SessionCreate,
    LoginRegister,
    App,
    MySessionsGm,
    MySessionsPlayer,
    AccountDetails,
    ClickedOutsideDirective,
    SessionParameters,
    MenageUsers
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
    MatSelectModule,
    MatPaginatorModule,
    MatRadioModule,
    MatDatepickerModule,
    MatNativeDateModule,
    FlexLayoutModule,
  ],
  bootstrap: [
    App,
    SessionList,
    SessionCreate,
    LoginRegister,
  ],
  providers: [
    provideAnimationsAsync(),
    MatDatepickerModule
  ]
})
export class AppModule { }