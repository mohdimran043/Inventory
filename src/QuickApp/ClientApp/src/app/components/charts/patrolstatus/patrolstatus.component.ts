import { Component, OnInit, AfterViewInit, Input, OnChanges, SimpleChanges, ViewChild } from '@angular/core';
import { fadeInOut } from '../../../services/animations';
import { ConfigurationService } from '../../../services/configuration.service';
import { CommonService } from '../../../services/common.service';
import { AlertService, DialogType, MessageSeverity } from '../../../services/alert.service';
import { ModalService } from '../../../services/modalservice';
import { BaseChartDirective } from 'ng2-charts/ng2-charts';
@Component({
  selector: 'app-patrolstatus',
  templateUrl: './patrolstatus.component.html',
  styleUrls: ['./patrolstatus.component.css'],
  animations: [fadeInOut]
})
export class PatrolstatusComponent implements OnInit, OnChanges {


  @Input('data')
  ahwalId: string;
  public chartColors: any[] = [
    {
      backgroundColor: ["#FF7360", "#6FC8CE", "#FAFFF2", "#FFFCC4", "#B9E8E0", "#F42A0A", "#95382A", "#97bbad", "#97bb0d"]
    }];
  @ViewChild(BaseChartDirective) chart: BaseChartDirective;
  // Doughnut
  public doughnutChartLabels: string[] = [];
  public doughnutChartData: number[] = [];
  public doughnutChartType = 'doughnut';
  public doughnutChartOptions: any = {
    scaleShowVerticalLines: false,
    responsive: true,
    title: {
      text: 'دولة باترول',
      display: true
    }
  };
  constructor(private svc: CommonService, public configurations: ConfigurationService, private alertService: AlertService,
    private modalService: ModalService) {
  }

  ngOnInit() {
    this.LoadData();
  }
  ngOnChanges(changes: SimpleChanges) {
  }
  LoadData() {
    this.svc.GetPatrolStatusChart(parseInt(this.ahwalId)).subscribe(resp => {
      console.log(resp);

      var chartObject = JSON.parse(resp);
      if (typeof chartObject !== "undefined" && chartObject != null) {
        //this.doughnutChartLabels = chartObject.chartlabel;
        
        //if (chartObject.chartsubdta.length > 0) {
        //  this.doughnutChartData = []
        //}
        //else {
        //  this.doughnutChartData = [];
        //}
        
        //this.chart.chart.config.data.labels = chartObject.chartlabel;
      }
    }, error => { });
  }
  public chartClicked(e: any): void {
    console.log(e);
  }
  public chartHovered(e: any): void {
    console.log(e);
  }
}
