import { Component, OnInit } from '@angular/core';
import { HttpService } from './services/http.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'starwarstest-ui';

  constructor(private httpService: HttpService) {

  }

  ngOnInit(): void {
    this.testApiConnection();
  }

  testApiConnection(){
    this.httpService.testApiConnection().subscribe(
      (response) => {
        console.log(response);
      }
    );
  }
}
