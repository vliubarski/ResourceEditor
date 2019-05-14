import { Component } from '@angular/core';
import { ResourceService } from '../products/resource.service';
import { Product, Filter } from '../products/product';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  products: Product[];
  data: Product[] = [];
  filter: Filter[];
  errorMessage: string;

  constructor(private resourceService: ResourceService, private spinner: NgxSpinnerService) {
  }

  onSearch(lookFor: string) {
    if (lookFor === "") {
      return;
    }
    this.spinner.show();
    this.resourceService.getProducts(lookFor).subscribe(
      (data: Product[]) => {
        this.data = data; this.populateFilter(); this.spinner.hide();
      },
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
      (data: boolean) => {
        if (data) {
          event.confirm.resolve(event.newData);
        } else {
          window.alert('Cannot create resource!');
          event.confirm.reject();
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
    if (!window.confirm('Are you sure you want to delete?')) {
      return;
    }
    this.resourceService.deleteResource(event.data).subscribe(
      (data: Product) => {
        if (data) {
          event.confirm.resolve(event.data);
        } else {
          event.confirm.reject();
        }
      },
      (err: any) => this.errorMessage = err.error
    );
    event.confirm.resolve();
  }

  onUpdate(event) {
    if (!window.confirm('You are about to Update this resource. Continue?')) {
      return;
    }
    this.resourceService.updateResource(event.newData).subscribe(
      (data: boolean) => {
        if (data) {
          event.confirm.resolve(event.newData);
        } else {
          event.confirm.reject();
        }
      },
      (err: any) => this.errorMessage = err.error
    );
  }

  settings = {
    delete: {
      confirmDelete: true,
      deleteButtonContent: ' Delete',
    },
    add: {
      confirmCreate: true,
      cancelButtonContent: ' Cancel',
      createButtonContent: 'Create '
    },
    edit: {
      confirmSave: true,
      editButtonContent: 'Edit',
      saveButtonContent: 'Update',
      cancelButtonContent: ' Cancel'
    },
    columns: {
      resourceType: {
        title: 'Type', editable: false
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

