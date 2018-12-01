

import { Component, AfterViewInit } from '@angular/core';
import { fadeInOut } from '../../services/animations';
import { ConfigurationService } from '../../services/configuration.service';

import { AlertService, DialogType, MessageSeverity } from '../../services/alert.service';
import { ModalService } from '../../services/modalservice';
import * as Prism from 'prismjs';


@Component({
  selector: 'home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  animations: [fadeInOut]
})
export class HomeComponent implements AfterViewInit {
  ahwalsrc: any;
  shiftssrc: any;
  selectedAhwalSrc: string;


  constructor(public configurations: ConfigurationService, private alertService: AlertService,
    private modalService: ModalService) {
    this.shiftssrc = JSON.parse(window.localStorage.getItem('Shifts'));
    this.ahwalsrc = JSON.parse(window.localStorage.getItem('Ahwals'));
  }

  ngAfterViewInit() {
    Prism.highlightAll();
  }
}
