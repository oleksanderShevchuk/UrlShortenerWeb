import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShortUrlInfoComponent } from './short-url-info.component';

describe('ShortUrlInfoComponent', () => {
  let component: ShortUrlInfoComponent;
  let fixture: ComponentFixture<ShortUrlInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ShortUrlInfoComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ShortUrlInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
