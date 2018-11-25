import { ElementRef,Component, OnInit ,ViewChild} from '@angular/core';
import { CommonService } from '../../../services/common.service';
import { DxDataGridComponent, DxSelectBoxComponent } 
from 'devextreme-angular'
import notify from 'devextreme/ui/notify';
import { confirm } from 'devextreme/ui/dialog';
import { AlertService, DialogType, MessageSeverity } 
from '../../../services/alert.service';
import { ModalService } from '../../../services/modalservice';
import { handler_ahwalMapping } from '../../../../environments/handler_ahwalMapping';
import { handler_operations } from '../../../../environments/handler_operations';
import { HandheldinventoryComponent } from '../../maintainence/inventory/handheldinventory/handheldinventory.component';
import { Timestamp } from '../../../../../node_modules/rxjs';
import {handler_incident} from '../../../../environments/handler_incidents';
import { FormGroup, FormControl } from '@angular/forms';

@Component({
  selector: 'app-incidents',
  templateUrl: './incidents.component.html',
  styleUrls: ['./incidents.component.css']
})
export class IncidentsComponent implements OnInit {
  @ViewChild('gridIncidents') incidentGrid: DxDataGridComponent;
  incidentsrc:any;
  incidentpopupsrc:any;
  incidenttypessrc:any;
  selIncidenttypeId:number;
  selincidentSourceID:number;
  incidentloadingVisible:boolean = false;
  selhdrAhwalId:number;
  ahwalsrc:any;
  userid:number = null;
  popupVisible:boolean;
  hdntrans:string;
  incidentplace:string;
  incidents_statusLabel:string;
  extrainfo1:string;
  extrainfo2:string;
  extrainfo3:string;
  lblinfo1visibility:boolean=false;
  lblinfo2visibility:boolean=false;
  lblinfo3visibility:boolean=false;
  txtinfo1visibility:boolean=false;
  txtinfo2visibility:boolean=false;
  txtinfo3visibility:boolean=false;
  lblinfo1:string="";
  lblinfo2:string="";
  lblinfo3:string="";
  txtinfo1:string="";
  txtinfo2:string="";
  txtinfo3:string="";

  constructor(private svc:CommonService, private modalService: ModalService,private alertService: AlertService) 
    { 
      this.userid = parseInt(window.localStorage.getItem('UserID'),10); 
      this.showLoadPanel();
    }

  GetDataSources()
  {
    this.svc.GetIncidentPopupSources().toPromise().then(resp =>
      {
       // console.log(resp);
             this.incidentpopupsrc = resp;
      },
      error => {
      });

      this.svc.GetIncidentTypes().toPromise().then(resp =>
        {
         // console.log(resp);
               this.incidenttypessrc = resp;
        },
        error => {
        });
  }

    onShown() {
      setTimeout(() => {
          this.incidentloadingVisible = false;
      }, 3000);
      }
      
      onIncidentShown()
      {
        setTimeout(() => {
          this.incidentloadingVisible = false;
      }, 3000);
      }

    ngOnInit() {

      this.bindIncidentGrid();
      this.GetDataSources();
      }
      

  showLoadPanel() {
    this.incidentloadingVisible = true;
    }

    bindIncidentGrid()
{
this.svc.GetIncidentsList().subscribe(resp =>
{

   this.incidentsrc = resp;
  this.incidentGrid.dataSource = this.incidentsrc;
  this.incidentGrid.instance.refresh();

});

}
 
onIncidentRowPrepared(e)
{

if(e.rowType ==='data')
{

//set default to white first
 e.rowElement.bgColor = "White";


    if(e.key.incidentstateid === handler_incident.Incident_State_New )
    {
        e.rowElement.bgColor='Red';

    }
    if(e.key.incidentstateid === handler_incident.Incident_State_Closed )
    {
        e.rowElement.bgColor='LightGray';

    }
    if(e.key.incidentstateid === handler_incident.Incident_State_HasComments )
    {
        e.rowElement.bgColor='Yellow';

    }

  }
 }

 async  getIncidentImg(incidentId:number)
 {
    return "../../../../assets/img/NewUpdate.png";
  }
  IncidentRwclick(e)
  {
     this.selincidentSourceID = e.key.incidentid;
  
  }
  
  onIncidentToolbarPreparing(e) {
    e.toolbarOptions.items.unshift({
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
          onClick: this.bindIncidentGrid.bind(this)
        }
      });
    }
    
  ShowAddPopup() {
    this.hdntrans = 'ADD';
    this.cleardefaultvalues();
    this.popupVisible = true;
  }
  cleardefaultvalues()
  {
    this.txtinfo1="";

  }

  ClosePopup() {
    this.popupVisible = false;
  }

RowAdd(e)
{
//console.log(e);
  //this.cleardata();
  let rqhdr:object;
  rqhdr = {
    userid:this.userid,
    incidentsourceid:this.selincidentSourceID,
    incidenttypeid:this.selIncidenttypeId,
    incidentplace : this.incidentplace,
    extrainfo1:this.extrainfo1,
    extrainfo2:this.extrainfo2,
    extrainfo3:this.extrainfo3
  };

  this.svc.Addincidents(rqhdr).subscribe(resp =>
    {
     this.incidents_statusLabel = resp;
     this.bindIncidentGrid();
  });
    

}

async incidentsourceValueChange(e)
{
  let rqhdr:object;
  let source:any;
  rqhdr = {
    userid:this.userid,
    incidentsourceid:this.selincidentSourceID
  };

 await this.svc.GetIncidentById(rqhdr).subscribe(resp =>
    {
     source = resp;
  });
}
  
}
