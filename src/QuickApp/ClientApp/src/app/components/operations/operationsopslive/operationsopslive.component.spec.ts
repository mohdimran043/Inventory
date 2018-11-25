import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OperationsopsliveComponent } from './operationsopslive.component';

describe('OperationsopsliveComponent', () => {
  let component: OperationsopsliveComponent;
  let fixture: ComponentFixture<OperationsopsliveComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OperationsopsliveComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OperationsopsliveComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
