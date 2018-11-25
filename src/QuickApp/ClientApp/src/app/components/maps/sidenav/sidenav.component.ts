import { NgModule, Component ,ViewChild, ElementRef} from '@angular/core';
import { DxTreeViewModule, DxListModule, DxTemplateModule } from 'devextreme-angular';
import { Product, Service } from '../../../services/sidenav.service'
import { SharedMapServiceService } from  '../../../services/shared-map-service.service'
import { MapComponent } from '../map.component';
import {NgbDateStruct, NgbCalendar} from '@ng-bootstrap/ng-bootstrap';


@Component({
    selector: 'sidenav',
    templateUrl: './sidenav.component.html',
    styleUrls: ['./sidenav.component.css'],
    providers: [Service]
})
export class SidenavComponent {
    @ViewChild('ipt') RngSlid: ElementRef;
    @ViewChild('Fromdp') Fromdp: ElementRef;
    @ViewChild('Todp') Todp: ElementRef;
    @ViewChild('slOptions') slOptions: ElementRef;
    @ViewChild('RpdeviceLst') RpdeviceLst: ElementRef;


    public sim_txt: string = 'Pause Simulation';
    products: Product[];
    checkedItems: Product[] = [];
    subscription: any;
    model: NgbDateStruct;


    constructor(service: Service, private someSharedService: SharedMapServiceService,private calendar: NgbCalendar) {
        this.products = service.getProducts();
    }

    selectionChanged(e) {
        //this.subscription = this.Mapobj.AllvehicleObsrv(e.node).subscribe();
       
        let value = e.node;
        if (this.isProduct(value)) {
            this.processProduct({
                id: value.key,
                text: value.text,
                itemData: value.itemData,
                selected: value.selected,
                category: value.parent.text
            });
            if(value.selected === true)
            {
                this.someSharedService.AllVehicles(value.text);
            }
            else
            {
                this.someSharedService.RemoveAllVehicles(value.text);
            }
           
        } else {
            value.items.forEach((product, index) => {
                this.processProduct({
                    id: product.key,
                    text: product.text,
                    itemData: product.itemData,
                    selected: product.selected,
                    category: value.text
                });
                if(value.selected === true)
                {
                    this.someSharedService.AllVehicles(product.text);
                }
                else
                {
                    this.someSharedService.RemoveAllVehicles(product.text);
                }
            });
           // alert('2');
        }
    }

    isProduct(data) {
        return !data.items.length;
    }

    processProduct(product) {
        let itemIndex = -1;

        this.checkedItems.forEach((item, index) => {
           if (item.id === product.id) {
                itemIndex = index;
                return false;
            }
        });
        if (product.selected && itemIndex === -1) {
            this.checkedItems.push(product);
        } else if (!product.selected) {
            this.checkedItems.splice(itemIndex, 1);
        }
    }
    SimulateDevices()
{

this.someSharedService.SimulateDevices(1);
}

PauseSimulation()
{
    if (this.sim_txt === 'Resume Simulate') {
        this.someSharedService.ResumeSimulation(1);
        this.sim_txt = 'Stop Simulate';
  
      }
      else {
        this.someSharedService.PausseSimulation(1);
        this.sim_txt = 'Resume Simulate';
      }
   
}

RngSlider_Simulate()
{
    this.someSharedService.RngSlider_Simulate(parseInt(this.RngSlid.nativeElement.value));
}

Simulate_By_Route_Device()
{
    this.someSharedService.Simulate_By_Route_Device(1);
}

StartRoute()
{
//console.log(this.Fromdp._model.day);
let objdescrpt:any = Object.getOwnPropertyDescriptor(this.Fromdp, '_model');
let FromDt:any = objdescrpt.value.day + "/" + objdescrpt.value.month + "/" + objdescrpt.value.year;
objdescrpt = Object.getOwnPropertyDescriptor(this.Todp, '_model');
let ToDt:any= objdescrpt.value.day + "/" + objdescrpt.value.month + "/" + objdescrpt.value.year;

console.log(FromDt);
//this.someSharedService.ShowReports(FromDt,ToDt,this.slOptions.nativeElement.value,this.RpdeviceLst.nativeElement.value);
}

onDateSelect(e)
{
console.log(e);
}

}

