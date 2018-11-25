import { Component, OnInit ,ViewChild} from '@angular/core';
import { CommonService } from '../../../../services/common.service';
import { DxDataGridComponent } from 'devextreme-angular';

@Component({
  selector: 'app-deviceinventory',
  templateUrl: './patrolcarsinventory.component.html',
  styleUrls: ['./patrolcarsinventory.component.css']
})
export class PatrolCarsinventoryComponent implements OnInit {

  @ViewChild(DxDataGridComponent) dataGrid: DxDataGridComponent;
  loadingVisible = false;
  orgs:any;
  dataSource: any;
  selhdrAhwalId: number;
  ahwalsrc:any;
  userid:any;
  constructor(private svc:CommonService) {
    this.userid = parseInt(window.localStorage.getItem('UserID'),10);
    this.ahwalsrc= JSON.parse(window.localStorage.getItem('Ahwals'));
    this.selhdrAhwalId = this.ahwalsrc[0].ahwalid;
    this.showLoadPanel();
   }

   onShown() {
    setTimeout(() => {
        this.loadingVisible = false;
    }, 3000);
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
    ahwalid : this.selhdrAhwalId,
    userid:this.userid,
    
  };
  this.svc.GetpatrolcarsInventoryList(rqhdr).subscribe(resp =>
    {

       this.dataSource = resp;
    //  console.log('resp' + resp);
      this.dataGrid.dataSource = this.dataSource;
      this.dataGrid.instance.refresh();

  },
    error => {

    });
}

CellPrepared(e) {
 
  if (e.rowType == "header") {
    console.log(e);
    //  e.cellElement.addClass("hdrcls"); 
  }
}

onToolbarPreparing(e) {
  let AhwalLst :any=[];
  AhwalLst =JSON.parse(window.localStorage.getItem('Orgs'));

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
              icon: 'refresh',
              onClick: this.refreshDataGrid.bind(this)
          }
      });
}

ahwalChanged(e) {
  console.log(e.value);
  this.selhdrAhwalId = e.value;
 this.LoadData();
}



refreshDataGrid() {
  this.LoadData();
}

}
