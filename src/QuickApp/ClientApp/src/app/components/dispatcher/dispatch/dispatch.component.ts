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

@Component({
  selector: 'app-dispatch',
  templateUrl: './dispatch.component.html',
  styleUrls: ['./dispatch.component.css']
})



export class DispatchComponent implements OnInit {

  @ViewChild(DxDataGridComponent) dataGrid: DxDataGridComponent;



  loadingVisible: boolean = false;
  selhdrAhwalId: number;
  selhdrShiftId: number;

  ahwalMapping_CheckInOut_StatusLabel: string;
  ahwalsrc: any;
  responsibilitysrc: patrolroles[];
  shiftssrc: any;
  sectorssrc: any;
  citysrc: any;
  associatesrc: any;
  state_src: any;
  sectorid: number;
  personsrc: any;
  handHeldsrc: any;
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
  Heldsrc: handhelds[];


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
    }, 3000);
  }


  showLoadPanel() {
    this.loadingVisible = false;
  }



  loadSources() {
    this.bindAhwalMappingGridSources();
    this.bindAhwalMappingGrid();
  }

  ngOnInit() {
    this.loadSources();
    setInterval(() => {
      //Added by imran to retain the state prior to refresh of the grid
      this.dataGrid.instance.refresh();
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


    this.svc.GetResponsibiltyList().toPromise().then(resp => {
      console.log(resp);
      this.responsibilitysrc = <patrolroles[]>resp;
      console.log(this.responsibilitysrc);
    });

    let rqhdrsector: object = {
      userid: this.userid,
      ahwalid: this.selhdrAhwalId
    };

    this.svc.GetSectorsList(rqhdrsector).toPromise().then(resp => {
      this.sectorssrc = resp;
    });


    let rqhdrassociate: object = {
      userid: this.userid,
      ahwalid: this.selhdrAhwalId
    };

    this.svc.GetAssociateList(rqhdrassociate).toPromise().then(resp => {
      this.associatesrc = resp;
    });

    let rqhdrperson: object = {
      userid: this.userid,
      ahwalid: this.selhdrAhwalId
    };


    this.svc.GetPersonList(rqhdrperson).toPromise().then(resp => {

      this.personsrc = resp;
    });

    this.svc.GetCheckinPatrolCarList(this.selhdrAhwalId, this.userid).toPromise().then(resp => {

      this.patrolCarsrc = <patrolcars[]>resp;
    });

    this.svc.GetCheckinHandHeldList(this.selhdrAhwalId, this.userid).toPromise().then(resp => {

      this.handHeldsrc = <handhelds[]>resp;
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

  person_displayExpr(item) {
    // console.log(item);
    if (item !== undefined) {
      return item.name;
    }
  }


  associateExpr(item) {
    console.log(JSON.stringify(item));
    if (item !== undefined) {
      return item.name;
    }

  }

  checkPatrolExp(item) {
    if (item !== undefined) {
      return item.platenumber;
    }
  }

  bindAhwalMappingGrid() {
    let rqhr: object = {
      AhwalId: this.selhdrAhwalId,
      ShiftId: this.selhdrShiftId
    };

    this.svc.GetDispatchList(rqhr).subscribe(resp => {

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
      }, {
        location: 'before',
        widget: 'dxButton',
        options: {
          icon: 'glyphicon glyphicon-user',
          onClick: this.showInfo.bind(this)
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
    this.bindAhwalMappingGridSources();
    this.bindAhwalMappingGrid();

  }

  shiftChanged(e) {
    this.selhdrShiftId = e.value;
    this.bindAhwalMappingGrid();

  }
  /* onEditorPreparing (e) {
      console.log(e);
      if (e.parentType == 'filterRow' )
      {
          e.editorName = "dxTextBox";
          e.editorOptions.onEnterKey = "Equals";
      }
      if (e.parentType == 'dataRow' )
      {
      e.editorOptions.dataSource = this.sectorssrc;
  }
  
  } */

  onEditorPreparing(e) {
    e.editorOptions.showClearButton = true;
    // console.log('editor' + JSON.stringify(e.editorOptions));
    if (e.dataField == "sunrisetimestamp" || e.dataField == "sunsettimestamp") {
      e.editorOptions.displayFormat = "dd/MM/yyyy";
      // e.editorOptions.width=100;
      // console.log('editor' + JSON.stringify(e.editorOptions));
    }
    //  e.editorName = "dxTextBox";
    // e.editorOptions.dataSource = this.sectorssrc;
    /* e.editorOptions.onValueChanged = (event) => {
        let value = event.value;
        e.setValue(value.toLowerCase());
    } */
    // }
    // if (e.parentType == 'filterRow' && e.editorName == 'dxSelectBox')
    // e.editorName = "dxTextBox";


  }

  onRowPrepared(e) {

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
      /*  if (e.parentType == "filterRow") {
              e.editorOptions.onEnterKey = "Equals";
          }
       */
      //if (e.parentType == 'filterRow' && e.editorName == 'dxSelectBox')
      // e.editorName = "dxTextBox";

      //if(e.RowType)
      /*  if(e.parentType == "filterRow" && e.dataField == "personname") {
      e.editorName = "dxTextBox"
      e.editorOptions.dataSource = this.sectorssrc;
       } */
    }
  }

  ContextMenuClick(e) {

    if (e.itemData.text === 'حذف') {
      this.alertService.showDialog('متأكد تبي تمسح؟ أكيد؟', DialogType.confirm, () => this.deleteMapping());
    }
    else if (e.itemData.text === 'غياب') {
      this.alertService.showDialog('متأكد تبي تغير الحالة لغياب؟ أكيد؟', DialogType.confirm, () => this.updatePersonState(e.itemData.text));
    }
    else if (e.itemData.text === 'مرضيه') {
      this.alertService.showDialog('متأكد تبي تغير الحالة مرضيه؟ أكيد؟', DialogType.confirm, () => this.updatePersonState(e.itemData.text));
    }
    else if (e.itemData.text === 'اجازه') {
      this.alertService.showDialog('متأكد تبي تغير الحالة لاجازه؟ أكيد؟', DialogType.confirm, () => this.updatePersonState(e.itemData.text));
    }
    else if (e.itemData.text === 'آخر كمن حاله') {
      this.show_States_PopUp();
    }

  }
  onContextMenuprepare(e) {

    this.selahwalmappingid = e.row.key.ahwalmappingid;

    if (e.row.rowType === 'data') {
      e.items = [{
        text: 'غياب',
        value: e.row.rowIndex,
        onItemClick: this.ContextMenuClick.bind(this)
      },
      {
        text: 'مرضيه',
        value: e.row.rowIndex,
        onItemClick: this.ContextMenuClick.bind(this)
      }
        ,
      {
        text: 'اجازه',
        value: e.row.rowIndex,
        onItemClick: this.ContextMenuClick.bind(this)
      },
      {
        text: 'حذف',
        value: e.row.rowIndex,
        onItemClick: this.ContextMenuClick.bind(this)
      },
      {
        text: 'آخر كمن حاله',
        value: e.row.rowIndex,
        onItemClick: this.ContextMenuClick.bind(this)
      }
      ];

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
    console.log(this.selahwalmappingid);
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
      /*  this.svc.updatePersonState(selmenu,this.selahwalmappingid,this.userid).toPromise().then(resp =>
       {
         let olog:operationLog = new operationLog();
         olog= <operationLog>resp;
         notify( olog.text, 'success', 600);
         this.bindAhwalMappingGrid();
 
     }); */

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
      /*  this.svc.DeleteAhwalMapping(this.selahwalmappingid,this.userid).toPromise().then(resp =>
       {
         let olog:operationLog = new operationLog();
         olog= <operationLog>resp;
         notify( olog.text, 'success', 600);
         this.bindAhwalMappingGrid();
   
     }); */

    }
  }


  refreshDataGrid() {
    // this.dataGrid.instance.refresh();
    this.bindAhwalMappingGrid();
  }

  popupVisible: any = false;
  showInfo() {
    this.clearpersonpopupvalues();
    this.ahwalMappingAddMethod = 'ADD';
    this.popupVisible = true;
    //this.modalService.open('custom-modal-2');
  }

  mappopupVisible: any = false;

  showmapInfo() {
    this.modalService.open('custom-modal-1');
  }

  citySelection(e) {
    this.selectedCity = e.value;
  }

  AhwalMapping_Add_SubmitButton_Click(e) {
    let rqhdr: object = {
      PatrolRoleId: this.selectedRole,
      Milnumber: this.selectPerson_Mno,
      ShiftId: this.selhdrShiftId,
      SectorId: this.selectedSector,
      CityGroupId: this.selectedCity,
      AssociateAhwalMappingID: this.selectedAssociateMapId,
      userid: this.userid
    };

    this.svc.AddAhwalMapping(rqhdr).subscribe(resp => {


      this.ahwalMapping_Add_status_label = resp;
      this.bindAhwalMappingGrid();

    });

  }

  clearpersonpopupvalues() {

    this.selectPerson_Mno = null;
    this.selectedShift = null;

    this.associatePersonMno = null;
    this.selectedCity = null;
    this.selectedAssociateMapId = null;
    /*  */
    this.ahwalMapping_Add_status_label = '';

    this.shiftvisibile = false;
    this.sectorvisibile = false;
    this.cityvisibile = false;
    this.associatevisibile = false;

    this.selectedSector = null;
    this.selectedRole = null;

  }




  RwPopupClick(e) {
    var component = e.component,
      prevClickTime = component.lastClickTime;
    component.lastClickTime = new Date();
    if (prevClickTime && (component.lastClickTime - prevClickTime < 300)) {


    }
    else {
      //console.log( e.values[2]);

      this.selectPerson_Mno = e.values[0];
    }

  }



  Rwclick(e) {
    console.log(e);
    var component = e.component,
      prevClickTime = component.lastClickTime;
    component.lastClickTime = new Date();
    if (prevClickTime && (component.lastClickTime - prevClickTime < 300)) {
      this.clearCheckInPopupValues();
      this.selahwalmappingid = e.key.ahwalmappingid;
      if (e.key.patrolroleid !== handler_ahwalMapping.PatrolRole_Associate) {
        this.selCheckInOutPersonMno = e.key.milnumber;
      }
      this.ShowCheckInoutPopup();
      /*    this.options.defaultOpen = true;
         this.styleExp = 'inline'; */
    }

  }

  RwAssociatePopupClick(e) {
    var component = e.component,
      prevClickTime = component.lastClickTime;
    component.lastClickTime = new Date();
    if (prevClickTime && (component.lastClickTime - prevClickTime < 300)) {
      //Double click code

    }
    else {
      //  this.associatePersonMno = e.values[0];
      // console.log('ahwalassociate' + e.keyExpr);
      this.selectedAssociateMapId = e.values[2];
      this.associatePersonMno = e.values[0];
    }

  }

  RwPatrolCheckPopupClick(e) {
    console.log(e);
    //console.log(e.data.patrolid);
    this.selCheckInOutPatrolPltNo = e.data.platenumber;
  }

  RwHandHeldCheckPopupClick(e) {
    this.selCheckInOutHHeldSerialNo = e.data.serial;
  }

  ShowCheckInoutPopup() {

    this.checkInOutPopupVisible = true;
    this.AhwalMapping_CheckInOut_ID = this.selahwalmappingid;
    // this.ShowCheckInOutPopdtls();
  }

  clearCheckInPopupValues() {
    this.ahwalMapping_CheckInOut_StatusLabel = null;
    this.selCheckInOutPersonMno = null;
    this.selCheckInOutPatrolPltNo = null;
    this.selCheckInOutHHeldSerialNo = null;
  }

  CloseCheckInoutPopup() {
    this.checkInOutPopupVisible = false;

  }
  AhwalMapping_CheckInButton_Click(e) {

    let rqhdr: object = {
      personMno: this.selCheckInOutPersonMno,
      plateNumber: this.selCheckInOutPatrolPltNo,
      serial: this.selCheckInOutHHeldSerialNo,
      userid: this.userid
    };

    this.svc.CheckInAhwalMapping(rqhdr).subscribe(resp => {
      this.ahwalMapping_CheckInOut_StatusLabel = resp;
      this.bindAhwalMappingGrid();

    });


  }

  checkhandheldexpr(item) {
    if (item !== undefined) {
      return item.serial;
    }
  }

  checkInassociateExpr(item) {
    if (item !== undefined) {
      console.log(JSON.stringify(item));

      //console.log(this.selectedAssociateMapId);
      return item.milnumber;
    }
  }

  RwPersonCheckPopupClick(e) {
    this.selCheckInOutPersonMno = e.data.milnumber;

  }
}
