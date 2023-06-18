import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HttpService {
  baseUrl = environment.apiBaseUrl;

  constructor(private http: HttpClient) { }

  GetFilteredCharactersFromOriginalTrilogy(): Observable<any> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Access-Control-Allow-Origin': '*',
        'Access-Control-Allow-Methods': '*',
        'Access-Control-Allow-Headers': 'Content-Type, Access-Control-Allow-Origin'
      })
    };
    return this.http.get<any>(this.baseUrl + 'test', httpOptions);
  }


  DownloadCSVOfOriginalTrilogySorted(): Observable<any> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Access-Control-Allow-Origin': '*',
        'Access-Control-Allow-Methods': '*',
        'Access-Control-Allow-Headers': 'Content-Type, Access-Control-Allow-Origin'
      }),
      responseType: 'blob' as 'json' 

    };
    return this.http.get<any>(this.baseUrl + 'originaltrilogycharacters/sorted/download-csv', httpOptions);
  }
}