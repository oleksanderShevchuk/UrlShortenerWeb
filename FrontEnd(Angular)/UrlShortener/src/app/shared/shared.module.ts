import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { ModalModule } from 'ngx-bootstrap/modal';
import { NotificationComponent } from './components/modals/notification/notification.component';
import { NotFoundComponent } from './components/errors/not-found/not-found/not-found.component';
import { ValidationMessagesComponent } from './components/errors/validation-messages/validation-messages/validation-messages.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

@NgModule({
  declarations: [
    NotFoundComponent,
    ValidationMessagesComponent,
    NotificationComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    HttpClientModule, 
    ModalModule.forRoot(),
  ],
  exports: [
    RouterModule,
    ReactiveFormsModule,
    ValidationMessagesComponent,
  ]
})
export class SharedModule { }
