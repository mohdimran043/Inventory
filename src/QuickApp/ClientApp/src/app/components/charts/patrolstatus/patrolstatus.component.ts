import { Component, OnInit ,Input} from '@angular/core';
import { fadeInOut } from '../../../services/animations';

@Component({
  selector: 'app-patrolstatus',
  templateUrl: './patrolstatus.component.html',
  styleUrls: ['./patrolstatus.component.css'],
  animations: [fadeInOut]
})
export class PatrolstatusComponent implements OnInit {


  @Input()
  ahwalId: string;


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
  constructor() { }

  ngOnInit() {
  }
  public chartClicked(e: any): void {
    console.log(e);
  }
  public chartHovered(e: any): void {
    console.log(e);
  }
}
