import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TransportScheduleComponent } from './transport-schedule.component';

describe('TransportScheduleComponent', () => {
  let component: TransportScheduleComponent;
  let fixture: ComponentFixture<TransportScheduleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TransportScheduleComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TransportScheduleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
