import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'abs-authors-list',
  templateUrl: './authors-list.component.html',
  styleUrls: ['./authors-list.component.css']
})
export class AuthorsListComponent {

  @Input()
  public authors: IAuthor[] = [];

  @Output()
  public remove = new EventEmitter<number>();

  constructor() { }

}

export interface IAuthor {
  name: string;
}
