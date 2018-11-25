import { Component, OnInit, OnDestroy, Input, TemplateRef, ViewChild ,CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';

import { AuthService } from '../../services/auth.service';
import { AlertService, MessageSeverity, DialogType } from '../../services/alert.service';
import { AppTranslationService } from '../../services/app-translation.service';
import { LocalStoreManager } from '../../services/local-store-manager.service';
import { Utilities } from '../../services/utilities';


@Component({
  selector: 'app-map-left-nav',
  templateUrl: './map-left-nav.component.html',
  styleUrls: ['./map-left-nav.component.css']
})
export class MapLeftNavDemoComponent implements OnInit, OnDestroy {

  _currentUserId: string;
  menuState = 'out';
  get currentUserId() {
    if (this.authService.currentUser) {this._currentUserId = this.authService.currentUser.id; }
    return this._currentUserId;
  }
  constructor(private alertService: AlertService, private translationService: AppTranslationService,
     private localStorage: LocalStoreManager, private authService: AuthService) {
  }
  ngOnInit() {
  }

  ngOnDestroy() {
  }

}
