import { TestBed } from '@angular/core/testing';

import { BooksCatalogService } from './books-catalog.service';

describe('BooksCatalogService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: BooksCatalogService = TestBed.get(BooksCatalogService);
    expect(service).toBeTruthy();
  });
});
