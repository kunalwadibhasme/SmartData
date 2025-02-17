import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DisplaycartproductsComponent } from './displaycartproducts.component';

describe('DisplaycartproductsComponent', () => {
  let component: DisplaycartproductsComponent;
  let fixture: ComponentFixture<DisplaycartproductsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DisplaycartproductsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DisplaycartproductsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
