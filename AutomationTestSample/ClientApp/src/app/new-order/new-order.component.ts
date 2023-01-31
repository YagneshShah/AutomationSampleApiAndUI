import { Component, Inject } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { SpinnerService } from '../services/spinner.service';

@Component({
  selector: 'app-new-order',
  templateUrl: './new-order.component.html',
  styleUrls: ['./new-order.component.css']
})
export class NewOrderComponent {

  clients: Client[] = [];
  sites: ClientSite[] = [];
  modalities: Modality[] = [];
  errorMessage?: string;

  orderForm = this.fb.group({
    patientMrn: [null, [Validators.required, Validators.maxLength(16)]], // the .NET APIs are 15 or 12 length depending which one is used
    patientFirstName: [null, [Validators.required, Validators.maxLength(64)]],
    patientLastName: [null, [Validators.required, Validators.maxLength(64)]],

    accessionNumber: [null, [Validators.required, Validators.maxLength(12)]], // the .NET APIs are 16 length, and the UI validatation says this is 10
    orgCode: [null, [Validators.required, Validators.maxLength(5)]],
    siteId: [null, [Validators.required, Validators.maxLength(5)]],
    modality: [null], // the .NET APIs require this, and have a max length of 5
    studyDateTime: [null, Validators.required] // the .NET APIs enforce this to not be in the future
  });

  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string,
    private fb: FormBuilder,
    private router: Router,
    private spinnerService: SpinnerService) {
    
  }

  ngOnInit(): void {
    this.loadLookupData();
  }

  onOrgChange(orgCode?: string): void {
    this.sites = this.clients.find(p => p.orgCode === orgCode)?.sites ?? [];
  }

  /**
   * Submits form data to the server to create a new order.
   */
  onSubmit() {
    this.errorMessage = undefined;
    if (this.orderForm.valid) {
      this.spinnerService.show();
      this.http.post(this.baseUrl + `api/orders`, this.orderForm.value).subscribe(result => {
        this.spinnerService.hide();
        // Success! Redirect to Orders page
        this.router.navigate(['/orders']);
      }, errorResponse => {
        this.spinnerService.hide();
        console.error(errorResponse);
        this.errorMessage = this.getErrorMessageFromResponse(errorResponse);
      });
    }
    else {
      // Mark all fields as touched to trigger validation messages
      this.orderForm.markAllAsTouched();
    }
  }

  /**
   * Loads look up data such as the list of clients and sites from the server.
   */
  private loadLookupData(): void {
    this.http.get<Client[]>(this.baseUrl + `api/clients`).subscribe(result => {
      this.clients = result;
    }, error => console.error(error));
    this.http.get<Modality[]>(this.baseUrl + `api/modalities`).subscribe(result => {
      this.modalities = result;
    }, error => console.error(error));
  }

  /**
   * Parses HttpErrorResponse and returns error messages from it
   * @param errorResponse
   */
  private getErrorMessageFromResponse(errorResponse: HttpErrorResponse): string {
    let errorMessage = "An unexpected error occurred.";
    if (errorResponse.error?.detail) {
      errorMessage = errorResponse.error.detail;
    }
    else if (errorResponse.error?.errors) {
      errorMessage = Object.entries(errorResponse.error.errors).map(([key, value]) => `${key}: ${value}`).join("<br/>");
    }
    else if (errorResponse.message) {
      errorMessage = errorResponse.message;
    }
    return errorMessage;
  }
}

interface Client {
  orgCode: string;
  name: string;
  sites: ClientSite[];
}

interface ClientSite {
  id: number;
  name: string;
}

interface Modality {
  code: string;
  name: string;
}
