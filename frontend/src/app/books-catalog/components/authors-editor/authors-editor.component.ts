import { Component, EventEmitter, Output, Input } from '@angular/core';
import { IAuthor } from '../authors-list/authors-list.component';

@Component({
  selector: 'abs-authors-editor',
  templateUrl: './authors-editor.component.html',
  styleUrls: ['./authors-editor.component.css']
})
export class AuthorsEditorComponent {

  @Input()
  public authors: IAuthor[];

  @Output()
  public add = new EventEmitter<string>();

  @Output()
  public remove = new EventEmitter<number>();

}
