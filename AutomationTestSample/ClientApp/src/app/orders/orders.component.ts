import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { SpinnerService } from '../services/spinner.service';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html'
})
export class OrdersComponent {

  private httpClient: HttpClient;
  private baseUrl: string;
  public orders: Order[] = [];

  constructor(http: HttpClient,
      @Inject('BASE_URL') baseUrl: string,
      private spinnerService: SpinnerService) {
    this.httpClient = http;
    this.baseUrl = baseUrl;
  }

  ngOnInit(): void {
    this.loadOrders();
  }
  
  public loadOrders() {
    this.httpClient.get<Order[]>(this.baseUrl + `api/orders`).subscribe(result => {
      this.orders = result;
    }, error => console.error(error));
  }

  public deleteOrder(order: Order) {
    if (confirm("Are you sure to cancel accession " + order.accessionNumber + " ?")) {
      this.spinnerService.show();
      this.httpClient.delete(this.baseUrl + `api/orders/${order.id}`).subscribe(result => {
        this.spinnerService.hide();
        this.loadOrders();
      }, error => {
        this.spinnerService.hide();
        console.error(error);
      });
    }
  }
}

interface Order {
  id: number;
  accessionNumber: string;
  orgCode: string;
  siteName: string;
  patientMrn: string;
  patientName: string;
  modality: string;
  studyDateTime: Date;
  status: string;
}
