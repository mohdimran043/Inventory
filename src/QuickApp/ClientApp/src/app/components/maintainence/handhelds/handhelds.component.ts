import { Component, OnInit ,ViewChild} from '@angular/core';
import { CommonService } from '../../../services/common.service';
import { DxDataGridComponent } from 'devextreme-angular';
import notify from 'devextreme/ui/notify';
import { ModalService } from '../../../services/modalservice';
import { THIS_EXPR } from '../../../../../node_modules/@angular/compiler/src/output/output_ast';
import { AlertService, DialogType, MessageSeverity } from '../../../services/alert.service';

@Component({
  selector: 'app-handhelds',
  templateUrl: './handhelds.component.html',
  styleUrls: ['./handhelds.component.css']
})
export class HandheldsComponent implements OnInit {

  @ViewChild(DxDataGridComponent) dataGrid: DxDataGridComponent;
  loadingVisible = false;
  rentalchk:number = 0;
  defectchk:number = 0;
  typesrc:any;
  dataSource: any;
  devicetypesrc:any;
   userid: string;
   popupVisible: any = false;
   hdntrans: string = '';
   defective: number = 0;
   handheldid: number = null;
   barcode: string = '';
   ahwalsrc:any;
   selhdrAhwalId:number;
   serial:string='';
   handhelds_StatusLabel:string='';


  constructor ( private modalService: ModalService, public svc:CommonService,private alertService: AlertService) {
    this.userid = window.localStorage.getItem('UserID');
    this.ahwalsrc= JSON.parse(window.localStorage.getItem('Ahwals'));
    this.selhdrAhwalId = this.ahwalsrc[0].ahwalid;

    this.showLoadPanel();
   }



   onShown() {
    setTimeout(() => {
        this.loadingVisible = false;
    }, 3000);
  }
  typeselboxtoggled(e)
  {

  }
  showLoadPanel() {
    this.loadingVisible = true;
  }

  ngOnInit() {

    this.LoadData();
  }

LoadData()
{
   let rqhdr:object;
rqhdr = {
  ahwalid:this.selhdrAhwalId,
  userid:this.userid
};

  this.svc.GethandheldsList(rqhdr).subscribe(resp =>
    {

       this.dataSource = resp;
      console.log('resp' + JSON.stringify(resp));
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
        value:this.ahwalsrc[0].ahwalid,
        onValueChanged: this.ahwalChanged.bind(this)
      }
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

ahwalChanged(e) {
  this.selhdrAhwalId = e.value;
  this.LoadData();
}




refreshDataGrid() {
  this.LoadData();
 
}

cleardata()
{
  this.handheldid=null;
  this.defectchk = 0;
  this.serial='';
  this.handhelds_StatusLabel='';

}


chkdeftoggle(e) {
  //console.log('check' + e);
  if (e === true) {
    this.defectchk = 1;
  }
  else {
    this.defectchk = 0;
  }

}

RowAdd(e)
{
//console.log(e);
  //this.cleardata();
  let rqhdr:object;
  rqhdr = {
    ahwalid:this.selhdrAhwalId,
    userid:this.userid,
    defective:this.defectchk,
    serial:this.serial,
    transmode:this.hdntrans,
    handheldid : null

  };

  this.svc.Addhandhelds(rqhdr).subscribe(resp =>
    {
     // notify(resp, 'success', 600);
     this.handhelds_StatusLabel = resp;
    this.LoadData();
  });
    

}




RowUpdate(e)
{
  console.log('update' + this.defectchk);
  let rqhdr:object;
  rqhdr = {
    ahwalid:this.selhdrAhwalId,
    userid:this.userid,
    defective:this.defectchk,
    serial:this.serial,
    transmode:this.hdntrans,
    handheldid : this.handheldid
  };

  this.svc.Updatehandhelds(rqhdr).subscribe(resp =>
    {
     // notify(resp, 'success', 600);
     this.handhelds_StatusLabel = resp;
     this.LoadData();
  });
}

RowDelete()
{
  //console.log(e);
  //this.cleardata();
  let rqhdr:object;
  rqhdr = {
    handheldid : this.handheldid,
    userid:this.userid,
    serial:this.serial
    
  };
  this.svc.Deletehandhelds(rqhdr).subscribe(resp =>
    {

      notify(resp, 'success', 1000);
     //this.handhelds_StatusLabel = resp;
     this.LoadData();
  });
}

ShowAddPopup() {
  this.hdntrans = 'ADD';
  this.cleardata();
  this.popupVisible = true;
}

ShowUpdatePopup(e, dt) {
  console.log(dt);
  this.cleardata();
  this.handheldid = dt.handheldid;
  this.serial = dt.serial;
  this.defectchk = parseInt(dt.defective);
  console.log(this.defectchk);
  this.hdntrans = 'UPDATE';
  
  this.popupVisible = true;
}

DelRecord(e, data) {
  this.cleardata();
  this.handheldid = data.handheldid;
  this.serial = data.serial;
  this.alertService.showDialog('متأكد تبي تمسح؟ أكيد؟', DialogType.confirm, () => this.RowDelete());

 // this.dataGrid.instance.deleteRow(rindex);
}

ClosePopup() {
  this.popupVisible = false;
}

}
