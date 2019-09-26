import { Component, Input } from '@angular/core';

@Component({
  selector: 'abs-catalog-page',
  templateUrl: './catalog-page.component.html',
  styleUrls: ['./catalog-page.component.css']
})
export class CatalogPageComponent {

  @Input()
  public title: string;

  @Input()
  public createPermissions: string[];

}
