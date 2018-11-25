import { Component, OnInit, AfterViewInit } from '@angular/core';
import { fadeInOut } from '../../../services/animations';
import { ConfigurationService } from '../../../services/configuration.service';

import { AlertService, DialogType, MessageSeverity } from '../../../services/alert.service';
import { ModalService } from '../../../services/modalservice';

@Component({
  selector: 'app-incident-chart',
  templateUrl: './incident.chart.component.component.html',
  styleUrls: ['./incident.chart.component.component.css'],
  animations: [fadeInOut]
})
export class IncidentChartComponentComponent implements OnInit {
  public barChartOptions: any = {
    scaleShowVerticalLines: false,
    responsive: true
  };
  public barChartLabels: string[] = ['Jan', 'Feb', 'Mar', 'May', 'Jun', 'July', 'Aug'];
  public barChartType: string = 'bar';
  public barChartLegend: boolean = true;

  public barChartData: any[] = [
    { data: [65, 59, 80, 81, 56, 55, 40], label: 'No Of Incidents' },
    { data: [28, 48, 40, 19, 40, 27, 12], label: 'Closed Incidents' }
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
    let data = [
      Math.round(Math.random() * 100),
      59,
      80,
      (Math.random() * 100),
      56,
      (Math.random() * 100),
      40];
    let clone = JSON.parse(JSON.stringify(this.barChartData));
    clone[0].data = data;
    this.barChartData = clone;

  }
  ngOnInit() {
  }

}
