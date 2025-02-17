import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompletedappointmentComponent } from './completedappointment.component';

describe('CompletedappointmentComponent', () => {
  let component: CompletedappointmentComponent;
  let fixture: ComponentFixture<CompletedappointmentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CompletedappointmentComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CompletedappointmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
