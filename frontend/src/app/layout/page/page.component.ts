import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'abs-page',
  templateUrl: './page.component.html',
  styleUrls: ['./page.component.css']
})
export class PageComponent {

  @Input()
  public title: string;

}
