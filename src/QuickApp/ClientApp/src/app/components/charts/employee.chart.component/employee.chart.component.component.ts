import { Component, OnInit, AfterViewInit,Input } from '@angular/core';
import { fadeInOut } from '../../../services/animations';
import { ConfigurationService } from '../../../services/configuration.service';

import { AlertService, DialogType, MessageSeverity } from '../../../services/alert.service';
import { ModalService } from '../../../services/modalservice';

@Component({
  selector: 'app-employee-chart',
  templateUrl: './employee.chart.component.component.html',
  styleUrls: ['./employee.chart.component.component.css'],
  animations: [fadeInOut]
})
export class EmployeeChartComponentComponent implements OnInit {


  @Input()
  ahwalId: string;

  public barChartOptions: any = {
    scaleShowVerticalLines: false,
    responsive: true,
    title: {
      text: 'Employee Charts',
      display: true
    }
  };
  
  public barChartLabels: string[] = ['Section1', 'Section2', 'Section3'];
  public barChartType = 'bar';
  
  public barChartLegend  =true;

  public barChartData: any[] = [
    { data: [100, 200, 49], label: 'On Duty', backgroundColor: ['#ff6384'] },
    { data: [1, 4, 5], label: 'Leave', backgroundColor: ['#ff6384'] }
  ];

  constructor(public configurations: ConfigurationService, private alertService: AlertService,
    private modalService: ModalService) {
  }
  // events
  public chartClicked(e: any): void {
    console.log(e);
  }

  public chartHovered(e: any): void {
    console.log(e);
  }

  public randomize(): void {
    // Only Change 3 values
    const data = [
      Math.round(Math.random() * 100),
      59,
      80,
      (Math.random() * 100),
      56,
      (Math.random() * 100),
      40];
    const clone = JSON.parse(JSON.stringify(this.barChartData));
    clone[0].data = data;
    this.barChartData = clone;

  }
  public printstat() {
    const printContent = document.getElementById('barChartDataid');
    const WindowPrt = window.open('', '', 'left=0,top=0,width=900,height=900,toolbar=0,scrollbars=0,status=0');
    // tslint:disable-next-line:whitespace
    // WindowPrt.document.write('<br><img src=\'' + printContent.toDataURL() + '\'/>');
    WindowPrt.document.close();
    WindowPrt.focus();
    WindowPrt.print();
    WindowPrt.close();
  }  ngOnInit() {
  }

}
