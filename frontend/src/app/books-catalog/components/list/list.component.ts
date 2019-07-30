import { Component, OnInit } from '@angular/core';
import { BooksCatalogService, IBookSummary } from '../../services/books-catalog.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'abs-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.css']
})
export class ListComponent implements OnInit {
  books: Observable<IBookSummary[]>;

  constructor(private svc: BooksCatalogService) { }

  ngOnInit() {
    this.books = this.svc.get();
  }

}
