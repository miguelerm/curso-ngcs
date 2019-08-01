import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ListComponent } from './list/list.component';
import { EditComponent } from './edit/edit.component';
import { NgbDatepickerModule, NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { NgbDateCustomParserFormatter } from './custom';

@NgModule({
  declarations: [ListComponent, EditComponent],
  imports: [
    CommonModule,
    FormsModule,
    NgbDatepickerModule,
  ],
  providers: [
    {provide: NgbDateParserFormatter, useClass: NgbDateCustomParserFormatter}
  ]
})
export class ComponentsModule { }
