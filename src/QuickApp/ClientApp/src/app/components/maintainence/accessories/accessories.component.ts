import { Component, OnInit ,ViewChild} from '@angular/core';
import { CommonService } from '../../../services/common.service';
import { DxDataGridComponent } from 'devextreme-angular';
import notify from 'devextreme/ui/notify';
import {accessorycls} from '../../../models/accessorycls';


@Component({
  selector: 'app-accessories',
  templateUrl: './accessories.component.html',
  styleUrls: ['./accessories.component.css']
})
export class AccessoriesComponent implements OnInit {

 @ViewChild(DxDataGridComponent) dataGrid: DxDataGridComponent;
  loadingVisible = false;
  selahwalid:number = 1;
  rentalchk:number = 0;
  defectchk:number = 0;
  typesrc:any;
  dataSource: any;
  devicetypesrc:any;
  public deviceobj:accessorycls = new accessorycls();


  constructor(private svc:CommonService) {
    this.showLoadPanel();
   // this.typesrc = JSON.parse(window.localStorage.getItem("devicetypes"));
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
  this.svc.GetaccessoryList().subscribe(resp =>
    {

       this.dataSource = JSON.parse(resp);
      console.log('resp' + resp);
      this.dataGrid.dataSource = this.dataSource;
      this.dataGrid.instance.refresh();

  },
    error => {

    });

    /*this.svc.GetDeviceTypesList().subscribe(resp =>
      {

         this.devicetypesrc = JSON.parse(resp);


    },
      error => {

      });*/

}
onToolbarPreparing(e) {
  e.toolbarOptions.items.unshift({
      location: 'before',
      template: 'Organization'
  }, {
          location: 'before',
          widget: 'dxSelectBox',
          options: {
              width: 200,
              items: [{
                  value: 'Org1',
                  text: 'Grouping by Org1'
              }, {
                  value: 'Org2',
                  text: 'Grouping by Org2'
              }],
              displayExpr: 'text',
              valueExpr: 'value',
              value: 'CustomerStoreState',
              onValueChanged: this.groupChanged.bind(this)
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

groupChanged(e) {
  this.dataGrid.instance.clearGrouping();
  this.dataGrid.instance.columnOption(e.value, 'groupIndex', 0);
}



refreshDataGrid() {
  this.LoadData();
  //this.dataGrid.instance.refresh();
}

cleardata()
{
  this.deviceobj.ahwalid =  -1;
  this.deviceobj.barcode = '';
  this.deviceobj.defective =  -1;
  this.deviceobj.devicenumber =  '';
  this.deviceobj.devicetypeid =  -1;
  this.deviceobj.model =  '';
  this.deviceobj.rental = -1;
  this.deviceobj.deviceid =  -1;
}

PopupInitialize(e)
{
  console.log('popupini');
  this.cleardata();
  this.cleardefaultvalues();
}
cleardefaultvalues()
{
  this.rentalchk = 0;
  this.defectchk = 0;
}
RowAdd(e)
{
  console.log(this.rentalchk);
  this.cleardata();
  this.deviceobj.ahwalid =  this.selahwalid;
  this.deviceobj.barcode =  e.data.barcode;
  this.deviceobj.defective =  e.data.defective;
  this.deviceobj.devicenumber =  e.data.devicenumber;
  this.deviceobj.devicetypeid =  1;
  this.deviceobj.model =  e.data.model;
  this.deviceobj.rental = this.rentalchk;

  this.svc.Addaccessory(this.deviceobj).subscribe(resp =>
    {
      console.log('resp' + resp);
     this.LoadData();
  },
    error => {

    });
    this.cleardata();
    this.cleardefaultvalues();

  notify(" Record Added SuccessFully", "success", 600);
}

checkBoxToggled(e)
{
  //console.log(e.value);
  if( e.value == true)
  {
    this.rentalchk = 1;
  }
  else{
    this.rentalchk = 0;
  }

}
RowUpdate(e)
{

  this.svc.Updateaccessory(this.deviceobj).subscribe(resp =>
    {

      console.log('resp' + resp);
     this.LoadData();
  },
    error => {

    });
}

RowDelete(e)
{
  this.cleardata();
  this.deviceobj.ahwalid =  this.selahwalid;
  this.deviceobj.barcode =  e.data.barcode;
  this.deviceobj.defective =  e.data.defective;
  this.deviceobj.devicenumber =  e.data.devicenumber;
  this.deviceobj.model =  e.data.model;
  this.deviceobj.rental = e.data.rental;
  this.deviceobj.deviceid =  e.data.deviceid;
  console.log(e);
  this.svc.Deleteaccessory(this.deviceobj).subscribe(resp =>
    {

      console.log('resp' + resp);
     this.LoadData();
  },
    error => {

    });
}


}
