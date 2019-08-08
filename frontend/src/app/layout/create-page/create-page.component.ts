import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'abs-create-page',
  templateUrl: './create-page.component.html',
  styleUrls: ['./create-page.component.css']
})
export class CreatePageComponent {

  @Input()
  public title: string;

  @Input()
  public isValid: boolean;

  @Output()
  public save = new EventEmitter();

}
