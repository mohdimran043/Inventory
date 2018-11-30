import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { AuthService } from "../../../services/auth.service";
import { AccountService } from '../../../services/account.service';
@Component({
  selector: 'app-header-inner',
  templateUrl: './header-inner.component.html'
})
export class HeaderInnerComponent implements OnInit {
  UserDisplayName: string;

  constructor(private accntService: AccountService) {
    if (typeof accntService.currentUser !== "undefined" && accntService.currentUser != null) {
      this.UserDisplayName = accntService.currentUser.empDisplayName;
    }

  }

  ngOnInit(): void {

  }
}
