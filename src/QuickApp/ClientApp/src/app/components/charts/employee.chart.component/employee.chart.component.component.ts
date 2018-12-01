import { Component, OnInit, AfterViewInit, Input, OnChanges, SimpleChanges, ViewChild } from '@angular/core';
import { fadeInOut } from '../../../services/animations';
import { ConfigurationService } from '../../../services/configuration.service';
import { CommonService } from '../../../services/common.service';
import { AlertService, DialogType, MessageSeverity } from '../../../services/alert.service';
import { ModalService } from '../../../services/modalservice';
import { BaseChartDirective } from 'ng2-charts/ng2-charts';

@Component({
  selector: 'app-employee-chart',
  templateUrl: './employee.chart.component.component.html',
  styleUrls: ['./employee.chart.component.component.css'],
  animations: [fadeInOut]
})
export class EmployeeChartComponentComponent implements OnInit, OnChanges {


  @Input('data')
  ahwalId: string; 
  @ViewChild(BaseChartDirective) chart: BaseChartDirective;
  public barChartOptions: any = {
    scaleShowVerticalLines: false,
    responsive: true,
    title: {
      text: 'Employee Charts',
      display: true
    }
  };

  public barChartLabels: string[] = ['Section1','Section2'];
  public barChartType = 'bar';

  public barChartLegend = true;

  public barChartData: any[] = [
    { data: [100, 200, 49], label: 'On Duty', backgroundColor: ['#ff6384'] },
    { data: [1, 4, 5], label: 'Leave', backgroundColor: ['#ff6384'] }
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

  //public printstat() {
  //  const printContent = document.getElementById('barChartDataid');
  //  const WindowPrt = window.open('', '', 'left=0,top=0,width=900,height=900,toolbar=0,scrollbars=0,status=0');
  //  // tslint:disable-next-line:whitespace
  //  // WindowPrt.document.write('<br><img src=\'' + printContent.toDataURL() + '\'/>');
  //  WindowPrt.document.close();
  //  WindowPrt.focus();
  //  WindowPrt.print();
  //  WindowPrt.close();
  //}
  LoadData() {
    //alert('LoadData' + this.ahwalId); 
    this.svc.GetEmployeeStatsChart(this.ahwalId).subscribe(resp => {
      console.log(resp);
      this.barChartLabels = resp.chartlabel;     

      this.chart.chart.config.data.labels = resp.chartlabel;
      this.barChartData = resp.chartsubdta;
      
    }, error => { });
  }
  ngOnInit() {
    //alert('ngonint ' + this.ahwalId);
  }
  ngOnChanges(changes: SimpleChanges) {
    //alert(this.ahwalId);
  }
}
