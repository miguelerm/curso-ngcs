import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { IEditBook } from '../edit/edit.component';
import ngbDateToDate from 'src/app/shared/functions/ngb-date-to-date.function';
import { BooksCatalogService } from '../../services/books-catalog.service';
import { Router, ActivatedRoute } from '@angular/router';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { NgTemplateOutlet } from '@angular/common';

import { map, filter, switchMap, tap, delay } from 'rxjs/operators';
import { isBookValid } from '../../functions/is-book-valid.function';

@Component({
  selector: 'abs-update',
  templateUrl: './update.component.html',
  styleUrls: ['./update.component.css']
})
export class UpdateComponent implements OnInit, OnDestroy {

  private id: number;
  public book: IEditBook = { authors: [], covers: [] };

  @ViewChild('content', { static: false})
  public template: NgTemplateOutlet;

  private dialog: NgbModalRef;

  public isValid = () => isBookValid(this.book);

  constructor(
    private readonly svc: BooksCatalogService,
    private readonly router: Router,
    private readonly route: ActivatedRoute,
    private readonly modal: NgbModal,
  ) { }

  ngOnInit() {
    this.route.params
    .pipe(
      map(params => parseInt(params.id, 10)),
      filter(id => !isNaN(id)),
      switchMap(id => this.svc.single(id)),
      tap(book => {
        this.id = book.id;
        const date = new Date(book.publishedOn);
        this.book = {
          ...book,
          covers: book.covers.map(c => ({...c, src: '/api/files/' + c.code + '?size=small'})),
          publishedOn: { year: date.getFullYear(), month: date.getMonth() + 1, day: date.getDate() },
        };
      }),
    )
    .subscribe(() => {
      const dialog = this.modal.open(this.template, { centered: true, size: 'lg'});
      this.dialog = dialog;
      dialog.result.then(() => this.back(), () => this.back());
    });
  }

  public ngOnDestroy() {
    const dialog = this.dialog;
    if (dialog) {
      dialog.dismiss();
    }
  }

  public save() {

    const book = this.book;
    const data = {
      id: this.id,
      title: book.title,
      description: book.description,
      publishedOn: ngbDateToDate(book.publishedOn),
      authors: book.authors,
      covers: book.covers.map(({code}) => ({code}))
    };

    this.svc.put(data).subscribe(
      () => this.back(),
      (...args) => console.log('error!!!!', args)
    );
  }

  private back() {
    this.router.navigate(['../../'], { relativeTo: this.route });
  }

}
