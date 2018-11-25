// ====================================================

// Email: support@ebenmonney.com
// ====================================================

import { Injectable } from '@angular/core';
import { Router, NavigationExtras } from "@angular/router";
import { Observable, Subject } from 'rxjs';
import { map } from 'rxjs/operators';

import { LocalStoreManager } from './local-store-manager.service';
import { EndpointFactory } from './endpoint-factory.service';
import { ConfigurationService } from './configuration.service';
import { DBkeys } from './db-Keys';
import { JwtHelper } from './jwt-helper';
import { Utilities } from './utilities';
import { LoginResponse, IdToken } from '../models/login-response.model';
import { User } from '../models/user.model';
import { Permission, PermissionNames, PermissionValues } from '../models/permission.model';
import { LayoutStore } from '../../../node_modules/angular-admin-lte';


@Injectable()
export class AuthService {

  public get loginUrl() { return this.configurations.loginUrl; }
  public get homeUrl() { return this.configurations.homeUrl; }

  public loginRedirectUrl: string;
  public logoutRedirectUrl: string;

  public reLoginDelegate: () => void;

  private previousIsLoggedInCheck = false;
  private _loginStatus = new Subject<boolean>();


  constructor(public layoutStore: LayoutStore, private router: Router, private configurations: ConfigurationService, private endpointFactory: EndpointFactory, private localStorage: LocalStoreManager) {
    this.initializeLoginStatus();
  }


  private initializeLoginStatus() {
    this.localStorage.getInitEvent().subscribe(() => {
      this.reevaluateLoginStatus();
    });
  }


  gotoPage(page: string, preserveParams = true) {

    let navigationExtras: NavigationExtras = {
      queryParamsHandling: preserveParams ? "merge" : "", preserveFragment: preserveParams
    };


    this.router.navigate([page], navigationExtras);
  }


  redirectLoginUser() {
    let redirect = this.loginRedirectUrl && this.loginRedirectUrl != '/' && this.loginRedirectUrl != ConfigurationService.defaultHomeUrl ? this.loginRedirectUrl : this.homeUrl;
    this.loginRedirectUrl = null;


    let urlParamsAndFragment = Utilities.splitInTwo(redirect, '#');
    let urlAndParams = Utilities.splitInTwo(urlParamsAndFragment.firstPart, '?');

    let navigationExtras: NavigationExtras = {
      fragment: urlParamsAndFragment.secondPart,
      queryParams: Utilities.getQueryParamsFromString(urlAndParams.secondPart),
      queryParamsHandling: "merge"
    };

    this.router.navigate([urlAndParams.firstPart], navigationExtras);
  }


  redirectLogoutUser() {
    let redirect = this.logoutRedirectUrl ? this.logoutRedirectUrl : this.loginUrl;
    this.logoutRedirectUrl = null;

    this.router.navigate([redirect]);
  }


  redirectForLogin() {
    this.loginRedirectUrl = this.router.url;
    this.router.navigate([this.loginUrl]);
  }


  reLogin() {

    this.localStorage.deleteData(DBkeys.TOKEN_EXPIRES_IN);

    if (this.reLoginDelegate) {
      this.reLoginDelegate();
    }
    else {
      this.redirectForLogin();
    }
  }


  refreshLogin() {
    return this.endpointFactory.getRefreshLoginEndpoint<LoginResponse>().pipe(
      map(response => this.processLoginResponse(response, this.rememberMe)));
  }


  login(userName: string, password: string, rememberMe?: boolean) {

    if (this.isLoggedIn)
      this.logout();

    return this.endpointFactory.getLoginEndpoint<LoginResponse>(userName, password).pipe(
      map(response => this.processLoginResponse(response, rememberMe)));
  }


  private processLoginResponse(response: LoginResponse, rememberMe: boolean) {

    let accessToken = response.access_token;

    if (accessToken == null)
      throw new Error("Received accessToken was empty");

    let idToken = response.id_token;
    let refreshToken = response.refresh_token || this.refreshToken;
    let expiresIn = response.expires_in;

    let tokenExpiryDate = new Date();
    tokenExpiryDate.setSeconds(tokenExpiryDate.getSeconds() + expiresIn);

    let accessTokenExpiry = tokenExpiryDate;

    let jwtHelper = new JwtHelper();
    let decodedIdToken = <IdToken>jwtHelper.decodeToken(response.id_token);


    //console.log(decodedIdToken)
    let permissions: PermissionValues[] = Array.isArray(decodedIdToken.permission) ? decodedIdToken.permission : [decodedIdToken.permission];

    if (!this.isLoggedIn)
      this.configurations.import(decodedIdToken.configuration);

    let user = new User(
      decodedIdToken.sub,
      decodedIdToken.name,
      decodedIdToken.fullname,
      decodedIdToken.email,
      decodedIdToken.jobtitle,
      decodedIdToken.phone,
      Array.isArray(decodedIdToken.role) ? decodedIdToken.role : [decodedIdToken.role]);
    user.isEnabled = true;

    this.saveUserDetails(user, permissions, accessToken, idToken, refreshToken, accessTokenExpiry, rememberMe);
    this.loadLeftNavigation();
    this.reevaluateLoginStatus(user);

    return user;
  }

  public loadLeftNavigation() {
    let sidebarLeftMenu = [
      { label: 'Home', route: 'Home', iconClasses: 'fa fa-road' },
      {
        label: 'الصيانه', route: 'maintainence/patrolcars', iconClasses: 'fa fa-th-list', children: [
          { label: 'الدوريات', route: 'maintainence/patrolcars', iconClasses: 'fa fa-automobile' },
          { label: 'تقارير الاستلام والتسليم الأجهزة', route: 'maintainence/patrolcarsinventory', iconClasses: 'fa fa-calendar' },
          { label: 'الأجهزة', route: 'maintainence/handhelds', iconClasses: 'fa fa-fax' },
          { label: 'تقارير الاستلام والتسليم الدوريات', route: 'maintainence/handheldsinventory', iconClasses: 'fa fa-calendar' }
        ]
      },
      {
        label: 'الأحوال', route: 'dispatcher/dispatcher', iconClasses: 'fa fa-eye', children: [
          { label: 'كشف التوزيع', route: 'dispatcher/dispatcher', iconClasses: 'fa fa-calendar' }
        ]
      },
      {
        label: 'العمليات', route: 'operations/operationsopslive', iconClasses: 'fa fa-arrows', children: [
          { label: 'الكشف', route: 'operations/operationsopslive', iconClasses: 'fa fa-calendar' },
          { label: 'البلاغات', route: 'operations/incidents', iconClasses: 'fa fa-user-secret' },
          { label: 'Incident Type', route: 'operations/incidenttype', iconClasses: 'fa fa-user-secret' }

        ]
      }
    ];
    //setTimeout(() => {
    //  this.layoutStore.setSidebarLeftMenu(sidebarLeftMenu);
    //}, 1);
    this.layoutStore.setSidebarLeftMenu(sidebarLeftMenu);
  }
  private saveUserDetails(user: User, permissions: PermissionValues[], accessToken: string, idToken: string, refreshToken: string, expiresIn: Date, rememberMe: boolean) {

    if (rememberMe) {
      this.localStorage.savePermanentData(accessToken, DBkeys.ACCESS_TOKEN);
      this.localStorage.savePermanentData(idToken, DBkeys.ID_TOKEN);
      this.localStorage.savePermanentData(refreshToken, DBkeys.REFRESH_TOKEN);
      this.localStorage.savePermanentData(expiresIn, DBkeys.TOKEN_EXPIRES_IN);
      this.localStorage.savePermanentData(permissions, DBkeys.USER_PERMISSIONS);
      this.localStorage.savePermanentData(user, DBkeys.CURRENT_USER);
    }
    else {
      this.localStorage.saveSyncedSessionData(accessToken, DBkeys.ACCESS_TOKEN);
      this.localStorage.saveSyncedSessionData(idToken, DBkeys.ID_TOKEN);
      this.localStorage.saveSyncedSessionData(refreshToken, DBkeys.REFRESH_TOKEN);
      this.localStorage.saveSyncedSessionData(expiresIn, DBkeys.TOKEN_EXPIRES_IN);
      this.localStorage.saveSyncedSessionData(permissions, DBkeys.USER_PERMISSIONS);
      this.localStorage.saveSyncedSessionData(user, DBkeys.CURRENT_USER);
    }

    this.localStorage.savePermanentData(rememberMe, DBkeys.REMEMBER_ME);
  }



  logout(): void {
    this.localStorage.deleteData(DBkeys.ACCESS_TOKEN);
    this.localStorage.deleteData(DBkeys.ID_TOKEN);
    this.localStorage.deleteData(DBkeys.REFRESH_TOKEN);
    this.localStorage.deleteData(DBkeys.TOKEN_EXPIRES_IN);
    this.localStorage.deleteData(DBkeys.USER_PERMISSIONS);
    this.localStorage.deleteData(DBkeys.CURRENT_USER);

    this.configurations.clearLocalChanges();

    this.reevaluateLoginStatus();
  }


  private reevaluateLoginStatus(currentUser?: User) {

    let user = currentUser || this.localStorage.getDataObject<User>(DBkeys.CURRENT_USER);
    let isLoggedIn = user != null;

    if (this.previousIsLoggedInCheck != isLoggedIn) {
      setTimeout(() => {
        this._loginStatus.next(isLoggedIn);
      });
    }

    this.previousIsLoggedInCheck = isLoggedIn;
  }


  getLoginStatusEvent(): Observable<boolean> {
    return this._loginStatus.asObservable();
  }


  get currentUser(): User {

    let user = this.localStorage.getDataObject<User>(DBkeys.CURRENT_USER);
    this.reevaluateLoginStatus(user);

    return user;
  }

  get userPermissions(): PermissionValues[] {
    return this.localStorage.getDataObject<PermissionValues[]>(DBkeys.USER_PERMISSIONS) || [];
  }

  get accessToken(): string {

    this.reevaluateLoginStatus();
    return this.localStorage.getData(DBkeys.ACCESS_TOKEN);
  }

  get accessTokenExpiryDate(): Date {

    this.reevaluateLoginStatus();
    return this.localStorage.getDataObject<Date>(DBkeys.TOKEN_EXPIRES_IN, true);
  }

  get isSessionExpired(): boolean {

    if (this.accessTokenExpiryDate == null) {
      return true;
    }

    return !(this.accessTokenExpiryDate.valueOf() > new Date().valueOf());
  }


  get idToken(): string {

    this.reevaluateLoginStatus();
    return this.localStorage.getData(DBkeys.ID_TOKEN);
  }

  get refreshToken(): string {

    this.reevaluateLoginStatus();
    return this.localStorage.getData(DBkeys.REFRESH_TOKEN);
  }

  get isLoggedIn(): boolean {
    return this.currentUser != null;
  }

  get rememberMe(): boolean {
    return this.localStorage.getDataObject<boolean>(DBkeys.REMEMBER_ME) == true;
  }
}
