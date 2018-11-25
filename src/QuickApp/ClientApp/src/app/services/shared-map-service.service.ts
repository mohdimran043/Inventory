import { Injectable , Output,EventEmitter} from '@angular/core';
import {HttpClient,HttpHeaders,HttpErrorResponse} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, retry, map, tap } from 'rxjs/operators';
import { driver } from '../components/driver/drivercls';


export class Qrystring{
  public Qry:string = encodeURI("SELECT * from driverinfo")
}



@Injectable({
  providedIn: 'root'
})
export class SharedMapServiceService {
  public DeviceId:any;
  public RngSimVal:any;
  
  public LngLat:any;
  private api_url:any;
  private httpOptions = {
    headers: new HttpHeaders({
      'Content-Type':  'text/html',
      'responseType': 'text' 
    })
  };

  private handleError(error: HttpErrorResponse) {
    if (error.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      console.error('An error occurred:', error.error.message);
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      console.error(
        `Backend returned code ${error.status}, ` +
        `body was: ${error.message}`);
    }
    // return an observable with a user-facing error message
    return throwError(
      'Something bad happened; please try again later.');
  };

  @Output() SimulateEvnt: EventEmitter<number> = new EventEmitter();
  @Output() PauseSimulateEvnt: EventEmitter<number> = new EventEmitter();
  @Output() ResumeSimulateEvnt: EventEmitter<number> = new EventEmitter();
  @Output() SimulateRouteEvnt: EventEmitter<number> = new EventEmitter();
  @Output() RangeSimEvnt: EventEmitter<number> = new EventEmitter();
  @Output() AllvehicleEvnt: EventEmitter<number> = new EventEmitter();
  @Output() RemoveAllvehicleEvnt: EventEmitter<number> = new EventEmitter();
  @Output() ShowReportEvnt: EventEmitter<string> = new EventEmitter();


  SimulateObsrv = this.SimulateEvnt.asObservable();
  PauseSimulateObsrv = this.PauseSimulateEvnt.asObservable();
  SimulateRouteObsrv = this.SimulateRouteEvnt.asObservable();
  ResumeimulateObsrv = this.ResumeSimulateEvnt.asObservable();
  RangeSimObsrv = this.RangeSimEvnt.asObservable();
  AllvehicleObsrv = this.AllvehicleEvnt.asObservable();
  RemoveAllvehicleObsrv = this.RemoveAllvehicleEvnt.asObservable();
  ShowReportObsrv = this.ShowReportEvnt.asObservable();

  constructor(private http: HttpClient) {
    this.api_url = document.getElementsByTagName('base')[0].href + 'api/map';
  }

 

  public getLatLng(frm: driver)
  {

    return this.http.post(this.api_url + "/DriverQry" ,frm, {responseType: 'text'});
   


  }

  public getElasticQuery() {
    return this.http.get(this.api_url + "/DriverElastic?w_clause=" + encodeURI("where 1=1 "), { responseType: 'text' });
  }

  public getvehiclelatlng(vehicles:any) {
    return this.http.get(this.api_url + "/AllVehiclesElastic?vehicles=" + encodeURI(vehicles), { responseType: 'text' });
  }

  public UploadFiles(frm: FormData) {

    return this.http.post(this.api_url, frm, { responseType: 'text' });


  }

  public Savedrivers(frm: driver) {
    console.log('savedrivers' + frm);
    return this.http.post(this.api_url + "/SaveDriverDtl", frm, { responseType: 'text' });
  }

  public AllVehicles(DeviceId: any) {
    this.DeviceId = DeviceId;
    console.log('AllVehicles' + DeviceId);
    this.AllvehicleEvnt.emit(DeviceId)
}

public RemoveAllVehicles(DeviceId: any) {
  this.DeviceId = DeviceId;
  console.log('RemoveAllVehicles' + DeviceId);
  this.RemoveAllvehicleEvnt.emit(DeviceId)
}
 
  public SimulateDevices(DeviceId: any) {
    this.DeviceId = DeviceId;
    console.log('Simulate Device Shared' + DeviceId);
    this.SimulateEvnt.emit(DeviceId)
}

public PausseSimulation(DeviceId: any) {
  this.DeviceId = DeviceId;
  console.log('Pause Device Shared ' + DeviceId);
  this.PauseSimulateEvnt.emit(DeviceId)
}

public ResumeSimulation(DeviceId: any) {
  this.DeviceId = DeviceId;
  console.log('Pause Device Shared ' + DeviceId);
  this.ResumeSimulateEvnt.emit(DeviceId)
}

public Simulate_By_Route_Device(DeviceId: any) {
  this.DeviceId = DeviceId;
  console.log('Pause Device Shared ' + DeviceId);
  this.SimulateRouteEvnt.emit(DeviceId)
}

public RngSlider_Simulate(RngSimVal: any) {
  this.RngSimVal = RngSimVal;
  console.log('Pause Device Shared ' + RngSimVal);
  this.RangeSimEvnt.emit(RngSimVal)
}


public GetDeviceRoute(FromDt:any,ToDt:any)
{
  console.log(FromDt);
  return  this.http.get(this.api_url + "/DeviceRoute?FromDt=" + FromDt + "&ToDt=" + ToDt, { responseType: 'text' });
}



}
