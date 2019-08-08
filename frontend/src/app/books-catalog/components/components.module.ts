import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { ListComponent } from "./list/list.component";
import { EditComponent } from "./edit/edit.component";
import {
  NgbDatepickerModule,
  NgbDateParserFormatter,
  NgbModalModule
} from "@ng-bootstrap/ng-bootstrap";
import { NgbDateCustomParserFormatter } from "./custom";
import { LayoutModule } from "src/app/layout/layout.module";
import { AuthorsListComponent } from "./authors-list/authors-list.component";
import { AuthorsEditorComponent } from "./authors-editor/authors-editor.component";
import { AuthorsFormComponent } from "./authors-form/authors-form.component";
import { RouterModule } from "@angular/router";
import { CreateComponent } from "./create/create.component";
import { UpdateComponent } from "./update/update.component";

@NgModule({
  declarations: [
    ListComponent,
    EditComponent,
    AuthorsListComponent,
    AuthorsEditorComponent,
    AuthorsFormComponent,
    CreateComponent,
    UpdateComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    NgbDatepickerModule,
    NgbModalModule,
    LayoutModule,
    RouterModule
  ],
  providers: [
    { provide: NgbDateParserFormatter, useClass: NgbDateCustomParserFormatter }
  ]
})
export class ComponentsModule {}
