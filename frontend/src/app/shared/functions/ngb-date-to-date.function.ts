import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';

export default function ngbDateToDate(ngbDate: NgbDateStruct): Date | undefined {
  if (!ngbDate) {
    return;
  }

  return new Date(ngbDate.year, ngbDate.month - 1, ngbDate.day);
}
