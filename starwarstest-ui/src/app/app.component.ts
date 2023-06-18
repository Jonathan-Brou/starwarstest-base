import { Component, OnInit } from '@angular/core';
import { HttpService } from './services/http.service';
import { person } from './model/person';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'starwarstest-ui';

  persons: person[] = null;
  isLoading: boolean = false;

  constructor(private httpService: HttpService) {

  }

  ngOnInit(): void {
    this.testApiConnection();
  }

  async getPersons(): Promise<void> {
    this.isLoading = true;
    try {
      const data = await this.httpService.getAllPersons().toPromise();
      this.persons = data as person[]
    } catch (error) {
      console.log("Error! " + error)
    } finally {
      this.isLoading = false
    }
  }

  testApiConnection(){
    this.httpService.testApiConnection().subscribe(
      (response) => {
        console.log(response);
      }
    );
  }

  downloadData(): void {
    const filename = 'SWAPIPersons.csv';
    const csvContent = this.generateCSVContent();

    // Create a Blob with the CSV content
    const blob = new Blob([csvContent], { type: 'text/csv' });

    const link = document.createElement('a');
    link.href = window.URL.createObjectURL(blob);
    link.download = filename;
    link.click();
    window.URL.revokeObjectURL(link.href);
    link.remove();
  }

  generateCSVContent(): string {
    // Assuring the CSV headers match the UI table.
    const header = 'Name,Homeworld,DoB,Height\n';
    const rows = this.persons.map(x => `${x.name},${x.homeworld},${x.birthYear},${x.height}\n`);
    return header + rows.join('');
  }
}
