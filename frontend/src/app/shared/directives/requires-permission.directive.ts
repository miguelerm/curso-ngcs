import { Directive, TemplateRef, ViewContainerRef, OnInit, Input } from '@angular/core';
import { AuthService } from '../services/auth.service';


@Directive({
  selector: '[absRequiresPermission]'
})
export class RequiresPermissionDirective implements OnInit {

  @Input('absRequiresPermission')
  public permission: string;

  constructor(
    private readonly templateRef: TemplateRef<any>,
    private readonly viewContainerRef: ViewContainerRef,
    private readonly auth: AuthService
  ) { }

  ngOnInit() {
    if (this.auth.hasPermission(this.permission)) {
      this.viewContainerRef.createEmbeddedView(this.templateRef);
    } else {
      this.viewContainerRef.clear();
    }
  }

}
