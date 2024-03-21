import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SqltopostgreComponent } from './sqltopostgre.component';

describe('SqltopostgreComponent', () => {
  let component: SqltopostgreComponent;
  let fixture: ComponentFixture<SqltopostgreComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SqltopostgreComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SqltopostgreComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
