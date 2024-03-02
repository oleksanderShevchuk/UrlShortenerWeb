import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ShortUrlListComponent } from './features/short-url/short-url-list/short-url-list.component';
import { CreateShortUrlComponent } from './features/short-url/create-short-url/create-short-url.component';
import { NotFoundComponent } from './shared/components/errors/not-found/not-found/not-found.component';
import { ShortUrlInfoComponent } from './features/short-url/short-url-info/short-url-info.component';
import { DescriptionComponent } from './features/description/description.component';
import { HomeComponent } from './features/home/home.component';

const routes: Routes = [
  { path: 'shortUrl', component: ShortUrlListComponent},
  { path: 'shortUrl/create', component: CreateShortUrlComponent},
  { path: 'about', component: DescriptionComponent },
  { path: 'short-url-info/:id', component: ShortUrlInfoComponent },
  { path: 'home', component: HomeComponent },
  { path: '', component: HomeComponent },
  { path: 'account', loadChildren: () => import('./account/account.module').then(module => module.AccountModule) },
  { path: 'not-found', component: NotFoundComponent },
  { path: '**', component: NotFoundComponent, pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
