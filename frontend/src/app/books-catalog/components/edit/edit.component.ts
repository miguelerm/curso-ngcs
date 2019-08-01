import { Component, OnInit } from '@angular/core';
import { BooksCatalogService } from '../../services/books-catalog.service';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'abs-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.css']
})
export class EditComponent implements OnInit {

  public title: string;
  public description: string;
  public publishedOn: Date;
  public author: string;
  public authors: string[] = [];

  public get isValid() {
    return this.title && this.description;
  }

  constructor(private svc: BooksCatalogService, private router: Router, private route: ActivatedRoute) { }

  ngOnInit() {
  }

  public add() {
    this.authors.push(this.author);
    this.author = '';
  }

  public remove(index: number) {
    this.authors.splice(index, 1);
  }

  public save() {
    const data = {
      title: this.title,
      description: this.description,
      publishedOn: this.publishedOn,
      authors: this.authors.map(name => ({ name })),
    };
    this.svc.post(data).subscribe(
      () => this.router.navigate(['../list'], { relativeTo: this.route }),
      (...args) => console.log('error!!!!', args)
    );
    console.log(JSON.stringify(data));
  }

}
