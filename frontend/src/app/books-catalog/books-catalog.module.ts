import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { BooksCatalogRoutingModule } from './books-catalog-routing.module';
import { BooksCatalogComponent } from './books-catalog.component';
import { ComponentsModule } from './components/components.module';
import { ServicesModule } from './services/services.module';
import { WithCredentialsInterceptor } from '../shared/interceptors/with-credentials.interceptor';
import { HTTP_INTERCEPTORS } from '@angular/common/http';

@NgModule({
  declarations: [BooksCatalogComponent],
  imports: [
    CommonModule,
    BooksCatalogRoutingModule,
    ComponentsModule,
    ServicesModule,
  ],
  providers: [
    ,
    { provide: HTTP_INTERCEPTORS, useClass: WithCredentialsInterceptor, multi: true }
  ]
})
export class BooksCatalogModule { }
