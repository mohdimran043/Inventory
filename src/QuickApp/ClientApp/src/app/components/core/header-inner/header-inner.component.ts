import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { AuthService } from "../../../services/auth.service";
import { AccountService } from '../../../services/account.service';
@Component({
  selector: 'app-header-inner',
  templateUrl: './header-inner.component.html'
})
export class HeaderInnerComponent implements OnInit {
  UserDisplayName: string;
  _accntService: AccountService;
  constructor(private accntService: AccountService) {
    this._accntService = accntService;
   
  }
  public LoadDisplayName(): void {
    if (typeof this._accntService.currentUser !== "undefined" && this._accntService.currentUser != null) {
      this.UserDisplayName = this._accntService.currentUser.empDisplayName;
    }
  }
  ngOnInit(): void {
    this.LoadDisplayName();   
  }
}
