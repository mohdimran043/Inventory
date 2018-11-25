import { Injectable } from '@angular/core';

export class Product {
    id: string;
    text: string;
    expanded?: boolean;
    selected?: boolean;
    items?: Product[];
}

var products: Product[] = [
  {
    id: "1_1_1",
  text: "All Vehicles",
    items: [{
        id: "1_1_1_1",
      text: "26470"
    }, {
        id: "1_1_1_2",
        text: "283584"
    }]
}, {
    id: "1_1_2",
    text: "Moving Vehicles",
    items: [{
      id: "1_1_2_1",
      text: "26570"
    }, {
      id: "1_1_2_2",
      text: "26670"
    }]
}, {
    id: "1_1_3",
    text: "Stopping Vehicles",
    items: [{
        id: "1_1_3_1",
        text: "26870"
      }]
}, {
    id: "1_1_4",
    text: "GPS Communication Problem",
    items: [{
        id: "1_1_4_1",
        text: "26870"
      }]
}, {
    id: "1_1_5",
    text: "Vehicles That Have Sent Alarm",
    items: [{
        id: "1_1_5_1",
        text: "26870"
      }]
}
, {
    id: "1_1_6",
    text: "Vehicles That Cannot Receive GPS Signal",
    items: [{
        id: "1_1_6_1",
        text: "26890"
      }]
}, {
    id: "1_1_7",
    text: "Routes",
    items: [{
        id: "1_1_7_1",
        text: "المنهج 22/06/2015 09:37:37 "
      }]
}, {
    id: "1_1_8",
    text: "(60) ادارة",
    items: [{
        id: "1_8_1",
        text: "(23) قسم"
      },
      {
        id: "1_8_2",
        text: "(28) قسم"
      }]
}, {
    id: "1_1_9",
    text: "الدوريات 88",
    items: [{
        id: "1_9_1",
        text: "(23) قسم"
      }]
}, {
    id: "1_1_10",
    text: "الفريق الخاص",
    items: [{
        id: "1_10_1",
        text: "(23) قسم"
      }]
}, {
    id: "1_1_11",
    text: "دخان",
    items: [{
        id: "1_11_1",
        text: "(23) قسم"
      }]
}, {
    id: "1_1_12",
    text: "مطار حمد الدولي",
    items: [{
        id: "1_12_1",
        text: "(23) قسم"
      }]
}
];

@Injectable()
export class Service {
    getProducts(): Product[] {
        
        return products;
    }
}
