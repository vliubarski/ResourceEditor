import { Component } from '@angular/core';
import { ResourceService } from '../products/resource.service';
import { Product } from '../products/product';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  products: Product[];
  data: Product[];
  errorMessage: string;

  constructor(private resourceService: ResourceService) { }

  onSearch(lookFor: string = '') {
    this.resourceService.getProducts(lookFor).subscribe(
      (data: Product[]) => this.data = data,
      (err: any) => this.errorMessage = err.error
    );

  }

  settings = {
    columns: {
      id: {
        title: 'ID'
      },
      productName: {
        title: 'Name'
      },
      productCode: {
        title: 'Code'
      },
      starRating: {
        title: 'Rating'
      }
    }
  };
}

