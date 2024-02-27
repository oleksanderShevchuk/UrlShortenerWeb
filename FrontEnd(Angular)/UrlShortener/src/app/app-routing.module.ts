import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ShortUrlListComponent } from './features/short-url/short-url-list/short-url-list.component';
import { CreateShortUrlComponent } from './features/short-url/create-short-url/create-short-url.component';

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
  // {
  //   path: '',
  //   runGuardsAndResolvers: 'always',
  //   canActivate: [AuthorizationGuard],
  //   children: [
  //     { path: 'play', component: PlayComponent },
  //     { path: 'admin', loadChildren: () => import('./admin/admin.module').then(module => module.AdminModule) },
  //   ]
  // },
  // Implementing lazy loading by the following format
  { path: 'account', loadChildren: () => import('./account/account.module').then(module => module.AccountModule) },
  // { path: 'not-found', component: NotFoundComponent },
  // { path: '**', component: NotFoundComponent, pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
