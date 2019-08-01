import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ListComponent } from './list/list.component';
import { EditComponent } from './edit/edit.component';

@NgModule({
  declarations: [ListComponent, EditComponent],
  imports: [
    CommonModule,
    FormsModule,
  ]
})
export class ComponentsModule { }
