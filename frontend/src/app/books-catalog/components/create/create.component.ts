import { Component } from '@angular/core';
import { IEditBook } from '../edit/edit.component';
import ngbDateToDate from 'src/app/shared/functions/ngb-date-to-date.function';
import { BooksCatalogService } from '../../services/books-catalog.service';
import { Router, ActivatedRoute } from '@angular/router';
import { isBookValid } from '../../functions/is-book-valid.function';
import { ICoverFile } from '../covers-editor/covers-editor.component';

@Component({
  selector: 'abs-create',
  templateUrl: './create.component.html',
  styleUrls: ['./create.component.css']
})
export class CreateComponent {

  public book: IEditBook = { authors: [], covers: [] };
  public isValid = () => isBookValid(this.book);

  constructor(private svc: BooksCatalogService, private router: Router, private route: ActivatedRoute) { }

  public save() {

    const book = this.book;
    const data = {
      title: book.title,
      description: book.description,
      publishedOn: ngbDateToDate(book.publishedOn),
      authors: book.authors,
      covers: book.covers.map(({code}) => ({ code }))
    };

    this.svc.post(data).subscribe(
      () => this.router.navigate(['../'], { relativeTo: this.route }),
      (...args) => console.log('error!!!!', args)
    );

  }

}
