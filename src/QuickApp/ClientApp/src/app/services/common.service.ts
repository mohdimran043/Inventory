import { Injectable, Output, EventEmitter, Injector } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError, retry, map, tap } from 'rxjs/operators';
import { patrolcars } from '../models/patrolcars';
import { handhelds } from '../models/handhelds';
import { accessorycls } from '../models/accessorycls';
import { persons } from '../models/persons';
import { citygroups } from '../models/citygroups';
import { ahwalmapping } from '../models/ahwalmapping';
import { user } from '../models/user';

import { EndpointFactory } from './endpoint-factory.service';
import { ConfigurationService } from './configuration.service';
@Injectable()

export class CommonService extends EndpointFactory {
  private api_url: any = document.getElementsByTagName('base')[0].href;

  constructor(http: HttpClient, configurations: ConfigurationService, injector: Injector) {
    super(http, configurations, injector);
  }
  //#region "PatrolCars"
  public GetPatrolCarList(ahwal: number, userid: number) {
    return this.http.post(this.api_url + '/api/maintainence/patrolcarslist',
      ahwal, { responseType: 'text' });
  }

  public AddPatrolCar(frm: patrolcars) {
    return this.http.post(this.api_url + '/api/maintainence/addpatrolcar',
      frm, { responseType: 'text' });
  }

  public UpdatePatrolCar(frm: patrolcars) {
    return this.http.post(this.api_url + '/api/maintainence/updatepatrolcar',
      frm, { responseType: 'text' });
  }

  public DeletePatrolCar(frm: patrolcars) {
    return this.http.post(this.api_url + '/api/maintainence/delpatrolcar',
      frm, { responseType: 'text' });
  }

  public GetPatrolCarTypes() {
    return this.http.post(this.api_url + '/api/maintainence/patrolcartypes',
      null, { responseType: 'text' });
  }

  public GetpatrolcarsInventoryList(rqhdr: object) {
    return this.http.post(this.api_url + '/api/maintainence/patrolcarsinventory', rqhdr, { responseType: 'json' });
  }

  //#endregion "PatrolCars"
  //#region "Hand Held"
  public GethandheldsList(rqhdr: object) {
    return this.http.post(this.api_url + '/api/maintainencehandheld/handheldlist',
      rqhdr, { responseType: 'json' });
  }
  public Addhandhelds(rqhdr: object) {
    return this.http.post(this.api_url + '/api/maintainencehandheld/addhandheld', rqhdr, { responseType: 'text' });
  }

  public Updatehandhelds(rqhdr: object) {
    return this.http.post(this.api_url + '/api/maintainencehandheld/addhandheld', rqhdr, { responseType: 'text' });
  }

  public Deletehandhelds(rqhdr: object) {
    return this.http.post(this.api_url + '/api/maintainencehandheld/delhandheld', rqhdr, { responseType: 'text' });
  }

  public GetHandHeldsInventoryList(rqhdr: object) {
    return this.http.post(this.api_url + '/api/maintainencehandheld/handheldinventory', rqhdr, { responseType: 'json' });
  }
  //#endregion "Hand Held"

  //#region "Accessory"
  public GetaccessoryList() {
    return this.http.post(this.api_url + '/api/maintainence/accessorylist', null,
      { responseType: 'text' });
  }
  public Addaccessory(frm: accessorycls) {
    return this.http.post(this.api_url + '/api/maintainence/addaccessory', frm,
      { responseType: 'text' });
  }

  public Updateaccessory(frm: accessorycls) {
    return this.http.post(this.api_url + '/api/maintainence/updateaccessory', frm,
      { responseType: 'text' });
  }

  public Deleteaccessory(frm: accessorycls) {
    console.log(frm);
    return this.http.post(this.api_url + '/api/maintainence/delaccessory',
      frm, { responseType: 'text' });
  }

  public GetAccessoryInventoryList() {
    return this.http.post(this.api_url + '/api/maintainence/accessoryinventory',
      null, { responseType: 'text' });
  }
  //#endregion "Accessory"

  //#region "Persons"
  public GetpersonList() {
    return this.http.post(this.api_url + '/api/maintainence/PersonsList',
      null, { responseType: 'text' });
  }
  public Addpersons(frm: persons) {
    return this.http.post(this.api_url + '/api/maintainence/addpersons',
      frm, { responseType: 'text' });
  }

  public Updatepersons(frm: persons) {
    return this.http.post(this.api_url + '/api/maintainence/updatepersons',
      frm, { responseType: 'text' });
  }

  public Deletepersons(frm: persons) {
    console.log(frm);
    return this.http.post(this.api_url + '/api/maintainence/delpersons', frm,
      { responseType: 'text' });
  }

  //#endregion "Persons"

  //#region "Dispatch"



  public GetPersonList(rqhdr: object) {

    return this.http.post(this.api_url + '/api/dispatcher/personsList', rqhdr, { responseType: 'json' });


  }




  public GetResponsibiltyList() {
    return this.http.get(this.api_url + '/api/dispatch/rolesList', { responseType: 'json' })

  }


  public GetSectorsList(rqhdr: object) {
    return this.http.post(this.api_url + '/api/dispatcher/sectorsList', rqhdr,
      { responseType: 'json' });

  }

  public GetCityList(rqhdr: object) {
    return this.http.post(this.api_url + '/api/dispatcher/cityList', rqhdr, { responseType: 'json' });

  }

  public GetAssociateList(rqhdr: object) {
    return this.http.post(this.api_url + '/api/dispatcher/associateList', rqhdr, { responseType: 'json' });

  }

  public GetPersonForUserForRole(mno: number, userid: number) {
    return this.http.get(this.api_url + '/api/dispatch/personForUserForRole?mno=' +
      mno + '&userid=' + userid, { responseType: 'json' });

  }

  public GetCityGroupForAhwal(ahwalid: number, sectorid?: number) {
    return this.http.get(this.api_url + '/api/dispatch/cityGroupforAhwal?ahwalid=' +
      ahwalid + '&sectorid=' + sectorid, { responseType: 'json' });

  }

  /*     public AddAhwalMapping2(ahwalmappingobj:ahwalmapping,userobj:user)
      {
        console.log('ahwalmappingobj' + ahwalmappingobj);
        let myData = {
          ahwalmappingobj:ahwalmappingobj,
          userobj:userobj
        };

        return this.http.post(this.api_url + '/api/dispatch/addAhwalMapping' ,
        myData,{ responseType: 'json' });

} */

  /*   public UpDateAhwalMapping(ahwalmappingobj:ahwalmapping)
    {
      return this.http.post(this.api_url + '/api/dispatch/updateAhwalMapping' , ahwalmappingobj , { responseType: 'text' });
    } */
  public GetMappingByID(AssociateMapId: number, userid: number) {
    return this.http.get(this.api_url + '/api/dispatch/mappingByID?associateMapID=' + AssociateMapId
      + '&userid=' + userid, { responseType: 'json' });

  }

  public GetPatrolCarByPlateNumberForUserForRole(CheckInOutPatrol: number, userid: number) {
    return this.http.get(this.api_url + '/api/dispatch/patrolCarByPlateNumberForUserForRole?CheckInOutPatrol=' +
      CheckInOutPatrol + '&userid=' + userid, { responseType: 'json' })

  }

  public GetHandHeldBySerialForUserForRole(CheckInOutHandHeld: number, userid: number) {
    return this.http.get(this.api_url + '/api/dispatch/handHeldBySerialForUserForRole?CheckInOutHandHeld=' +
      CheckInOutHandHeld + '&userid=' + userid, { responseType: 'json' });

  }

  public GetMappingByPersonID(CheckInOutPerson: number, userid: number) {
    return this.http.get(this.api_url + '/api/dispatch/mappingByPersonID?CheckInOutPerson=' +
      CheckInOutPerson + '&userid=' + userid, { responseType: 'json' });

  }
  /*  public DeleteAhwalMapping(ahwalmappingid:number,userid:number)
   {
     return this.http.delete(this.api_url + '/api/dispatch/deleteAhwalMapping?ahwalmappingid=' +
     ahwalmappingid + '&userid=' + userid , { responseType: 'json' });

} */
  //#endregion "Dispatch"

  public GetDeviceTypesList() {

    return this.http.post(this.api_url + '/api/maintainence/devicetypeslist',
      null, { responseType: 'text' });
  }



  /* public updatePersonState(selmenu:string,ahwalmappingid:number,userid:number)
          {
            return this.http.put(this.api_url + '/api/dispatch/updatePersonState?selmenu='+ selmenu + '&ahwalmappingid=' +
            ahwalmappingid + '&userid=' + userid , { responseType: 'json' });
  
          } */

  public GetCheckinPatrolCarList(ahwalid: number, userid: number) {
    return this.http.get(this.api_url + '/api/maintainence/checkinpatrolcarslist?ahwalid=' +
      ahwalid + '&userid=' + userid, { responseType: 'json' });
  }

  public GetCheckinHandHeldList(ahwalid: number, userid: number) {
    return this.http.get(this.api_url + '/api/maintainencehandheld/checkinhandheldslist?ahwalid=' + ahwalid +
      '&userid=' + userid, { responseType: 'json' });
  }

  public CheckInAhwalMapping(rqhdr: object) {


    return this.http.post(this.api_url + '/api/dispatcher/checkInAhwalMapping', rqhdr, { responseType: 'text' });
  }

  public AddAhwalMapping(rqhdr: object) {
    return this.http.post(this.api_url + '/api/dispatcher/addAhwalMapping', rqhdr, { responseType: 'text' });
  }

  public updatePersonState(rqhdr: object) {
    return this.http.post(this.api_url + '/api/dispatcher/updatePersonState', rqhdr, { responseType: 'text' });

  }

  public DeleteAhwalMapping(rqhdr: object) {
    return this.http.post(this.api_url + '/api/dispatcher/deleteAhwalMapping', rqhdr, { responseType: 'text' });
  }

  public GetAhwalPersonStates(ahwalmappingid: number) {
    return this.http.post(this.api_url + '/api/dispatcher/ahwalPersonStates', ahwalmappingid, { responseType: 'json' });
  }

  public GetDispatchList(rqhdr: object) {
    return this.http.post(this.api_url + '/api/dispatcher/dispatchList', rqhdr, { responseType: 'json' });
  }
  //#endregion "Dispatch"

  //#region "Generic"
  public GetAhwalList(userid: number) {
    return this.http.post(this.api_url + '/api/generic/ahwalList', userid, { responseType: 'text' });
  }

  public GetOrganizationList(userid: number) {
    return this.http.post(this.api_url + '/api/maintainence/organizationlist', userid, { responseType: 'text' });
  }

  public GetShiftsList() {
    return this.http.get(this.api_url + '/api/generic/shiftsList', { responseType: 'json' });

  }

  public GetCheckInOutStatesList() {
    return this.http.post(this.api_url + '/api/generic/checkinoutstates', null, { responseType: 'json' });
  }
  //#endregion "Generic"

  public GetIncidentsList() {
    return this.http.post(this.api_url + '/api/operations/operationslist', null, { responseType: 'json' });
  }

  public GetIncident_View(rqhdr: object) {
    return this.http.post(this.api_url + '/api/operations/incidentview', rqhdr, { responseType: 'json' });
  }
  public GetIncidentSourceList() {
    return this.http.post(this.api_url + '/api/operations/incidentsources', null, { responseType: 'json' });

  }

  public GetOpsLiveList(rqhr: object) {
    return this.http.post(this.api_url + '/api/operations/opslivelist', rqhr, { responseType: 'json' });
  }
  public ChangeOpsPersonState(rqhr: object) {
    return this.http.post(this.api_url + '/api/operations/changeopspersonstate', rqhr, { responseType: 'text' });
  }

  public AttachIncident(rqhr: object) {
    return this.http.post(this.api_url + '/api/operations/attachincident', rqhr, { responseType: 'text' });
  }

  public GetIncidentPopupSources() {
    return this.http.post(this.api_url + '/api/operations/incidentpopupsources', null, { responseType: 'json' });
  }

  public GetIncidentTypes() {
    return this.http.post(this.api_url + '/api/operations/incidenttypes', null, { responseType: 'json' });
  }
  public AddIncidentTypes(rqhdr: object) {
    return this.http.post(this.api_url + '/api/operations/addupdateincidenttypes', rqhdr, { responseType: 'text' });
  }
  public DeleteIncidentTypes(rqhdr: object) {
    return this.http.post(this.api_url + '/api/operations/deleteincidenttypes', rqhdr, { responseType: 'text' });
  }
  public Addincidents(rqhdr: object) {
    return this.http.post(this.api_url + '/api/operations/addincidents', null, { responseType: 'text' });
  }

  public GetIncidentById(rqhdr: object) {
    return this.http.post(this.api_url + '/api/operations/incidentbyid', rqhdr, { responseType: 'json' });
  }
}
