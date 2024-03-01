import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ShortUrlListComponent } from './features/short-url/short-url-list/short-url-list.component';
import { CreateShortUrlComponent } from './features/short-url/create-short-url/create-short-url.component';
import { NotFoundComponent } from './shared/components/errors/not-found/not-found/not-found.component';
import { ShortUrlInfoComponent } from './features/short-url/short-url-info/short-url-info.component';

const routes: Routes = [
  {
    path: 'ShortUrl',
    component: ShortUrlListComponent
  },
  {
    path: 'ShortUrl/Create',
    component: CreateShortUrlComponent
  },
  {
    path: '', redirectTo: 'home', pathMatch: 'full'
  },
  { path: 'account', loadChildren: () => import('./account/account.module').then(module => module.AccountModule) },
  { path: 'not-found', component: NotFoundComponent },
  { path: '**', component: NotFoundComponent, pathMatch: 'full' },
  { path: 'short-url-info/:id', component: ShortUrlInfoComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
