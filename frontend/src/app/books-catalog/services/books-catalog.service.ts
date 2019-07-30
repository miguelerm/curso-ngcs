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
}

export interface IBookSummary {
  id: number;
  title: string;
}
