import { IEditBook } from '../components/edit/edit.component';

export function isBookValid(book: IEditBook) {
  return !!(book && book.title && book.description) && book.covers.every(x => !!x.code);
}
