import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MenuComponent } from './menu/menu.component';
import { RouterModule } from '@angular/router';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { PageComponent } from './page/page.component';
import { CatalogPageComponent } from './catalog-page/catalog-page.component';
import { CreatePageComponent } from './create-page/create-page.component';
import { FormsModule } from '@angular/forms';
import { EditModalComponent } from './edit-modal/edit-modal.component';
import { SharedModule } from '../shared/shared.module';



@NgModule({
  declarations: [MenuComponent, PageComponent, CatalogPageComponent, CreatePageComponent, EditModalComponent],
  imports: [
    CommonModule,
    RouterModule,
    NgbDropdownModule,
    FormsModule,
    SharedModule,
  ],
  exports: [MenuComponent, PageComponent, CatalogPageComponent, CreatePageComponent, EditModalComponent]
})
export class LayoutModule { }
