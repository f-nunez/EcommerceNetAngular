import { Component, OnInit } from '@angular/core';
import { AsyncValidatorFn, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { map, of, switchMap, timer } from 'rxjs';
import { AccountService } from '../account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;
  errors: string[];

  constructor(private accountService: AccountService, private router: Router, private formBuilder: FormBuilder) { }

  ngOnInit(): void {
    this.createRegisterForm();
  }

  createRegisterForm() {
    this.registerForm = this.formBuilder.group({
      displayName: [null, [Validators.required]],
      email: [
        null,
        [Validators.required, Validators.pattern('[\\w-]+@([\\w-]+\\.)+[\\w-]+')],
        [this.validateIfEmailIsNotTaken()]
      ],
      password: [null, [Validators.required]]
    });
  }

  onSubmit() {
    this.accountService.register(this.registerForm.value).subscribe({
      next: () => this.router.navigateByUrl('/shop'),
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
            })
          );
        })
      );
    };
  }

}
