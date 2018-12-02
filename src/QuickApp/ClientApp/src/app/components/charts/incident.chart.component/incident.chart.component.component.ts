import { Component, OnInit, AfterViewInit, Input, OnChanges, SimpleChanges, ViewChild } from '@angular/core';
import { fadeInOut } from '../../../services/animations';
import { ConfigurationService } from '../../../services/configuration.service';
import { CommonService } from '../../../services/common.service';
import { AlertService, DialogType, MessageSeverity } from '../../../services/alert.service';
import { ModalService } from '../../../services/modalservice';
import { BaseChartDirective } from 'ng2-charts/ng2-charts';
@Component({
  selector: 'app-incident-chart',
  templateUrl: './incident.chart.component.component.html',
  styleUrls: ['./incident.chart.component.component.css'],
  animations: [fadeInOut]
})
export class IncidentChartComponentComponent implements OnInit, OnChanges {


  @Input('data')
  ahwalId: string;
  @ViewChild(BaseChartDirective) chart: BaseChartDirective;
  public barChartOptions: any = {
    scaleShowVerticalLines: false,
    responsive: true,
    title: {
      text: 'احصائيات الحوادث',
      display: true
    }
  };
  public colors = [
    {
      backgroundColor: 'rgba(77,83,96,0.2)'
    },
    {
      backgroundColor: 'rgba(30, 169, 224, 0.8)'
    }
  ];
  public barChartLabels: string[] = [];
  public barChartType: string = 'bar';
  public barChartLegend: boolean = true;

  public barChartData: any[] = [
    { data: [], label: 'حادث' }
  ];

  constructor(private svc: CommonService, public configurations: ConfigurationService, private alertService: AlertService,
    private modalService: ModalService) {
  }
  // events
  public chartClicked(e: any): void {
    console.log(e);
  }

  public chartHovered(e: any): void {
    console.log(e);
  }
  LoadData() {
    this.svc.GetIncidentChart(parseInt(this.ahwalId)).subscribe(resp => {
      
      var chartObject = JSON.parse(resp);
      if (typeof chartObject !== "undefined" && chartObject != null) {
        this.barChartLabels = chartObject.chartlabel;
        this.chart.chart.config.data.labels = chartObject.chartlabel;
        this.barChartData = chartObject.chartsubdta;
      }

    }, error => { });
  }
  ngOnInit() {
    this.LoadData();
  }
  ngOnChanges(changes: SimpleChanges) {
  }
}
