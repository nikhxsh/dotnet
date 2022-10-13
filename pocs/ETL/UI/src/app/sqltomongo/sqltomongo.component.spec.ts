import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SqltomongoComponent } from './sqltomongo.component';

describe('SqltomongoComponent', () => {
  let component: SqltomongoComponent;
  let fixture: ComponentFixture<SqltomongoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SqltomongoComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SqltomongoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
