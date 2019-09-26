import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NotFoundComponent } from './not-found/not-found.component';
import { HasAccessDirective } from './directives/has-access.directive';
import { RequiresPermissionDirective } from './directives/requires-permission.directive';



@NgModule({
  declarations: [NotFoundComponent, HasAccessDirective, RequiresPermissionDirective],
  imports: [
    CommonModule
  ],
  exports: [HasAccessDirective, RequiresPermissionDirective]
})
export class SharedModule { }
