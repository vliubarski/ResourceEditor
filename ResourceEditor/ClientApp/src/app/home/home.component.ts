import { Component, OnInit } from '@angular/core';
import { ResourceService } from '../products/resource.service';
import { Product, Filter } from '../products/product';
import { LocalDataSource, ServerDataSource } from 'ng2-smart-table';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  products: Product[];
  data: Product[] =[] ;
  filter: Filter[];
  errorMessage: string;

  constructor(private resourceService: ResourceService, http: HttpClient) {
  }

  onSearch(lookFor: string = '') {
    this.resourceService.getProducts(lookFor).subscribe(
      (data: Product[]) => this.data = data,
      (err: any) => this.errorMessage = err.error
    );

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

  onCreateConfirm(event) {
    this.resourceService.saveProducts(event.newData).
      subscribe(data => this.data.push(data));
  }

  onDeleteConfirm(event) {
    console.log("Delete Event In Console")
    console.log(event);
    if (window.confirm('Are you sure you want to delete?')) {
      event.confirm.resolve();
    } else {
      event.confirm.reject();
    }
  }

  onSaveConfirm(event) {
    console.log("Edit Event In Console")
    console.log(event);
    event.confirm.resolve();
  }

  settings = {
    delete: {
      confirmDelete: true,
    },
    add: {
      confirmCreate: true,
    },
    edit: {
      confirmSave: true,
    },
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
        width: '150px',
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

