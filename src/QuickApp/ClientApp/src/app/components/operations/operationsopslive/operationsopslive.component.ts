import { ElementRef, Component, OnInit, ViewChild } from '@angular/core';
import { CommonService } from '../../../services/common.service';
import { DxDataGridComponent, DxSelectBoxComponent } from 'devextreme-angular'
import notify from 'devextreme/ui/notify';
import { confirm } from 'devextreme/ui/dialog';
import { AlertService, DialogType, MessageSeverity } from '../../../services/alert.service';
import { ModalService } from '../../../services/modalservice';
import { handler_ahwalMapping } from '../../../../environments/handler_ahwalMapping';
import { ahwalmapping } from '../../../models/ahwalmapping';
import { citygroups } from '../../../models/citygroups';
import { persons } from '../../../models/persons';
import { patrolcars } from '../../../models/patrolcars';
import { handhelds } from '../../../models/handhelds';
import { associates } from '../../../models/associates';
import { sectors } from '../../../models/sectors';

import { patrolroles } from '../../../models/patrolroles';
import { user } from '../../../models/user';
import { operationLog } from '../../../models/operationLog';

import { handler_operations } from '../../../../environments/handler_operations';
import { HandheldinventoryComponent } from '../../maintainence/inventory/handheldinventory/handheldinventory.component';
import { Timestamp } from '../../../../../node_modules/rxjs';
import { handler_incident } from '../../../../environments/handler_incidents';
import { FormGroup, FormControl } from '@angular/forms';
const CircularJSON = require('circular-json');

@Component({
  selector: 'app-operationsopslive',
  templateUrl: './operationsopslive.component.html',
  styleUrls: ['./operationsopslive.component.css']
})
export class OperationsopsliveComponent implements OnInit {

  @ViewChild(DxDataGridComponent) dataGrid: DxDataGridComponent;
  @ViewChild('gridIncidents') incidentGrid: DxDataGridComponent;
  @ViewChild('gridIncidentPopup') gridIncidentPopup: DxDataGridComponent;



  namet: any;
  loadingVisible: boolean = false;
  incidentloadingVisible: boolean = false;
  selhdrAhwalId: number;
  selhdrShiftId: number;
  ahwalMapping_CheckInOut_StatusLabel: string;
  ahwalsrc: any;
  responsibilitysrc: patrolroles[];
  shiftssrc: any;
  sectorssrc: sectors[];
  citysrc: any;
  associatesrc: associates[];
  state_src: any;
  sectorid: number;
  personsrc: persons[];
  shiftvisibile: boolean = false;
  sectorvisibile: boolean = false;
  cityvisibile: boolean = false;
  associatevisibile: boolean = false;
  userid: number = null;
  selectedRole: string = null;
  selectedShift: string = null;
  ahwalMapping_Add_status_label: string = '';
  selectPerson_Mno: string = null;
  ahwalMappingAddMethod: string = '';
  selahwalmappingid: number = null;
  selectedSector: string = null;
  selectedAssociateMapId: string = null;
  selectedCity: string = null;
  checkInOutPopupVisible: any = false;
  statesPopupVisible: any = false;
  personname: string = '';
  associatePersonMno: number = null;
  menuOpen: boolean = false;
  userobj: user = new user();
  dataSource: any;
  styleExp: string = 'none';
  AhwalMapping_CheckInOut_ID: any;
  AhwalMapping_CheckInOut_Method: any;
  selCheckInOutPersonMno: number = null;
  selCheckInOutPatrolPltNo: number = null;
  selCheckInOutHHeldSerialNo: number = null;
  selStatePersonid: number;
  patrolCarsrc: patrolcars[];
  selRowIndex: number;
  handHeldsrc: handhelds[];
  incidentsrc: any;
  incidentsources: any;
  incidentdatasources: any;
  contextPopupVisible: any = false;
  mappopupVisible: any = false;
  myGroup = new FormGroup({
    address: new FormGroup({ debtor: new FormControl() })
  });
  constructor(private svc: CommonService, private modalService: ModalService, private alertService: AlertService) {
    this.userid = parseInt(window.localStorage.getItem('UserID'), 10);
    this.userobj.userID = this.userid;

    this.ahwalsrc = JSON.parse(window.localStorage.getItem('Ahwals'));
    this.selhdrAhwalId = this.ahwalsrc[0].ahwalid;
    this.shiftssrc = JSON.parse(window.localStorage.getItem('Shifts'));
    this.selhdrShiftId = this.shiftssrc[0].shiftid;
    this.showLoadPanel();
  }

  onShown() {
    setTimeout(() => {
      this.loadingVisible = false;
      this.incidentloadingVisible = false;

    }, 3000);
  }

  onIncidentShown() {
    setTimeout(() => {
      this.incidentloadingVisible = false;
    }, 3000);
  }

  showLoadPanel() {
    this.loadingVisible = true;
    this.incidentloadingVisible = true;
  }





  ngOnInit() {
    this.bindAhwalMappingGridSources();
    this.bindAhwalMappingGrid();
    this.bindIncidentGrid();

    setInterval(() => {
      //Added by imran to retain the state prior to refresh of the gridS
      this.dataGrid.instance.refresh();
      this.incidentGrid.instance.refresh();
    }, 10000);
  }

  roleSelection(e) {
    console.log(e);
    this.selectedRole = (e.value);

    if (e.value !== null) {
      if (parseInt(e.value, 10) === handler_ahwalMapping.PatrolRole_CaptainAllSectors ||
        parseInt(e.value, 10) === handler_ahwalMapping.PatrolRole_CaptainShift) {
        //this.shiftvisibile = true;
        this.sectorvisibile = false;
        this.cityvisibile = false;
        // this.searchInput.nativeElement.visible = false;
        this.associatevisibile = false;
      }
      else if (parseInt(e.value, 10) === handler_ahwalMapping.PatrolRole_CaptainSector ||
        parseInt(e.value, 10) === handler_ahwalMapping.PatrolRole_SubCaptainSector) {
        // this.shiftvisibile = true;
        this.sectorvisibile = true;
        this.cityvisibile = false;
        //this.searchInput.nativeElement.visible = false;
        this.associatevisibile = false;
      }
      else if (parseInt(e.value, 10) === handler_ahwalMapping.PatrolRole_Associate) {
        // this.shiftvisibile = false;
        this.sectorvisibile = false;
        this.cityvisibile = false;
        // this.searchInput.nativeElement.visible = false;
        this.associatevisibile = true;
      }
      else if (parseInt(e.value, 10) != -1 && parseInt(e.value, 10) != null) {
        // this.shiftvisibile = true;
        this.sectorvisibile = true;
        this.cityvisibile = true;
        // this.searchInput.nativeElement.visible = true;
        this.associatevisibile = false;
      }
    }

  }

  bindAhwalMappingGridSources() {

    this.svc.GetIncidentSourceList().toPromise().then(resp => {
      console.log(JSON.stringify(resp));
      this.incidentsources = resp;
    });


  }


  sectorSelection(e) {

    if (e.value !== null) {
      this.selectedSector = e.value;
      let rqhdr: object = {
        userid: this.userid,
        sectorid: this.selectedSector,
        ahwalid: this.selhdrAhwalId
      };

      this.svc.GetCityList(rqhdr).subscribe(resp => {
        this.citysrc = resp;
      });
    }
    else {
      this.citysrc = null;
    }
  }



  bindAhwalMappingGrid() {
    let rqhr: object = {
      AhwalId: this.selhdrAhwalId,
      ShiftId: this.selhdrShiftId
    };

    this.svc.GetOpsLiveList(rqhr).subscribe(resp => {

      this.dataSource = resp;
      // console.log('resp' + resp);
      this.dataGrid.dataSource = this.dataSource;
      this.dataGrid.instance.refresh();

    },
      error => {

      });
  }

  onToolbarPreparing(e) {

    e.toolbarOptions.items.unshift({
      location: 'before',
      template: 'الأحوال'
    }, {
        location: 'before',
        widget: 'dxSelectBox',
        options: {
          width: 200,
          dataSource: this.ahwalsrc,
          displayExpr: 'name',
          valueExpr: 'ahwalid',
          value: this.ahwalsrc[0].ahwalid,
          onValueChanged: this.ahwalChanged.bind(this)
        }
      }, {
        location: 'before',
        template: 'الشفت'
      }, {
        location: 'before',
        widget: 'dxSelectBox',
        options: {
          width: 200,
          dataSource: this.shiftssrc,
          displayExpr: 'name',
          valueExpr: 'shiftid',
          value: this.shiftssrc[0].shiftid,
          onValueChanged: this.shiftChanged.bind(this)

        }
      }
      , {
        location: 'after',
        widget: 'dxButton',
        options: {
          icon: 'refresh',
          onClick: this.refreshDataGrid.bind(this)
        }
      });
  }

  ahwalChanged(e) {
    this.selhdrAhwalId = e.value;
    this.bindAhwalMappingGrid();

  }

  shiftChanged(e) {
    this.selhdrShiftId = e.value;
    this.bindAhwalMappingGrid();

  }


  onEditorPreparing(e) {
    e.editorOptions.showClearButton = true;
    // console.log('editor' + JSON.stringify(e.editorOptions));

  }

  onCellPrepared(e) {
    //console.log(e);
  }

  checkvisibility(obj: any, personstateid: number) {
    //console.log(obj);
    //return false;
  }
  customBtnclick(personstate: any, ahwalmappingId: number) {
    console.log(JSON.stringify(personstate));
    let rqhr: object = {
      personstate: personstate,
      ahwalmappingId: ahwalmappingId,
      userid: this.userid
    };

    this.svc.ChangeOpsPersonState(rqhr).subscribe(resp => {
      notify(JSON.parse(resp).text, 'success', 900);
      if (JSON.parse(resp).statusid === handler_operations.Opeartion_Status_Success) {

        this.bindAhwalMappingGrid();
      }

    });
  }
  onRowPrepared(e) {

    if (e.rowType === 'data') {
      // console.log(e.Row);
      //console.log(e.rowElement.nativeElement);

      //console.log(e.cells[0]);
      // console.log(e.rowElement);
      //let searchbtn = $( e.data.ID).dxButton("instance");
      //console.log(searchbtn);

      //set default to white first
      e.rowElement.bgColor = "White";
      // e.rowElement.font = "Italic";
      //e.rowElement.css('background', 'green');
      // e.cells[0].cellElement.css("color", "red");
      // e.rowElement.color="red";
      //e.rowElement.Font.Bold = false;
      console.log(e.key.patrolpersonstateid);
      if (e.key.patrolpersonstateid === handler_ahwalMapping.PatrolPersonState_SunRise ||
        e.key.patrolpersonstateid === handler_ahwalMapping.PatrolPersonState_Sea ||
        e.key.patrolpersonstateid === handler_ahwalMapping.PatrolPersonState_Back ||
        e.key.patrolpersonstateid === handler_ahwalMapping.PatrolPersonState_BackFromWalking) {
        e.rowElement.bgColor = 'LightGreen';

      }
      if (e.key.patrolpersonstateid === handler_ahwalMapping.PatrolPersonState_Land) {
        e.rowElement.bgColor = 'LightGray';

      }
      if (e.key.patrolpersonstateid === handler_ahwalMapping.PatrolPersonState_Away) {
        e.rowElement.bgColor = 'Yellow';

      }
      if (e.key.patrolpersonstateid === handler_ahwalMapping.PatrolPersonState_Sick ||
        e.key.patrolpersonstateid === handler_ahwalMapping.PatrolPersonState_Absent ||
        e.key.patrolpersonstateid === handler_ahwalMapping.PatrolPersonState_Off) {
        e.rowElement.bgColor = 'PaleVioletRed';

      }
      if (e.key.patrolpersonstateid === handler_ahwalMapping.PatrolPersonState_WalkingPatrol) {
        e.rowElement.bgColor = 'CadetBlue';

      }
      if (e.key.patrolroleid === handler_ahwalMapping.PatrolRole_Associate) {
        e.rowElement.bgColor = 'SandyBrown';

      }


      if (e.key.incidentid !== null && e.key.incidentid !== '') {
        e.rowElement.bgColor = 'Red';

      }

      if (e.key.laststatechangetimestamp != null) {
        var lastTimeStamp = <Date>(e.key.laststatechangetimestamp);
        if (e.key.personState === handler_ahwalMapping.PatrolPersonState_Land) //max 1 hour
        {


          /*  var hours = (DateTime.Now - lastTimeStamp).TotalHours;
           if (hours >= 1)
           {
               e.Row.ForeColor = System.Drawing.Color.PaleVioletRed;
               e.Row.Font.Bold = true;
           } */
        }
        else if (e.key.personState == handler_ahwalMapping.PatrolPersonState_Away) //max 10 minues
        {
          /* var minutes = (DateTime.Now - lastTimeStamp).TotalMinutes;
          if (minutes >= 11)
          {
              e.Row.ForeColor = System.Drawing.Color.PaleVioletRed;
              e.Row.Font.Bold = true;
          } */
        }


      }

    }
  }



  onStatesRowPrepared(e) {
    if (e.rowType === 'data') {
      console.log(e);
      //set default to white first
      e.rowElement.bgColor = "White";
      // e.rowElement.font = "Italic";
      //e.rowElement.css('background', 'green');
      // e.cells[0].cellElement.css("color", "red");
      // e.rowElement.color="red";
      //e.rowElement.Font.Bold = false;

      if (e.key.patrolpersonstateid === handler_ahwalMapping.PatrolPersonState_SunRise ||
        e.key.patrolpersonstateid === handler_ahwalMapping.PatrolPersonState_Sea ||
        e.key.patrolpersonstateid === handler_ahwalMapping.PatrolPersonState_Back ||
        e.key.patrolpersonstateid === handler_ahwalMapping.PatrolPersonState_BackFromWalking) {
        e.rowElement.bgColor = 'LightGreen';

      }
      if (e.key.patrolpersonstateid === handler_ahwalMapping.PatrolPersonState_Land) {
        e.rowElement.bgColor = 'LightGray';

      }
      if (e.key.patrolpersonstateid === handler_ahwalMapping.PatrolPersonState_Away) {
        e.rowElement.bgColor = 'Yellow';

      }
      if (e.key.patrolpersonstateid === handler_ahwalMapping.PatrolPersonState_Sick || e.key.patrolpersonstateid === handler_ahwalMapping.PatrolPersonState_Absent || e.key.patrolpersonstateid === handler_ahwalMapping.PatrolPersonState_Off) {
        e.rowElement.bgColor = 'PaleVioletRed';

      }
      if (e.key.patrolpersonstateid === handler_ahwalMapping.PatrolPersonState_WalkingPatrol) {
        e.rowElement.bgColor = 'CadetBlue';

      }
      if (e.key.patrolroleid === handler_ahwalMapping.PatrolRole_Associate) {
        e.rowElement.bgColor = 'SandyBrown';

      }




    }
  }
  show_States_PopUp() {
    console.log('show_States_PopUp' + this.selahwalmappingid);
    this.statesPopupVisible = true;
    this.svc.GetAhwalPersonStates(this.selahwalmappingid).subscribe(resp => { this.state_src = resp; });

  }
  updatePersonState(selmenu: string) {
    if (this.selahwalmappingid !== null) {
      let rqhdr: object = {
        Selmenu: selmenu,
        AhwalMappingId: this.selahwalmappingid,
        userid: this.userid
      };
      this.svc.updatePersonState(rqhdr).subscribe(resp => {

        notify(resp, 'success', 600);
        this.bindAhwalMappingGrid();

      });


    }
  }
  deleteMapping() {
    console.log(this.selahwalmappingid);
    if (this.selahwalmappingid !== null) {
      let rqhdr: object = {

        AhwalMappingId: this.selahwalmappingid,
        userid: this.userid
      };

      this.svc.DeleteAhwalMapping(rqhdr).toPromise().then(resp => {

        notify(resp, 'success', 600);
        this.bindAhwalMappingGrid();
      });


    }
  }

  refreshDataGrid() {
    // this.dataGrid.instance.refresh();
    this.bindAhwalMappingGrid();
  }



  showmapInfo() {
    this.modalService.open('custom-modal-1');
  }

  citySelection(e) {
    this.selectedCity = e.value;
  }





  Rwclick(e) {
    console.log('dblclick');
    var component = e.component,
      prevClickTime = component.lastClickTime;
    component.lastClickTime = new Date();
    if (prevClickTime && (component.lastClickTime - prevClickTime < 300)) {
      console.log('dblclick1');
      this.selahwalmappingid = e.key.ahwalmappingid;
      this.show_States_PopUp();
    }
    else {
      console.log('dblclick2');
    }

  }


  bindIncidentGrid() {
    this.svc.GetIncidentsList().subscribe(resp => {

      this.incidentsrc = resp;
      this.incidentGrid.dataSource = this.incidentsrc;
      this.incidentGrid.instance.refresh();

    });
  }

  onIncidentRowPrepared(e) {

    if (e.rowType === 'data') {

      //set default to white first
      e.rowElement.bgColor = "White";


      if (e.key.incidentstateid === handler_incident.Incident_State_New) {
        e.rowElement.bgColor = 'Red';

      }
      if (e.key.incidentstateid === handler_incident.Incident_State_Closed) {
        e.rowElement.bgColor = 'LightGray';

      }
      if (e.key.incidentstateid === handler_incident.Incident_State_HasComments) {
        e.rowElement.bgColor = 'Yellow';

      }

    }
  }

  async  getIncidentImg(incidentId: number) {
    return "../../../../assets/img/NewUpdate.png";


  }

  onContextMenuprepare(e) {
    this.selahwalmappingid = e.row.key.ahwalmappingid;

    if (e.row.rowType === 'data') {
      e.items = [{
        text: 'تسليم بلاغ',
        value: e.row.rowIndex,
        onItemClick: this.ContextMenuClick.bind(this)

      }
      ];

    }
  }

  ContextMenuClick(e) {
    if (e.itemData.text === 'تسليم بلاغ') {
      this.contextPopupVisible = true
    }
  }
  selIncidentId: any;
  //gridIncPopupVisible:any;
  IncidentRwclick(e) {
    console.log(CircularJSON.stringify(e));
    this.selIncidentId = e.key.incidentid;
    //this.gridIncPopupVisible = false;
    console.log('hrd' + this.gridIncidentPopup);
    // this.gridIncidentPopup.nativeElement.instance.option('visible','false') ;
    // console.log(util.inspect(e) + 'click');
    console.log(this.selIncidentId);
    console.log(e.key.incidentsourceid);
  }

  AttahcIncidentSubmitButton_Click(e) {
    let rqhdr: object = {

      ahwalmappingid: this.selahwalmappingid,
      userid: this.userid,
      incidentid: this.selIncidentId
    };

    this.svc.AttachIncident(rqhdr).toPromise().then(resp => {

      notify(JSON.parse(resp).text, 'success', 600);
      this.bindAhwalMappingGrid();
    });
  }

  onIncToolbarPreparing(e) {

    e.toolbarOptions.items.unshift({
      location: 'before',
      template: 'الأحوال'
    }, {
        location: 'before',
        widget: 'dxSelectBox',
        options: {
          width: 200,
          dataSource: this.ahwalsrc,
          displayExpr: 'name',
          valueExpr: 'ahwalid',
          value: this.ahwalsrc[0].ahwalid,
          onValueChanged: this.ahwalChanged.bind(this)
        }
      }, {
        location: 'before',
        template: 'الشفت'
      }, {
        location: 'before',
        widget: 'dxSelectBox',
        options: {
          width: 200,
          dataSource: this.shiftssrc,
          displayExpr: 'name',
          valueExpr: 'shiftid',
          value: this.shiftssrc[0].shiftid,
          onValueChanged: this.shiftChanged.bind(this)

        }
      }
      , {
        location: 'after',
        widget: 'dxButton',
        options: {
          icon: 'refresh',
          onClick: this.refreshDataGrid.bind(this)
        }
      });
  }

}
