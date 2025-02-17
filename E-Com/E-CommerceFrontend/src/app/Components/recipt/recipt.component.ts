import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { AddtoCartService } from '../../Services/addto-cart.service';
import { Route, Router } from '@angular/router';
import { delay } from 'rxjs/operators';
import { ReceiptSharedserviceService } from '../../Services/receipt-sharedservice.service';

@Component({
  selector: 'app-recipt',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './recipt.component.html',
  styleUrl: './recipt.component.css'
})
export class ReciptComponent {

  closeModel() {
    this.router.navigateByUrl('/dashboard');
    this.isvisible = false;
    this.respData = [];

  }

  isvisible: boolean = true;
  respData: any;
  respData1 : any;
  allow: boolean = true;
  usertypeid = sessionStorage.getItem('usertypeid');
  constructor(private cartService: AddtoCartService, private router: Router,
              private receiptsharedService: ReceiptSharedserviceService) {

  }
  ngOnInit() {
    if (this.usertypeid != '2') {
      this.allow = false;
    }
    this.receiptsharedService.receiptData$.subscribe(data =>{
      this.respData1 = data;
      console.log("ReceiptData :==", this.respData1);
    })
    

    // const userid = sessionStorage.getItem('userid');
    // this.GetReceiptData(userid);
  }

  GetReceiptData(userid: any) {
    this.cartService.GenerateReceipt(userid).subscribe((res: any) => {
      console.log("ReceiptData : ", res);
      this.respData = res;
    });
  }

  printInvoice() {
    window.print();
  }


}





