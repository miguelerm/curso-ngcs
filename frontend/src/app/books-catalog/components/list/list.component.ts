import { Component, OnInit, EventEmitter, AfterViewChecked } from '@angular/core';
import { BooksCatalogService, IBookSummary } from '../../services/books-catalog.service';
import { Observable, fromEvent, merge } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { map, switchMap, debounce, debounceTime, distinctUntilChanged, mergeMap, startWith } from 'rxjs/operators';

@Component({
  selector: 'abs-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.css']
})
export class ListComponent implements OnInit {

  public readonly refresh = new EventEmitter<string>();
  books: Observable<IBookSummary[]>;
  form: FormGroup;

  constructor(private svc: BooksCatalogService, fb: FormBuilder) {
    this.form = fb.group({
      criteria: [null, Validators.required]
    });

  }

  ngOnInit() {

    const criteriaChanges = this.form.valueChanges
    .pipe(
      map((v: IFormValue) => v.criteria),
      startWith(''),
      debounceTime(500),
      distinctUntilChanged()
    );

    const refresh = this.refresh
      .pipe(
        map(() => this.form.get('criteria').value as string)
      );

    this.books = merge(criteriaChanges, refresh)
      .pipe(
        mergeMap(criteria => this.svc.get(criteria)),
      );
  }

}

interface IFormValue {
  criteria: string;
}
