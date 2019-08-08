import { Component, OnInit } from '@angular/core';
import { BooksCatalogService } from '../../services/books-catalog.service';
import { Router, ActivatedRoute } from '@angular/router';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { IAuthor } from '../authors-list/authors-list.component';
import ngbDateToDate from 'src/app/shared/functions/ngb-date-to-date.function';

@Component({
  selector: 'abs-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.css']
})
export class EditComponent {

  public title: string;
  public description: string;
  public publishedOn: NgbDateStruct;
  public authors: IAuthor[] = [];

  public get isValid() {
    return this.title && this.description;
  }

  constructor(private svc: BooksCatalogService, private router: Router, private route: ActivatedRoute) { }

  public add(author: IAuthor) {
    this.authors.push(author);
  }

  public remove(index: number) {
    this.authors.splice(index, 1);
  }

  public save() {

    const data = {
      title: this.title,
      description: this.description,
      publishedOn: ngbDateToDate(this.publishedOn),
      authors: this.authors,
    };

    this.svc.post(data).subscribe(
      () => this.router.navigate(['../'], { relativeTo: this.route }),
      (...args) => console.log('error!!!!', args)
    );

  }

}
