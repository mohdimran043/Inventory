

import { Component, AfterViewInit, Output, EventEmitter,ViewChild} from '@angular/core';
import { fadeInOut } from '../../services/animations';
import { ConfigurationService } from '../../services/configuration.service';

import { AlertService, DialogType, MessageSeverity } from '../../services/alert.service';
import { ModalService } from '../../services/modalservice';
import * as Prism from 'prismjs';
import { alert } from 'devextreme/ui/dialog';
import { EmployeeChartComponentComponent } from '../charts/employee.chart.component/employee.chart.component.component';


@Component({
  selector: 'home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  animations: [fadeInOut]
})
export class HomeComponent implements AfterViewInit {
  ahwalsrc: any;
  shiftssrc: any;
  selectedAhwalSrc: string;
  @Output() emitAhwalChange = new EventEmitter<any[]>();
  @ViewChild(EmployeeChartComponentComponent) empComponet: EmployeeChartComponentComponent;

  constructor(public configurations: ConfigurationService, private alertService: AlertService,
    private modalService: ModalService) {
    this.shiftssrc = JSON.parse(window.localStorage.getItem('Shifts'));
    this.ahwalsrc = JSON.parse(window.localStorage.getItem('Ahwals'));
    if (typeof this.ahwalsrc !== "undefined" && this.ahwalsrc.length > 0) {
      this.selectedAhwalSrc = this.ahwalsrc[0].ahwalid;

    }
   
  }
  public ahwalChangeEvent(id) {
    console.log(id);
    this.empComponet.ahwalId = id;
    this.empComponet.LoadData();
  }
  ngAfterViewInit() {
    Prism.highlightAll();
  }
}
