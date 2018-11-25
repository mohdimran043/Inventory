import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'topnav',
  templateUrl: './topnav.component.html',
  styleUrls: ['./topnav.component.css']
})
export class TopnavComponent implements OnInit {
  constructor() { }
  public contentstr:any;

  ngOnInit() {
    this.contentstr = "Number of Vehicles on the Screen: 1           Vehicle: 26470      Vehicle: 26470, Time: 7:38:12 AM, Speed: 0 km/h, Address: Ad Dawha, Qatar ";

  }

}
