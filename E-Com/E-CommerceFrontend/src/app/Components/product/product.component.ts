import { Component, inject, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ProductService } from '../../Services/product.service';
import { ToastrService } from 'ngx-toastr';
import Swal from 'sweetalert2';
import { sellingPriceGreaterThanPurchasePrice } from './CustomValidator';

@Component({
  selector: 'app-product-manager',
  templateUrl: './product.component.html',
  styleUrl: './product.component.css',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule]
})
export class ProductComponent implements OnInit {
  products: any;
  showForm = false;
  viewForm = false;
  formType: 'add' | 'edit' | 'view' | null = null;
  selectedProduct: any = null;
  private toastr: ToastrService;
  maxDate = new Date().toISOString().split('T')[0];
  apiUrl = "https://localhost:7162"
  productImage: any;


  productForm = new FormGroup({
    productName: new FormControl('', [Validators.required, Validators.maxLength(20)]),
    productCode: new FormControl('', [Validators.required, Validators.pattern('^[a-zA-Z0-9_]*$')]),
    category: new FormControl('', [Validators.required, Validators.maxLength(10)]),
    brand: new FormControl('', [Validators.required, Validators.maxLength(10)]),
    sellingPrice: new FormControl('', [Validators.required, Validators.min(0.01)]),
    purchasePrice: new FormControl('', [Validators.required, Validators.min(0.01)]),
    purchaseDate: new FormControl('', Validators.required),
    stock: new FormControl('', [Validators.required, Validators.min(0)])
  }, { validators: sellingPriceGreaterThanPurchasePrice });

  constructor(private productService: ProductService) {
    this.toastr = inject(ToastrService);

  }

  ngOnInit() {
    this.loadProducts();
  }

  // loadProducts() {
  //   this.productService.getProducts().subscribe((res:any) => {
  //     this.products = res?.data;
  //     console.log('Products: ', this.products);
  //   });
  // }

  loadProducts() {
    const UsertableId = sessionStorage.getItem('userid');
    this.productService.getProductbyuserid(UsertableId).subscribe((res: any) => {
      this.products = res?.data;
      if (this.products == null || this.products.length === 0) {
        Swal.fire({
          title: 'No Products Present',
          text: 'No products are present in your account.',
          icon: 'info',
          showCancelButton: true,
          confirmButtonText: 'Add Product',
          cancelButtonText: 'Close'
        }).then((result) => {
          if (result.isConfirmed) {
            // Navigate to the add product page or open the add product form
            this.openForm('add', null);
          }
        });
      }

      console.log('Products: ', this.products);
    });
  }




  openForm(type: 'add' | 'edit' | 'view', product?: any) {
    this.formType = type;
    this.selectedProduct = product || null;
    if (type === 'edit' && product) {
      this.showForm = true;
      this.productForm.patchValue(product);
    }
    else if (type === 'view' && product) {
      this.productImage = `${this.apiUrl}${product.productImagePath}`;
      console.log("ProductImage : ", this.productImage);
      this.viewForm = true;
    }
    else if (type === 'add') {
      this.showForm = true;
      this.productForm.reset();
    }
  }

  closeForm() {
    this.showForm = false;
    this.viewForm = false;
    this.formType = null;
    this.selectedProduct = null;
    this.productForm.reset();
    this.loadProducts();
  }

  // onFileChange(event: any) {
  //   const file = event.target.files[0];
  //   if (file) {
  //     const reader = new FileReader();
  //     reader.onload = () => {
  //       this.productForm.patchValue({
  //         productImagePath: reader.result as string
  //       });
  //     };
  //     reader.readAsDataURL(file);
  //   }
  // }
  selectedFile: File | null = null; // To store the Base64 image 

  onFileChange(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.selectedFile = file;
      // const reader = new FileReader();
      // reader.onload = () => {
      //   this.base64Image = reader.result as string; // Store Base64 string in a variable
      // };
      // reader.readAsDataURL(file);
    }
  }

  submitForm() {
    debugger
    if (this.productForm.invalid) {
      this.toastr.error('Please fill in all required fields correctly.');
      this.productForm.markAllAsTouched();

      return;
    }

    const UsertableId = sessionStorage.getItem('userid');

    const formData = new FormData();
    Object.keys(this.productForm.value).forEach((key) => {
      formData.append(key, this.productForm.get(key)?.value || '');
    });
    formData.append('productImagePath', this.selectedFile!);
    formData.append('UsertableId', UsertableId!);
    //const formData = this.productForm.value;
    console.log("formData-----", formData)
    if (this.formType === 'edit' && this.selectedProduct) {
      this.productService.updateProduct(this.selectedProduct.id, formData).subscribe((res: any) => {
        console.log("rewwww---", res)
        this.toastr.success('Product updated successfully.');
        this.closeForm();
        this.loadProducts();
      });
    } else {
      this.productService.addProduct(formData).subscribe(() => {
        this.toastr.success('Product added successfully.');
        this.closeForm();
        this.loadProducts();
      });
    }
  }

  // deleteProduct(productId: number) {
  //   this.productService.deleteProduct(productId).subscribe(() => {
  //     this.loadProducts();
  //     this.toastr.success('Product Deleted Successfully!');
  //   });
  // }

  deleteProduct(productId: number) {
    Swal.fire({
      title: 'Are you sure?',
      text: 'Do you really want to delete this product? This process cannot be undone.',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Yes, delete it!',
      cancelButtonText: 'No, keep it'
    }).then((result) => {
      if (result.isConfirmed) {
        this.productService.deleteProduct(productId).subscribe(() => {
          this.loadProducts();
          Swal.fire({
            icon: 'success',
            title: 'Product Deleted Successfully!',
            showConfirmButton: false,
            timer: 1500
          });
        });
      } else if (result.dismiss === Swal.DismissReason.cancel) {
        Swal.fire({
          icon: 'info',
          title: 'Product Deletion Cancelled',
          showConfirmButton: false,
          timer: 1500
        });
      }
    });
  }




}
