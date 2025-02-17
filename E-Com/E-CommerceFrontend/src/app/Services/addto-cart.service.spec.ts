import { TestBed } from '@angular/core/testing';

import { AddtoCartService } from './addto-cart.service';

describe('AddtoCartService', () => {
  let service: AddtoCartService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AddtoCartService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
