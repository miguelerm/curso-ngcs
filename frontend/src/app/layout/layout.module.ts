import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MenuComponent } from './menu/menu.component';
import { RouterModule } from '@angular/router';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { PageComponent } from './page/page.component';
import { CatalogPageComponent } from './catalog-page/catalog-page.component';



@NgModule({
  declarations: [MenuComponent, PageComponent, CatalogPageComponent],
  imports: [
    CommonModule,
    RouterModule,
    NgbDropdownModule,
  ],
  exports: [MenuComponent, PageComponent, CatalogPageComponent]
})
export class LayoutModule { }
