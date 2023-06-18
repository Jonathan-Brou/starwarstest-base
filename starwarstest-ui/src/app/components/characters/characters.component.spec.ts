import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CharactersComponent } from './characters.component';
import { HttpService } from 'src/app/services/http.service';
import { of, throwError } from 'rxjs';
import { saveAs } from 'file-saver';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { HttpClientModule } from '@angular/common/http'; // Add this line

describe('CharactersComponent', () => {
  let component: CharactersComponent;
  let fixture: ComponentFixture<CharactersComponent>;
  let httpServiceSpy: jasmine.SpyObj<HttpService>;
  let saveAsSpy: jasmine.Spy;

  beforeEach(async () => {
    const httpServiceSpyObj = jasmine.createSpyObj('HttpService', [
      'GetFilteredCharactersFromOriginalTrilogy',
      'DownloadCSVOfOriginalTrilogySorted'
    ]);

    await TestBed.configureTestingModule({
      declarations: [CharactersComponent],
      providers: [{ provide: HttpService, useValue: httpServiceSpyObj }],
      imports: [HttpClientTestingModule, HttpClientModule] // Add HttpClientModule here
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CharactersComponent);
    component = fixture.componentInstance;
    httpServiceSpy = TestBed.inject(HttpService) as jasmine.SpyObj<HttpService>;
    saveAsSpy = spyOn(saveAs, 'saveAs'); // Create a spy for saveAs function

  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  describe('toggleTable', () => {
    it('should clear the table if showTable is true', () => {
      component.showTable = true;
      spyOn(component, 'clearTable');

      component.toggleTable();

      expect(component.clearTable).toHaveBeenCalled();
    });

    it('should populate the table if showTable is false', () => {
      component.showTable = false;
      spyOn(component, 'populateTable');

      component.toggleTable();

      expect(component.populateTable).toHaveBeenCalled();
    });
  });

  describe('populateTable', () => {
    it('should populate the table with characters', () => {
      const characters = [{ name: 'Luke Skywalker' }, { name: 'Darth Vader' }];
      httpServiceSpy.GetFilteredCharactersFromOriginalTrilogy.and.returnValue(
        of(characters)
      );

      component.populateTable();

      expect(Array.from(component.characters)).toEqual(
        jasmine.arrayContaining(characters)
      );
      expect(component.buttonLabel).toBe('Clear');
      expect(component.showTable).toBe(true);
      expect(component.errorMessage).toBe('');
    });

    it('should set error message if there is an error', () => {
      const error = 'Server error';
      httpServiceSpy.GetFilteredCharactersFromOriginalTrilogy.and.returnValue(
        throwError(error)
      );

      component.populateTable();

      expect(Array.from(component.characters)).toEqual([]);
      expect(component.buttonLabel).toBe('Populate Table');
      expect(component.showTable).toBe(false);
      expect(component.errorMessage).toBe(
        'Server is not responding. Please try again later. This can happen if the server has not fully loaded or is overloaded. Sorry for any inconvience.' 
        );
    });
  });

  describe('clearTable', () => {
    it('should clear the table', () => {
      component.characters = [
        {
          name: 'Luke Skywalker',
          planet: 'Tatooine',
          films: ['A New Hope', 'The Empire Strikes Back']
        },
        {
          name: 'Darth Vader',
          planet: 'Mustafar',
          films: ['A New Hope', 'The Empire Strikes Back', 'Return of the Jedi']
        }
      ];

      component.clearTable();

      expect(component.characters).toEqual([]);
      expect(component.buttonLabel).toBe('Populate Table');
      expect(component.showTable).toBe(false);
      expect(component.errorMessage).toBe('');
    });
  });

  describe('downloadCSV', () => {
    it('should download the CSV file', () => {
      const blob = new Blob(['CSV data'], { type: 'text/csv' });
      httpServiceSpy.DownloadCSVOfOriginalTrilogySorted.and.returnValue(of(blob));

      component.downloadCSV();

      expect(saveAsSpy).toHaveBeenCalledWith(blob, 'original_trilogy_characters.csv'); // Use the spy for saveAs function
      expect(component.errorMessage).toBe('');
    });

    it('should set error message if there is an error', () => {
      const error = 'Download error';
      httpServiceSpy.DownloadCSVOfOriginalTrilogySorted.and.returnValue(throwError(error));

      component.downloadCSV();

      expect(component.errorMessage).toBe(
        'Server is not responding. Please try again later. This can happen if the server has not fully loaded or is overloaded. Sorry for any inconvience.' 
      );
    });
  });
});
