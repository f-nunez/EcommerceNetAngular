import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { AccountService } from '../account/account.service';
import { BasketService } from '../basket/basket.service';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss']
})
export class CheckoutComponent implements OnInit {

  constructor(
    private formBuilder: FormBuilder,
    private accountService: AccountService,
    private basketService: BasketService) { }

  ngOnInit(): void {
    this.getAddressFormValues();
    this.getDeliveryFormValues();
  }

  checkoutForm = this.formBuilder.group({
    addressForm: this.formBuilder.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      street: ['', Validators.required],
      city: ['', Validators.required],
      state: ['', Validators.required],
      zipCode: ['', Validators.required]
    }),
    deliveryForm: this.formBuilder.group({
      deliveryMethod: ['', Validators.required]
    }),
    paymentForm: this.formBuilder.group({
      nameOnCard: ['', Validators.required]
    })
  });

  getAddressFormValues() {
    this.accountService.getUserAddress().subscribe({
      next: address => {
        if (address != null)
          this.checkoutForm.get('addressForm')?.patchValue(address);
      },
      error: error => console.log(error)
    });
  }

  getDeliveryFormValues() {
    const basket = this.basketService.getCurrentBasketValue();
    if (basket != null && basket.deliveryMethodId) {
      this.checkoutForm.get('deliveryForm')
        ?.get('deliveryMethod')
        ?.patchValue(basket.deliveryMethodId.toString());
    }
  }
}
