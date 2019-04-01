import { Component, OnInit } from '@angular/core';
import { ResourceService } from '../products/resource.service';
import { Product, Filter } from '../products/product';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  products: Product[];
  data: Product[];
  filter: Filter[];
  errorMessage: string;

  constructor(private resourceService: ResourceService) {
  }

  onSearch(lookFor: string = '') {
    this.resourceService.getProducts(lookFor).subscribe(
      (data: Product[]) => this.data = data,
      (err: any) => this.errorMessage = err.error
    );

    let cultureCodes = this.data.map(u => { return { cultureCode: u.cultureCode } });
    let filter = Array.from(new Set(cultureCodes.map(item => item.cultureCode)))
      .map(id => { return { title: id, value: id } });

    this.settings.columns.cultureCode.filter.config.list = filter;
    this.settings = Object.assign({}, this.settings);
  }

  settings = {
    columns: {
      resourceType: {
        title: 'Type'
      },
      resourceKey: {
        title: 'Key', editable: false
      },
      resourceValue: {
        title: 'Value'
      },
      cultureCode: {
        with: 100,
        title: 'Culture Code',
        filter: {
          type: 'list',
          config: {
            selectText: 'Select...',
            list: this.filter,
          },
        }
      }
    }
  };
}

