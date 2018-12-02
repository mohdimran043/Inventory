import { ElementRef,Component, OnInit ,ViewChild} from '@angular/core';
import { CommonService } from '../../../services/common.service';
import { DxDataGridComponent, DxSelectBoxComponent } from 'devextreme-angular'
import notify from 'devextreme/ui/notify';
import { confirm } from 'devextreme/ui/dialog';
import { AlertService, DialogType, MessageSeverity } from '../../../services/alert.service';
import { ModalService } from '../../../services/modalservice';
import { handler_ahwalMapping } from '../../../../environments/handler_ahwalMapping';
import { handler_operations } from '../../../../environments/handler_operations';
import { HandheldinventoryComponent } from '../../maintainence/inventory/handheldinventory/handheldinventory.component';
import { Timestamp } from '../../../../../node_modules/rxjs';
import {handler_incident} from '../../../../environments/handler_incidents';
import { FormGroup, FormControl } from '@angular/forms';
import { RSA_PKCS1_OAEP_PADDING, ESPIPE } from 'constants';
import responsive_box from '../../../../../node_modules/devextreme/ui/responsive_box';
import { fadeInOut } from '../../../services/animations';
@Component({
  selector: 'app-incidents',
  templateUrl: './incidents.component.html',
  styleUrls: ['./incidents.component.css'],
  animations: [fadeInOut]
})
export class IncidentsComponent implements OnInit {
  @ViewChild('gridIncidents') incidentGrid: DxDataGridComponent;
  @ViewChild('gridComments') gridComments: DxDataGridComponent;

  incidentsrc:any;
  incidentpopupsrc:any;
  incidenttypessrc:any;
  commentsincidentsscr:any;
  selIncidenttypeId:number;
  selincidentSourceID:number;
  incidentloadingVisible:any = false;
  selhdrAhwalId:number;
  ahwalsrc:any;
  userid:number = null;
  popupVisible:boolean;
  hdntrans:any;
  incidentplace:any;
  incidents_statusLabel:any;
  extrainfo1:any;
  extrainfo2:any;
  extrainfo3:any;
  lblinfo1visibility:any=false;
  lblinfo2visibility:any=false;
  lblinfo3visibility:any=false;
  txtinfo1visibility:any=false;
  txtinfo2visibility:any=false;
  txtinfo3visibility:any=false;
  lblinfo1:any='';
  lblinfo2:any='';
  lblinfo3:any='';
  txtinfo1:any='';
  txtinfo2:any='';
  txtinfo3:any='';
  comentPopupVisible:any = false;
  selIncidentId:number;
  commentLoadingVisible: any = false;
  comments_incident_username:any='';
  comments_incident_typename:any='';
  comments_incident_sourcename:any='';
  comments_incident_commentplace:any='';
  comments_incident_extra1name:any='';
  comments_incident_exxtra1value:any='';
  comments_incident_extra2name:any='';
  comments_incident_exxtra2value:any='';
  comments_incident_extra3name:any='';
  comments_incident_exxtra3value:any='';

  constructor(private svc:CommonService, private modalService: ModalService,private alertService: AlertService)
    {
      this.userid = parseInt(window.localStorage.getItem('UserID'),10);
      this.showLoadPanel();

    }

  GetDataSources() {
    this.svc.GetIncidentPopupSources().toPromise().then(resp => {
       // console.log(resp);
             this.incidentpopupsrc = resp;
      });

      this.svc.GetIncidentTypesList().toPromise().then(resp => {
         // console.log(resp);
               this.incidenttypessrc = resp;
        });
  }



      onCommentsShown() {
      setTimeout(() => {
          this.commentLoadingVisible = false;
      }, 3000);
      }

      onIncidentShown() {
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

    showCommentsLoadPanel() {
      this.commentLoadingVisible = true;
      }

    bindIncidentGrid() {
this.svc.GetIncidentsList().subscribe(resp => {

   this.incidentsrc = resp;
  this.incidentGrid.dataSource = this.incidentsrc;
  this.incidentGrid.instance.refresh();

});

}

onIncidentRowPrepared(e) {

  if(e.rowType ==='data') {

   e.rowElement.bgColor = 'White';


      if(e.key.incidentstateid === handler_incident.Incident_State_New ) {
        e.rowElement.bgColor ='#edb6ad';

      }
      if(e.key.incidentstateid === handler_incident.Incident_State_Closed ) {
          e.rowElement.bgColor='#eaefef';

      }
      if(e.key.incidentstateid === handler_incident.Incident_State_HasComments ) {
          e.rowElement.bgColor='#e8e69b';

      }

    }
   }


 async  getIncidentImg(incidentId:number) {
    return '../../../../assets/img/NewUpdate.png';
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
    this.txtinfo1='';
   //this.source = null;
this.txtinfo2 ='';
this.txtinfo3='';
this.incidentplace='';
this.selincidentSourceID = null;
this.selIncidenttypeId = null;
this.incidents_statusLabel = null;
  }

  ClosePopup() {
    this.popupVisible = false;
  }

RowAdd(e)
{
  console.log('place' + this.incidentplace);
  let rqhdrAddIncident:object;
  rqhdrAddIncident = {
    userid:this.userid,
    incidentsourceid:this.selincidentSourceID,
    incidenttypeid:this.selIncidenttypeId,
    incidentplace : this.incidentplace,
    extrainfo1:this.txtinfo1,
    extrainfo2:this.txtinfo2,
    extrainfo3:this.txtinfo3
  };
console.log(rqhdrAddIncident);
   this.svc.Addincidents(rqhdrAddIncident).subscribe(resp =>
    {
     this.incidents_statusLabel = resp;
     this.bindIncidentGrid();
  });


}



async incidentsourceValueChange(e)
{
  console.log(e.value);
  this.selincidentSourceID=e.value;
  if(this.selincidentSourceID != null && this.userid !=null) {
    let rqhdr:object;
    let source:any;
    rqhdr = {
      userid:this.userid,
      incidentsourceid:this.selincidentSourceID
    };

   await this.svc.GetIncidentBySourceId(rqhdr).toPromise().then(resp =>
      {
       source = resp;
       console.log('source' + source);
    });
    console.log(source);
    if (source == null) {
      return;
    }

    if (source.requiresextrainfo1 == 1)
    {
        //Incidents_AddIncident_Extrainfo1_Label.Visible = true;
        //Incidents_AddIncident_Extrainfo1_TextBox.Visible = true;
        this.txtinfo1 = source.extrainfo1;

    }
    if (source.requiresextrainfo2 == 1)
    {
       // Incidents_AddIncident_Extrainfo2_Label.Visible = true;
        //Incidents_AddIncident_Extrainfo2_TextBox.Visible = true;
        this.txtinfo2 = source.extrainfo2;

    }
    if (source.requiresextrainfo3 == 1)
    {
       // Incidents_AddIncident_Extrainfo3_Label.Visible = true;
        //Incidents_AddIncident_Extrainfo3_TextBox.Visible = true;
       //1 Incidents_AddIncident_Extrainfo3_Label.Text = source.ExtraInfo3;
       this.txtinfo3 = source.extrainfo3;

    }
  }

}

onContextMenuprepare(e) {
  this.selIncidentId = e.row.key.incidentid;

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
    this.alertService.showDialog('متأكد تبي تمسح؟ أكيد؟', DialogType.confirm, () => this.closeincident());


  }
}

closeincident()
{
  if(this.selIncidentId != null)
  {
  let rqhdr:object;
  rqhdr = {
    userid:this.userid,
    incidentid:this.selIncidentId
  };
  this.svc.CloseIncident(rqhdr).toPromise().then(resp =>
    {
       notify(resp,'success',900);
       this.bindIncidentGrid();
  });
}
}
Rwclick(e) {
  console.log(e);
  var component = e.component,
     prevClickTime = component.lastClickTime;
  component.lastClickTime = new Date();
  if (prevClickTime && (component.lastClickTime - prevClickTime < 300)) {

    this.selIncidentId = e.key.incidentid;
    this.comentPopupVisible = true;
    this.ClearCommentsView();
    this.showselectedincidentvalues();
    this.showCommentsGrid();

  }

}
async showselectedincidentvalues()
{
  let rqhdr:object;
  rqhdr = {
    userid:this.userid,
    incidentid:this.selIncidentId
  };
  await this.svc.GetIncidentById(rqhdr).toPromise().then(resp =>
    {

    this.comments_incident_username = JSON.parse(resp).username;
    this.comments_incident_typename = JSON.parse(resp).incidenttype;
    this.comments_incident_commentplace = JSON.parse(resp).incidentplace;
    this.comments_incident_sourcename = JSON.parse(resp).incidentsrcname;
    this.comments_incident_extra1name = JSON.parse(resp).incsrcextrainfo1;
    this.comments_incident_exxtra1value = JSON.parse(resp).incextrainfo1;

    this.comments_incident_extra2name = JSON.parse(resp).incsrcextrainfo2;
    this.comments_incident_exxtra2value = JSON.parse(resp).incextrainfo2;
    this.comments_incident_extra3name = JSON.parse(resp).incsrcextrainfo3;
    this.comments_incident_exxtra3value = JSON.parse(resp).incextrainfo3;
  });
}
async showCommentsGrid(){
  this.showCommentsLoadPanel();
  let rqhdr:object;
  rqhdr = {
    userid:this.userid,
    incidentid:this.selIncidentId
  };
  await this.svc.GetIncidentComments(rqhdr).toPromise().then(resp =>
    {
     this.commentsincidentsscr = resp;

     console.log('source' + JSON.stringify(resp));
  });

  console.log('source' + this.commentsincidentsscr);
}

onCommentsToolbarPreparing(e) {

  e.toolbarOptions.items.unshift(
    {
      location: 'after',
      widget: 'dxButton',
      options: {
        icon: 'refresh',
        onClick: this.refreshComments.bind(this)
      }
    },
    {
      location: 'before',
      widget: 'dxButton',
      options: {
        icon: 'plus',
        onClick: this.AddComments.bind(this)
      }
    }, {
      location: 'before',
      template: ''
    }, {
        location: 'before',
        widget: 'dxTextBox',
        options: {
          width: 200,
          placeholder: 'اكتب تعليق جديد',
          rtlEnabled: true,
          showClearButton:true,
          onValueChanged: this.commentvalchange.bind(this)
         }
      }

  );
}
txtcomments:any;
commentvalchange(e)
{
  this.txtcomments= e.value;
}
async AddComments()
{
  let rqhdr:object;
  rqhdr = {
    userid:this.userid,
    incidentid:this.selIncidentId,
    commenttext:this.txtcomments

  };
  await this.svc.AddIncidentComments(rqhdr).toPromise().then(resp =>
    {
      this.txtcomments ='';
        notify(resp, 'success', 900);

this.showCommentsGrid();

  });

}
refreshComments() {
this.showCommentsGrid();
}

 ClearCommentsView()
{
  //console.log('Remove' + e.key);
  let rqhdr:object;
  rqhdr = {
    userid:this.userid,
    incidentid:this.selIncidentId
  };
    this.svc.clearIncidentCommentsView(rqhdr).toPromise().then(resp =>
    {

  });
}

async IncidentImgUpdate(e)
{
  let rqhdr:object;
    rqhdr = {
      userid:this.userid,
      incidentid:e.key.incidentid
    };

  await this.svc.GetIncident_View(rqhdr).toPromise().then(resp => {

          if (resp != null)
          {
              return '../../../../assets/img/NewUpdate.png';
          }
   });


return '../../../../assets/img/NoUpdate.png';
}

}
