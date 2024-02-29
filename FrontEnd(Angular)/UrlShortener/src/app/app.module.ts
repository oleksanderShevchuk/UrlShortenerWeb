import { NgModule } from '@angular/core';
import { BrowserModule, provideClientHydration } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { ShortUrlListComponent } from './features/short-url/short-url-list/short-url-list.component';
import { CreateShortUrlComponent } from './features/short-url/create-short-url/create-short-url.component';
import { CardModule } from 'primeng/card';
import { InputTextModule } from 'primeng/inputtext';
import { ReactiveFormsModule } from '@angular/forms';
import { ButtonModule} from 'primeng/button';
import { FooterComponent } from './components/footer/footer.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { SharedModule } from './shared/shared.module';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { JwtInterceptor } from './shared/interceptors/jwt.interceptor';
import { ToastrModule } from 'ngx-toastr';


@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    ShortUrlListComponent,
    CreateShortUrlComponent,
    FooterComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    CardModule,
    InputTextModule,
    ReactiveFormsModule,
    ButtonModule,
    SharedModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot()
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
