import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders } from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class HttpService {
  baseUrl = environment.apiBaseUrl;

  constructor(private http: HttpClient) { }

  testApiConnection() {
    var httpOptions = {
      headers: new HttpHeaders({
        "Access-Control-Allow-Origin": "*",
        "Access-Control-Allow-Methods": "*",
        "Access-Control-Allow-Headers": "Content-Type, Access-Control-Allow-Origin"
      })
    }
    return this.http.get(this.baseUrl + 'test', httpOptions).pipe();
  }

  getAllPersons() {
    var httpOptions = {
      headers: new HttpHeaders({
        "Access-Control-Allow-Origin": "*",
        "Access-Control-Allow-Methods": "*",
        "Access-Control-Allow-Headers": "Content-Type, Access-Control-Allow-Origin"
      })
    }
    return this.http.get(this.baseUrl + 'api/person', httpOptions).pipe();
  }

}
