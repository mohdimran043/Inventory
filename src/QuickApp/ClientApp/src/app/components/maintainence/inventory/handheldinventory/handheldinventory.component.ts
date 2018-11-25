import { Component, OnInit ,ViewChild} from '@angular/core';
import { CommonService } from '../../../../services/common.service';
import { DxDataGridComponent } from 'devextreme-angular'

@Component({
  selector: 'app-handheldinventory',
  templateUrl: './handheldinventory.component.html',
  styleUrls: ['./handheldinventory.component.css']
})
export class HandheldinventoryComponent implements OnInit {
  @ViewChild(DxDataGridComponent) dataGrid: DxDataGridComponent;
  loadingVisible = false;
  orgs:any;
  dataSource: any;
  selhdrAhwalId: number;
  ahwalsrc:any;
  userid:any;
  statesrc:any;
  stateid:number=-1;
  constructor(private svc:CommonService) {
    this.userid = parseInt(window.localStorage.getItem('UserID'),10);
    this.ahwalsrc= JSON.parse(window.localStorage.getItem('Ahwals'));
    this.selhdrAhwalId = this.ahwalsrc[0].ahwalid;
    this.showLoadPanel();
    this.bindhanheldhdrsorce();
   }

 
 


   bindhanheldhdrsorce()
   {
    this.svc.GetCheckInOutStatesList().toPromise().then(resp =>
      {
          console.log(resp);
             this.statesrc = resp;
             console.log(this.statesrc);
      },
      error => {
      });
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
  console.log(this.stateid);
  rqhdr = {
    ahwalid : this.selhdrAhwalId,
    userid:this.userid
  };
  this.svc.GetHandHeldsInventoryList(rqhdr).subscribe(resp =>
    {

       this.dataSource = resp;
      console.log('resp' + resp);
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

}
