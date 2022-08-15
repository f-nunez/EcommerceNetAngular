import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-test-error',
  templateUrl: './test-error.component.html',
  styleUrls: ['./test-error.component.scss']
})
export class TestErrorComponent implements OnInit {
  baseUrl: string = environment.apiUrl;
  validationErrors: any;

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
  }

  get400Error() {
    this.http.get(this.baseUrl + 'buggy/badrequest').subscribe({
      next: (response) => console.log(response),
      error: (error) => console.log(error)
    });
  }

  get400ValidationError() {
    this.http.get(this.baseUrl + 'products/fortytwo').subscribe({
      next: (response) => console.log(response),
      error: (error) => {
        let errorProperties = Object.keys(error.errors);
        let errorArray = [];
        for (let errorProperty of errorProperties) {
          errorArray.push(error.errors[errorProperty]);
        }
        this.validationErrors = errorArray;
      }
    });
  }

  get404Error() {
    this.http.get(this.baseUrl + 'products/42').subscribe({
      next: (response) => console.log(response),
      error: (error) => console.log(error)
    });
  }

  get500Error() {
    this.http.get(this.baseUrl + 'buggy/servererror').subscribe({
      next: (response) => console.log(response),
      error: (error) => console.log(error)
    });
  }
}
