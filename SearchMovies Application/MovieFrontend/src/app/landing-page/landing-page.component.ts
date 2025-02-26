import { Component } from '@angular/core';
import { LoginService } from '../Services/login.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';


@Component({
  selector: 'app-landing-page',
  standalone: true,
  imports: [CommonModule, FormsModule, MatPaginatorModule],
  templateUrl: './landing-page.component.html',
  styleUrl: './landing-page.component.css'
})
export class LandingPageComponent {

  searchName: any = '';


  maxDate: any;
  isFormVisible: boolean = false;
  isUpdate: boolean = false;
  isemptylist:boolean = false;
  isLoading: boolean = false;
  movieslist: any;
  apiUrl = "https://localhost:7176";

  constructor(private loginregisterService: LoginService, private toastr: ToastrService,
    private router: Router, private route: ActivatedRoute) { }
  ngOnInit(): void {
    this.maxDate = new Date().getFullYear()
    console.log(this.maxDate);
    this.navigateToUrl(this.searchName);

    this.GetAllMovies()
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

  onSearch() {
    console.log("SearchName : ", this.searchName)
    if(this.searchName == '')
    {
      this.GetAllMovies();
    }
    else
    {
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
    })
    }
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
 
  reset() {
    if (!this.searchName) {
      this.onSearch();
    }
  }
}
