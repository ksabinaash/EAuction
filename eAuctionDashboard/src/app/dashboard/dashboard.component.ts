import { AfterViewInit, Component, OnInit, ViewChild, ChangeDetectorRef } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Buyer } from '../models/buyer';
import { Product } from '../models/product';
import { ProductService } from '../services/product.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})

export class DashboardComponent implements AfterViewInit {

  products!: Product[];

  selectedProductId!: string;

  product!: Product | null;

  bids!: Buyer[];

  displayedColumns: string[] = ['bidAmount', 'firstName', 'email', 'phone'];

  dataSource!: MatTableDataSource<Buyer>;

  @ViewChild(MatPaginator) paginator !: MatPaginator;

  @ViewChild(MatSort) sort !: MatSort;

  /**
   * Set the paginator after the view init since this component will
   * be able to query its view for the initialized paginator.
   */
  ngAfterViewInit() {

  }

  constructor(private productService: ProductService, private detect: ChangeDetectorRef) {

    this.getProducts();

  }

  ngOnInit(): void {

  }

  getProducts(): void {

    this.products = [];

    this.selectedProductId = "";

    this.product = null;

    this.bids = [];

    this.productService.getProducts()
      .subscribe(products => {

        this.products = products;

        this.product = this.products[0];

        this.selectedProductId = this.product.productId;

        this.bids = [];

      });
  }

  getBids(): void {
    if (this.product != null) {

      this.bids = this.product.buyers;

      this.refreshDataSource();
    }
    else {
      console.log("Getting bids failed!");
    }
  }

  getProduct(): void {
    if (this.products != null && this.products.length > 0) {

      this.product = this.products.filter(m => m.productId == this.selectedProductId)[0];

      this.bids = [];
    }

    else {
      console.log("Getting product failed!");
    }
  }

  refreshDataSource(): void {

    this.dataSource = new MatTableDataSource(this.bids);

    this.detect.detectChanges();

    this.dataSource.paginator = this.paginator;

    this.dataSource.sort = this.sort;

  }
}
