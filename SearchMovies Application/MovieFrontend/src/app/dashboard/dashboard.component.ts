import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { LoginService } from '../Services/login.service';
import Swal from 'sweetalert2';
import {MatPaginatorModule, PageEvent} from '@angular/material/paginator';


@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule, MatPaginatorModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent {

  searchName: any = '';


  maxDate: any;
  isFormVisible: boolean = false;
  isUpdate: boolean = false;

  isLoading: boolean = false;
  movieslist: any;
  isemptylist: boolean = false;

  apiUrl = "https://localhost:7176";

  constructor(private loginregisterService: LoginService, private toastr: ToastrService,
    private router: Router, private route: ActivatedRoute) {
      this.updatePaginatedMovies();

     }
    
  ngOnInit(): void {
    this.maxDate = new Date().getFullYear()
    console.log(this.maxDate)
    this.navigateToUrl(this.searchName);
    this.GetAllMovies();
    // this.onPageChange();


    
  }

  pageIndex = 0;      // Current page index
  pageSize = 3;       // Items per page
  paginatedMovies: any =[]; // Array to hold the movies for the current page

 

  // Handle page change
  onPageChange(event: PageEvent) {
    console.log("PAgeEvent :", event);
    this.pageIndex = event.pageIndex;
    this.pageSize = event.pageSize;
    this.updatePaginatedMovies();
  }

  // Update the paginatedMovies list based on the current page and page size
  updatePaginatedMovies() {
    const start = this.pageIndex * this.pageSize;
    const end = start + this.pageSize;
    this.paginatedMovies = this.movieslist?.slice(start, end);
  }




  movieForm: FormGroup = new FormGroup({
    movieName: new FormControl('', [Validators.required, Validators.minLength(1), Validators.maxLength(30)]),
    year: new FormControl('', [Validators.required]),

  });

  onSearch() {
    console.log("SearchName : ", this.searchName)
    if(this.searchName == '')
    {
      this.GetAllMovies();
    }else{
      this.navigateToUrl(this.searchName);
    this.loginregisterService.GetMovies(this.searchName, this.apikey).subscribe({
      next: (res: any) => {
        if (res.status == 200) {
          this.movieslist = res.data;
          this.updatePaginatedMovies();
          if(this.movieslist =='')
          {
            this.isemptylist = true;
          }
          this.isemptylist = false;
          console.log("Movies Data : ", res.data);
        }
      }
    })}
  }
apikey:any = sessionStorage.getItem("ApiKey");
  navigateToUrl(search:string)
  {
    this.router.navigate([],{
      relativeTo: this.route,
      queryParams:{ s:search, apikey : this.apikey},
      queryParamsHandling: 'merge',
    })
  }

  GetAllMovies() {
    this.isLoading = true
    this.loginregisterService.GetAllMovie().subscribe({
      next: (res: any) => {
        if (res.status == 200) {
          this.movieslist = res.data;
          this.updatePaginatedMovies();
          this.isLoading = false;
        }
        this.isLoading = false;
      }
    });
    this.isLoading = false;
  }


  imageurl: any;
  updatemovie : any;
  OnUpdate(movie: any) {
    console.log(movie);
    this.isFormVisible = true;
    this.isUpdate = true;
    this.updatemovie = movie;
    this.movieForm.patchValue(movie);
    this.imageurl = movie.posterimage;
    console.log("imageurl : ", this.imageurl);
  }

  OnDelete(id: any) {
    Swal.fire({
      title: 'Are you sure?',
      text: 'Do you really want to Delete this Movie?',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Yes, Delete it!',
      cancelButtonText: 'No, keep it'
    }).then((result) => {
      if (result.isConfirmed) {
        this.loginregisterService.Deletemovie(id).subscribe({
          next: (res: any) => {
            if (res.status == 200) {
              Swal.fire({
                title: 'Success!',
                text: 'Movie Deleted!',
                icon: 'success',
                confirmButtonText: 'OK'
              });
              this.GetAllMovies();
            }
          }
        });
      }
    });
  }
  reset() {
    if (!this.searchName) {
      this.onSearch();
    }
  }

  selectedFile: File | null = null;

  onFileChange(event: any) {
    console.log("File", event);
    const file = event.target.files[0];
    console.log("File", file);
    if (file) {
      this.selectedFile = file;
    }
  }

  onSubmit() {
    this.isLoading = true;
    if (this.isUpdate == false) {
      if (this.movieForm.invalid || !this.selectedFile) {
        this.movieForm.markAllAsTouched();
        this.isLoading = false;
        this.toastr.error('Please fill in all required fields correctly.');
        return;
      }
      const formData = new FormData();
      Object.keys(this.movieForm.value).forEach((key) => {
        formData.append(key, this.movieForm.get(key)?.value || '');
      });
      formData.append('posterimage', this.selectedFile!);

      console.log("After movieData", formData)
      console.log("selected file", this.selectedFile)
      this.loginregisterService.AddMovie(formData).subscribe({
        next: (res: any) => {
          if (res.status == 200) {
            console.log('Register Form Submitted', res);
            this.movieForm.reset();
            this.isFormVisible = false;
            //this.loginForm.reset();
            this.isLoading=false;
            this.GetAllMovies();
            //this.toastr.success('Registration successful!');
            Swal.fire({
              position: "center",
              icon: "success",
              title: "Movie Added Successful!.",
              showConfirmButton: false,
              timer: 2500
            });
          }
          else {
            this.isLoading=false;
            this.toastr.error('Failed');
          }
        },
        error: (err: any) => {
          console.error(err);
          this.isLoading=false;
          this.toastr.error('Failed');
        },
      });
    }
    else {
      //for update
      if (this.movieForm.invalid) {
        this.movieForm.markAllAsTouched();
        this.isLoading = false;
        this.toastr.error('Please fill in all required fields correctly.');
        return;
      }
      const formData = new FormData();
      Object.keys(this.movieForm.value).forEach((key) => {
        formData.append(key, this.movieForm.get(key)?.value || '');
      });
      formData.append('posterimage', this.selectedFile!);

      console.log("After movieData update", formData)
      console.log("selected file", this.selectedFile)
      this.loginregisterService.update(this.updatemovie.id, formData).subscribe({
        next: (res: any) => {
          if (res.status == 200) {
            console.log('Update Form Submitted', res);
            this.movieForm.reset();
            this.isFormVisible = false;
            this.isUpdate = false;
            this.isLoading=false;
            this.GetAllMovies();

            Swal.fire({
              position: "center",
              icon: "success",
              title: "Movie Updated Successful!.",
              showConfirmButton: false,
              timer: 2500
            });
          }
          else {
            this.isLoading=false;
            this.isUpdate = false;
            this.toastr.error('Movie not Found');
          }
        },
        error: (err: any) => {
          console.error(err);
          this.isLoading=false;
          this.toastr.error('Failed');
          this.isUpdate = false;

        },
      });

    }

  }

  onCancel(): void {
    this.movieForm.reset();
    this.isFormVisible = false; // Hide form on cancel
  }

  OpenMovieForm() {
    this.isFormVisible = true;
  }
  omit_special_char(event: any) {
    var k;
    k = event.keyCode;  //         k = event.keyCode;  (Both can be used)
    if (k >= 48 && k <= 57) {
      return true
    }
    else {
      return false
    }
  }


}
