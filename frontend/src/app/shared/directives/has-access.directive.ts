import { Directive, ElementRef, Input, OnInit, HostListener } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { HttpClient } from '@angular/common/http';

@Directive({
  selector: '[absHasAccess]'
})
export class HasAccessDirective implements OnInit {

  @Input('absHasAccess')
  public permissions: string[];

  private readonly element: HTMLElement;

  constructor(
    elementRef: ElementRef,
    private readonly auth: AuthService,
    private readonly http: HttpClient
  ) {
    this.element = elementRef.nativeElement;
    this.element.style.display = 'none';
  }

  ngOnInit() {
    const element = this.element;

    if (this.auth.user.name === 'ASD') {
      const permissions = this.permissions;
      this.http.get<boolean>('/api/authentication/has-access', { params: { permissions } })
        .subscribe(hasPermissions => {
            element.style.display = hasPermissions ? '' : 'none';
        });
    } else {
      element.style.display = 'none';
    }
  }




}
