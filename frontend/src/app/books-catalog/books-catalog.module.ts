import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { BooksCatalogRoutingModule } from './books-catalog-routing.module';
import { BooksCatalogComponent } from './books-catalog.component';
import { ComponentsModule } from './components/components.module';
import { ServicesModule } from './services/services.module';


@NgModule({
  declarations: [BooksCatalogComponent],
  imports: [
    CommonModule,
    BooksCatalogRoutingModule,
    ComponentsModule,
    ServicesModule,
  ]
})
export class BooksCatalogModule { }
