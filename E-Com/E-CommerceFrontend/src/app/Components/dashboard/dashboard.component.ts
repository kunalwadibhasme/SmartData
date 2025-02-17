import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { ProductService } from '../../Services/product.service';
import { ToastrService } from 'ngx-toastr';
import { AddtoCartService } from '../../Services/addto-cart.service';
import { SharedserviceService } from '../../Services/sharedservice.service';

@Component({
    selector: 'app-dashboard',
    standalone: true,
    imports: [CommonModule],
    templateUrl: './dashboard.component.html',
    styleUrl: './dashboard.component.css'
})
export class DashboardComponent {
    toastr: any;
    apiUrl = "https://localhost:7162"


    ngOnInit() {
        if(this.usertypeid !='2')
        {
            this.allow = false;
        }
        this.loadProducts();
        
    }
    constructor(private productService: ProductService, private CartService:AddtoCartService, private Sharedsevice : SharedserviceService) {
        this.toastr = inject(ToastrService);
    }
    products : any
    allow : boolean = true;
    usertypeid = sessionStorage.getItem('usertypeid');

    loadProducts() {
        this.productService.getProducts().subscribe((res: any) => {
            this.products = res?.data;
            console.log('Products: ', this.products);
        });
    }

     
    // addToCart(product: any) {
    //     console.log(`Added ${product.productName} to the cart!`);
    //     const userid = sessionStorage.getItem('userid');
    //     this.CartService.CartMasterDetail(userid, product.id, 1).subscribe((res:any)=>{
    //         console.log('AddtoCart Response : ',res);
            
    //         const TotalQuantity = res;
    //         this.Sharedsevice.updateTotalQuantity(TotalQuantity);
    //     });
        
    // }
    addToCart(product: any) {
        console.log(`Added ${product.productName} to the cart!`);
        const userid = sessionStorage.getItem('userid');
        this.CartService.getProductById(product.id).subscribe((res:any)=>{
          if(res.data.stock === 1){
            this.toastr.error('Product Out of Stock');
          }
          else{
            this.CartService.CartMasterDetail(userid, product.id, 1).subscribe((res:any)=>{
              console.log('AddtoCart Response : ',res);
              this.toastr.success('Product added to cart Successfully!', null, { timeOut: 1000 })
              const TotalQuantity = res;
              this.Sharedsevice.updateTotalQuantity(TotalQuantity);
              //this.cartItems = this.cartItems.filter(item => item.id !== product.itemId);
            // this.calculateSubtotal();
            // this.fetchCartProducts();
          });
          }
        })
        
      }

}

