import { Component, OnInit, ViewChild, ChangeDetectorRef } from '@angular/core';
import { CommonService } from '../../../services/common.service';
import { DxDataGridComponent } from 'devextreme-angular'
import notify from 'devextreme/ui/notify';
import { patrolcars } from '../../../models/patrolcars';
import SelectBox from 'devextreme/ui/select_box';
import { ModalService } from '../../../services/modalservice';
import { AlertService, DialogType, MessageSeverity } from '../../../services/alert.service';
import { debounce } from 'rxjs/operators';
@Component({
  selector: 'app-communication',
  templateUrl: './communication.component.html',
  styleUrls: ['./communication.component.css']
})
export class CommunicationComponent implements OnInit {
  @ViewChild(DxDataGridComponent) dataGrid: DxDataGridComponent;
  data: any;
  patrolid: number = 0;
  userid: number;
  dataSource: any;
  incidenttypeid: number;
  incidenttypename: string;
  popupVisible: boolean;
  hdntrans: string = '';
  modaltitle: string = '';

  loadingVisible = false;
  constructor(private svc: CommonService, private modalService: ModalService, private cd: ChangeDetectorRef, private alertService: AlertService) {
    this.userid = parseInt(window.localStorage.getItem('UserID'), 10);
    this.showLoadPanel();
  }
  showLoadPanel() {
    this.loadingVisible = true;
  }

  ngOnInit() {
    this.LoadData();
  }
  onShown() {
    setTimeout(() => {
      this.loadingVisible = false;
    }, 3000);
  }
  LoadData() {
    let userid: string = window.localStorage.getItem('UserID');
    this.svc.GetIncidentTypes().subscribe(resp => {
      console.log(resp);
      this.dataSource = resp;
    }, error => { });

  }

  onToolbarPreparing(e) {
    e.toolbarOptions.items.unshift({
      location: 'before',
      template: 'الأحوال'
    }, {
        location: 'after',
        widget: 'dxButton',
        options: {
          icon: 'plus',
          onClick: this.ShowAddPopup.bind(this)
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
  refreshDataGrid() {
    this.LoadData();

  }
  RowAdd(e) {
    debugger
    let rqhdr: object = { incidenttypeid: this.incidenttypeid, userid: this.userid, incidenttypename: this.incidenttypename };
    this.svc.AddIncidentTypes(rqhdr).subscribe(resp => { this.LoadData(); }, error => { });
    this.cleardata();
    this.popupVisible = false;
    notify(' Record Added SuccessFully', 'success', 600);
  }
  
  RowDelete() {
    //this.cleardata();
    let rqhdr: object = { incidenttypeid: this.incidenttypeid, userid: this.userid };
    this.svc.DeleteIncidentTypes(rqhdr).subscribe(resp => {
      notify(' Record Deleted  SuccessFully', 'success', 1000);
      this.LoadData();
    }, error => { });
  }
  cleardata() {
    this.incidenttypeid = 0;
    this.incidenttypename = '';
  }

  showInfo() {

    this.popupVisible = true;
  }

  ShowAddPopup() {
    this.hdntrans = 'I';
    this.modaltitle = 'Add Incident Type';
    this.cleardata();
    this.popupVisible = true;
  }

  ShowUpdatePopup(e, dt) {

    this.hdntrans = 'U';
    this.incidenttypeid = dt.incidenttypeid;
    this.incidenttypename = dt.name;
    this.modaltitle = 'Update Incident Type';
    this.popupVisible = true;
  }


  DelRecord(e, data) {
    this.cleardata();
    this.incidenttypeid = data.incidenttypeid;
    this.alertService.showDialog('متأكد تبي تمسح؟ أكيد؟', DialogType.confirm, () => this.RowDelete());
  }

  ClosePopup() {
    this.popupVisible = false;
  }

}
