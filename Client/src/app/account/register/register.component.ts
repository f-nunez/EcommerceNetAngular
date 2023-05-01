import { Component } from '@angular/core';
import { AbstractControl, AsyncValidatorFn, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { debounceTime, finalize, map, of, switchMap, take, timer } from 'rxjs';
import { AccountService } from '../account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
  errors: string[] | null = null;

  constructor(private accountService: AccountService, private router: Router, private formBuilder: FormBuilder) { }

  registerForm = this.formBuilder.group({
    displayName: ['', Validators.required],
    email: ['', [Validators.required, Validators.email], [this.validateIfEmailIsNotTaken()]],
    password: ['', [Validators.required]],
  })

  onSubmit() {
    this.accountService.register(this.registerForm.value).subscribe({
      next: () => {
        console.log("Navigating!");
        this.router.navigateByUrl('/shop');
      },
      error: (error) => {
        let errorProperties = Object.keys(error.errors);
        let errorArray = [];
        for (let errorProperty of errorProperties) {
          errorArray.push(error.errors[errorProperty]);
        }
        this.errors = errorArray;
      }
    }
    );
  }

  validateIfEmailIsNotTaken(): AsyncValidatorFn {
    return control => {
      return timer(500).pipe(
        switchMap(() => {
          if (!control.value) {
            return of(null);
          }
          return this.accountService.emailExists(control.value).pipe(
            map(response => {
              return response ? { emailExists: true } : null;
            }, finalize(() => control.markAsTouched()))
          );
        })
      );
    };
  }
}
