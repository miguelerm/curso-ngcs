import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { NotFoundComponent } from './shared/not-found/not-found.component';

const routes: Routes = [
  { path: 'books', loadChildren: () => import('./books-catalog/books-catalog.module').then(m => m.BooksCatalogModule) },
  { path: '404', component: NotFoundComponent },
  { path: '', pathMatch: 'full', redirectTo: '/books' },
  { path: '**', redirectTo: '/404'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
