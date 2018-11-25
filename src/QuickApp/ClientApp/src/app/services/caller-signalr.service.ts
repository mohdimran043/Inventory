import { EventEmitter, Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { CallerInfo } from '../Models/caller.model';
import { debounce } from 'rxjs/operators';

@Injectable()
export class SignalRService {
  startCallEventReceived = new EventEmitter<CallerInfo>();
  endCallEventRecieved = new EventEmitter<Boolean>();
  connectionEstablished = new EventEmitter<Boolean>();

  private connectionIsEstablished = false;
  private _hubConnection: HubConnection;

  constructor() {
    this.createConnection();
    this.registerOnServerEvents();
    this.startConnection();
  }

  invokeUpdateGrids(message: CallerInfo) {
    this._hubConnection.invoke('UpdateGrids', message);
  }

  private createConnection() {
    //console.log(window.location.protocol +"//"+ window.location.host + '/CallerHub');
    //  var url = 'http://localhost:2021/CallerHub';
    this._hubConnection = new HubConnectionBuilder()
      .withUrl(window.location.protocol + "//" + window.location.host + '/CallerHub')
      .build();
  }

  private startConnection(): void {
    this._hubConnection
      .start()
      .then(() => {
        this.connectionIsEstablished = true;
        console.log('Hub connection started');
        this.connectionEstablished.emit(true);
      })
      .catch(err => {
        console.log('Error while establishing connection, retrying...');
        setTimeout(this.startConnection(), 5000);
      });
  }

  private registerOnServerEvents(): void {

    this._hubConnection.on('startcallevent', (data: any) => {
      this.startCallEventReceived.emit(data);
    });
    this._hubConnection.on('endcallevent', (data: any) => {
      this.endCallEventRecieved.emit(true);
    });
  }
}  
