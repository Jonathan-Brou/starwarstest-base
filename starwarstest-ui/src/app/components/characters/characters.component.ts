import { Component } from '@angular/core';
import { Character } from 'src/app/models/character.model';
import { HttpService } from 'src/app/services/http.service';
import { saveAs } from 'file-saver';

@Component({
  selector: 'app-characters',
  templateUrl: './characters.component.html',
  styleUrls: ['./characters.component.css']
})
export class CharactersComponent {
  characters: Character[] = [];
  showTable = false;
  buttonLabel = 'Populate Table';
  errorMessage = '';


  constructor(private httpService: HttpService) {}
  

  oggleTable(): void {
    if (this.showTable) {
      this.clearTable();
    } else {
      this.populateTable();
    }
  }

  
  toggleTable(): void {
    if (this.showTable) {
      this.clearTable();
    } else {
      this.populateTable();
    }
  }

  populateTable(): void {
    this.httpService.GetFilteredCharactersFromOriginalTrilogy().subscribe(
      (characters: any[]) => {
        this.characters = characters;
        this.buttonLabel = 'Clear';
        this.showTable = true;
        this.errorMessage = ''; // Clear any previous error message

      },
      (error: any) => {
        console.error('Error fetching characters:', error);
        this.errorMessage = 'Server is not responding. Please try again later. This can happen if the server has not fully loaded or is overloaded. Sorry for any inconvience.';

      }
    );
  }

  clearTable(): void {
    this.characters = [];
    this.buttonLabel = 'Populate Table';
    this.showTable = false;
    this.errorMessage = ''; // Clear any previous error message

  }

  downloadCSV(){
    this.httpService.DownloadCSVOfOriginalTrilogySorted().subscribe((data: Blob) => {
      var blob = new Blob([data], { type: 'text/csv' });
      saveAs(blob, 'original_trilogy_characters.csv');
    },
    (error: any) => {
      console.error('Error downloaded characters:', error);
      this.errorMessage = 'Server is not responding. Please try again later. This can happen if the server has not fully loaded or is overloaded. Sorry for any inconvience.';

    });
  }
}
