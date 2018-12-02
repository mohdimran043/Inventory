import { Component, OnInit, AfterViewInit, Input, OnChanges, SimpleChanges ,ViewChild} from '@angular/core';
import { fadeInOut } from '../../../services/animations';
import { ConfigurationService } from '../../../services/configuration.service';
import { CommonService } from '../../../services/common.service';
import { AlertService, DialogType, MessageSeverity } from '../../../services/alert.service';
import { ModalService } from '../../../services/modalservice';
import { BaseChartDirective } from 'ng2-charts/ng2-charts';
@Component({
  selector: 'app-deviceavailability',
  templateUrl: './deviceavailability.component.html',
  styleUrls: ['./deviceavailability.component.css'],
  animations: [fadeInOut]
})
export class DeviceavailabilityComponent implements OnInit, OnChanges {

  @Input('data')
  ahwalId: string;
  @ViewChild(BaseChartDirective) chart: BaseChartDirective;

  public barChartOptions: any = {
    scaleShowVerticalLines: false,
    responsive: true,
    title: {
      text: 'Device availablity',
      display: true
    }
  };
  public barChartLabels: string[] = ['Sector1', 'Sector2', 'Sector3'];
  public barChartType: string = 'bar';
  public barChartLegend: boolean = true;

  public barChartData: any[] = [
    { data: [100, 200, 49], label: 'Available', backgroundColor: ['#ff6384'] },
    { data: [1, 4, 5], label: 'Not available', backgroundColor: ['#ff6384'] }
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
    //this.svc.GetDeviceAvailabilityChart(this.ahwalId).subscribe(resp => { console.log(resp); }, error => { });
  }
  ngOnInit() {
  }
  ngOnChanges(changes: SimpleChanges) {
  }
}
