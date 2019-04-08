import { Component } from '@angular/core';
import { ResourceService } from '../products/resource.service';
import { Product, Filter } from '../products/product';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  products: Product[];
  data: Product[] = [];
  filter: Filter[];
  errorMessage: string;

  constructor(private resourceService: ResourceService) {
  }

  onSearch(lookFor: string = '') {
    this.resourceService.getProducts(lookFor).subscribe(
      (data: Product[]) => { this.data = data; this.populateFilter(); },
      (err: any) => this.errorMessage = err.error
    );
  }

  populateFilter() {
    let cultureCodes = this.data.map(u => { return { cultureCode: u.cultureCode } });
    let filter = Array
      .from(new Set(cultureCodes.map(item => item.cultureCode)))
      .map(id => { return { title: id, value: id } });

    this.settings.columns.cultureCode.filter.config.list = filter;
    this.settings = Object.assign({}, this.settings);
  }

  onCreateConfirm(event) {
    this.resourceService.createResource(event.newData).subscribe(
      (data: Product) => {
        if (this.validated(data)) {
          //this.data.push(data); //this.populateFilter();
          event.confirm.resolve(event.newData);
        } 
      },
      (err: any) => this.errorMessage = err.error
    );
  }

  validated(data: Product): boolean {
    if (data.resourceKey !== null) {
      return true;
    }
    return false;
  }

  onDeleteConfirm(event) {
    this.resourceService.createResource(event.newData).subscribe(
      (data: Product) => {
        if (this.validated(data)) {
          //this.data.push(data); //this.populateFilter();
          event.confirm.resolve(event.newData);
        }
      },
      (err: any) => this.errorMessage = err.error
    );

    //console.log("Delete Event In Console");
    //console.log(event);
    //if (window.confirm('Are you sure you want to delete?')) {
    //  event.confirm.resolve();
    //} else {
    //  event.confirm.reject();
    //}
  }

  onSaveConfirm(event) {
    console.log("Edit Event In Console");
    console.log(event);
    event.confirm.resolve();
  }

  settings = {
    delete: {
      confirmDelete: true,
      deleteButtonContent: ' Delete',
    },
    add: {
      confirmCreate: true,
      cancelButtonContent: ' Cancel',
    },
    edit: {
      confirmSave: true,
      editButtonContent: ' Edit',
      cancelButtonContent: ' Cancel',
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

