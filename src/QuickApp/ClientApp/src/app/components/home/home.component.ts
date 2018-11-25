

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

 

  constructor(public configurations: ConfigurationService, private alertService: AlertService,
    private modalService: ModalService) {

  }

  ngAfterViewInit() {
    Prism.highlightAll();
  }
}
