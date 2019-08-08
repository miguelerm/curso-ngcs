import { Injectable } from '@angular/core';
import { ServicesModule } from './services.module';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: ServicesModule
})
export class BooksCatalogService {

  constructor(private http: HttpClient) { }

  public get(): Observable<IBookSummary[]> {
    return this.http.get<IBookSummary[]>('/api/books');
  }

  public single(id: number): Observable<IBookSummary> {
    return this.http.get<IBookSummary>('/api/books/' + id);
  }

  public post(data: IPostBook): Observable<IBookSummary> {
    return this.http.post<IBookSummary>('/api/books', data);
  }

  public put(data: IPutBook): Observable<IBookSummary> {
    return this.http.put<IBookSummary>('/api/books/' + data.id, data);
  }
}

export interface IBookSummary {
  id: number;
  title: string;
  description: string;
  publishedOn?: Date;
  authors: {name: string}[];
}

export interface IPostBook {
  title: string;
  description: string;
  publishedOn?: Date;
  authors: {name: string}[];
}

export interface IPutBook extends IPostBook {
  id: number;
}

