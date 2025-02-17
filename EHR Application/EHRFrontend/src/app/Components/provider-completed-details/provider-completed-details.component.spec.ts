import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProviderCompletedDetailsComponent } from './provider-completed-details.component';

describe('ProviderCompletedDetailsComponent', () => {
  let component: ProviderCompletedDetailsComponent;
  let fixture: ComponentFixture<ProviderCompletedDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProviderCompletedDetailsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProviderCompletedDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
