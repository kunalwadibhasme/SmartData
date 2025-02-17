import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { AddtoCartService } from '../../Services/addto-cart.service';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import Swal from 'sweetalert2';
import { Router } from '@angular/router';
import { SharedserviceService } from '../../Services/sharedservice.service';
import { ReceiptSharedserviceService } from '../../Services/receipt-sharedservice.service';


interface CartItem {
  id: number;
  brand: string;
  productName: string;
  productCode: string;
  sellingPrice: number;
  quantity: number;
}


@Component({
  selector: 'app-displaycartproducts',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './displaycartproducts.component.html',
  styleUrl: './displaycartproducts.component.css'
})
export class DisplaycartproductsComponent {

  cartItems: CartItem[] = [];
  subtotal: number = 0;
  isVisible: boolean = false;
  paymentForm: FormGroup;
  constructor(private cartService: AddtoCartService, private fb: FormBuilder, private Sharedsevice: SharedserviceService,
    private toastr: ToastrService, private router: Router, private CartService: AddtoCartService,
    private receiptservice: ReceiptSharedserviceService) {
    this.paymentForm = this.fb.group({
      cardNumber: ['', [Validators.required, Validators.pattern('^[0-9]{16}$')]],
      expiryDate: ['', [Validators.required, Validators.pattern('^(0[1-9]|1[0-2])\/?([0-9]{2})$')]],
      cvv: ['', [Validators.required, Validators.pattern('^[0-9]{3}$')]],
      amount: [this.subtotal]
    });
  }

  ngOnInit(): void {

    this.fetchCartProducts();
  }




  omit_special_char(event : any)
{   
   var k;  
   k = event.keyCode;  //         k = event.keyCode;  (Both can be used)
   if(k >= 48 && k <= 57)
    {
      return true
    }
    else
    {
      
      //this.paymentForm.get('cvv')?.markAsTouched()
      return false
    } 
}



onSubmit() {
  const userId = sessionStorage.getItem('userid');
  if (this.paymentForm.valid) {
    // var credentials = this.paymentForm.value;


    this.cartService.ValidatePayment(this.paymentForm.value.cardNumber, this.paymentForm.value.expiryDate, this.paymentForm.value.cvv).subscribe(
      (response) => {
        if (response == true) {
          //sweetAlert
          Swal.fire({
            title: "Payment Successful!",
            text: "Your payment has been processed successfully.",
            icon: "success",
            confirmButtonColor: "#3085d6",
            confirmButtonText: "OK"
          }).then((result) => {
            if (result.isConfirmed) {
              console.log("Payment confirmed");
              // this.updatesalesdetail();
              this.updatesalesdetail2();

              
              this.isVisible = false;
              this.router.navigateByUrl('/receipt');
            }
          });
        } else {

          Swal.fire({
            title: "Payment Failed",
            text: "Your payment credentials are incorrect. Please try again.",
            icon: "error",
            confirmButtonColor: "#d33",
            confirmButtonText: "OK"
          });
        }
      },
      (error) => {
        // Handle error from API call
        Swal.fire({
          title: "Payment Error",
          text: "An error occurred while processing your payment. Please try again.",
          icon: "error",
          confirmButtonColor: "#d33",
          confirmButtonText: "OK"
        });
      }
    );
  } else {

    this.paymentForm.markAllAsTouched();
  }
}


updatesalesdetail(): void {
  const userid = sessionStorage.getItem('userid');
  this.cartService.Updatesalesdetail(userid, this.subtotal).subscribe((res: any) => {
      this.router.navigateByUrl('/receipt');
    const Invoice = res;
    console.log('Invoice Id is ; ', Invoice);
  });
}

updatesalesdetail2(): void {
  const userid = sessionStorage.getItem('userid');
  this.cartService.Updatesalesdetail2(userid, this.subtotal).subscribe((res: any) => {
      //this.router.navigateByUrl('/receipt');
    const Invoicedata = res;
    this.receiptservice.GetReceiptData(Invoicedata);
    console.log('Invoice Id is ; ', Invoicedata);
  });

}




onCancel() {
  this.isVisible = false;
  this.paymentForm.reset();
}
Back()
{
  this.router.navigateByUrl('/dashboard');
}


fetchCartProducts(): void {
  const userid = sessionStorage.getItem('userid');
  this.cartService.getCartItems(userid).subscribe((res: any) => {
    console.log("cartItems ==", res);
    this.cartItems = res;
    console.log("this.cartItems==>>", this.cartItems);
    this.calculateSubtotal();
  });
}

calculateSubtotal(): void {
  this.subtotal = this.cartItems.reduce((acc, item) => acc + (item.sellingPrice * item.quantity), 0);
}

deleteItem(itemId: number): void {
  const userid = sessionStorage.getItem('userid');
  this.cartService.deleteCartItem(itemId, userid).subscribe(() => {
    this.cartItems = this.cartItems.filter(item => item.id !== itemId);
    this.calculateSubtotal();
    this.fetchCartProducts();
  });
}

addToCart(product: any) {
  console.log(`Added ${product.productName} to the cart!`);
  const userid = sessionStorage.getItem('userid');
  this.CartService.getProductById(product.id).subscribe((res: any) => {
    if (res.data.stock === 1) {
      this.toastr.error('Product Out of Stock');
    }
    else {
      this.CartService.CartMasterDetail(userid, product.id, 1).subscribe((res: any) => {
        console.log('AddtoCart Response : ', res);
        const TotalQuantity = res;
        this.Sharedsevice.updateTotalQuantity(TotalQuantity);
        //this.cartItems = this.cartItems.filter(item => item.id !== product.itemId);
        this.calculateSubtotal();
        this.fetchCartProducts();
      });
    }
  })

}



pay(): void {
  if(this.cartItems.length === 0) {
  // Show error message
  this.toastr.error('Cart is empty. Please add items to the cart before proceeding.');
  return;
}
// Implement payment logic here
this.paymentForm.reset();
this.isVisible = true;
    }
    
}



