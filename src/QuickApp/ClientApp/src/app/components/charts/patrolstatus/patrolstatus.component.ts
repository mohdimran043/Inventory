import { Component, OnInit, AfterViewInit, Input, OnChanges, SimpleChanges ,ViewChild } from '@angular/core';
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
export class PatrolstatusComponent implements OnInit, OnChanges  {


  @Input('data')
  ahwalId: string;

  @ViewChild(BaseChartDirective) chart: BaseChartDirective;
  // Doughnut
  public doughnutChartLabels: string[] = ['Walking', 'Long break', 'Short break', 'Available'];
  public doughnutChartData: number[] = [2, 6, 34, 12];
  public doughnutChartType = 'doughnut';
  public doughnutChartOptions: any = {
    scaleShowVerticalLines: false,
    responsive: true,
    title: {
      text: 'Patrol Status',
      display: true
    }
  };
  constructor(private svc: CommonService, public configurations: ConfigurationService, private alertService: AlertService,
    private modalService: ModalService) {
  }

  ngOnInit() {
  }
  ngOnChanges(changes: SimpleChanges) {
  }
  LoadData() {
    this.svc.GetPatrolStatusChart(this.ahwalId).subscribe(resp => { console.log(resp); }, error => { });
  }
  public chartClicked(e: any): void {
    console.log(e);
  }
  public chartHovered(e: any): void {
    console.log(e);
  }
}
