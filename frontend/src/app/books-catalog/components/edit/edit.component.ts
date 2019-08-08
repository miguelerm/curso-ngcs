import { Component, Input } from '@angular/core';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { IAuthor } from '../authors-list/authors-list.component';

@Component({
  selector: 'abs-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.css']
})
export class EditComponent {

  @Input()
  public book: IEditBook;

  public add(author: IAuthor) {
    this.book.authors.push(author);
  }

  public remove(index: number) {
    this.book.authors.splice(index, 1);
  }

}

export interface IEditBook {
  title?: string;
  description?: string;
  publishedOn?: NgbDateStruct;
  authors: IAuthor[];
}
