import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MenuComponent } from './menu/menu.component';
import { RouterModule } from '@angular/router';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { PageComponent } from './page/page.component';
import { CatalogPageComponent } from './catalog-page/catalog-page.component';
import { CreatePageComponent } from './create-page/create-page.component';
import { FormsModule } from '@angular/forms';



@NgModule({
  declarations: [MenuComponent, PageComponent, CatalogPageComponent, CreatePageComponent],
  imports: [
    CommonModule,
    RouterModule,
    NgbDropdownModule,
    FormsModule,
  ],
  exports: [MenuComponent, PageComponent, CatalogPageComponent, CreatePageComponent]
})
export class LayoutModule { }
