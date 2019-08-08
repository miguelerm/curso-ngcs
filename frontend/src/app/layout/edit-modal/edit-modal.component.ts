import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'abs-edit-modal',
  templateUrl: './edit-modal.component.html',
  styleUrls: ['./edit-modal.component.css']
})
export class EditModalComponent {

  @Input()
  public title: string;

  @Input()
  public isValid: boolean;

  @Output()
  public save = new EventEmitter();

}
