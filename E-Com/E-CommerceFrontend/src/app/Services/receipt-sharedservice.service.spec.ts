import { TestBed } from '@angular/core/testing';

import { ReceiptSharedserviceService } from './receipt-sharedservice.service';

describe('ReceiptSharedserviceService', () => {
  let service: ReceiptSharedserviceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ReceiptSharedserviceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
