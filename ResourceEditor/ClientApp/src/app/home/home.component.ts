import { Component } from '@angular/core';
import { ResourceService } from '../products/resource.service';
import { Product } from '../products/product';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  products: Product[];
  errorMessage: string;


  constructor(private resourceService: ResourceService) {

  }

  OnClick() {
    //console.log("clicked !");
    this.resourceService.getProducts('abc').subscribe(
      (products: Product[]) => this.products = products,
      (err: any) => this.errorMessage = err.error
    );

  }

}

