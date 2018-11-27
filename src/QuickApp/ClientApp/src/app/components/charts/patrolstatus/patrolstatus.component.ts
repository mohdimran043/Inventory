import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-patrolstatus',
  templateUrl: './patrolstatus.component.html',
  styleUrls: ['./patrolstatus.component.css']
})
export class PatrolstatusComponent implements OnInit {
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
