import { Component, EventEmitter, Output, ViewChild, ElementRef, Input } from '@angular/core';
import { IAuthor } from '../authors-list/authors-list.component';

@Component({
  selector: 'abs-authors-form',
  templateUrl: './authors-form.component.html',
  styleUrls: ['./authors-form.component.css']
})
export class AuthorsFormComponent {

  public name: string;

  @Input()
  public authors: IAuthor[];

  @Output()
  public enter = new EventEmitter<IAuthor>();

  @ViewChild('nameInput', { static: false })
  public input: ElementRef;

  public add() {
    const name = this.name;
    const author = { name };
    this.enter.emit(author);
    this.name = '';
    const input = this.input.nativeElement as HTMLInputElement;
    input.focus();
  }

  public isUnique() {
    const authors = this.authors;
    const name = this.name;

    return !authors || !authors.length || authors.every(x => x.name !== name);
  }

}
