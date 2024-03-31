// angular import
import { Component, OnInit } from '@angular/core';

// project import
import { SharedModule } from 'src/app/theme/shared/shared.module';

declare const AmCharts: any;

import '../../../assets/charts/amchart/amcharts.js';
import '../../../assets/charts/amchart/gauge.js';
import '../../../assets/charts/amchart/serial.js';
import '../../../assets/charts/amchart/light.js';
import '../../../assets/charts/amchart/pie.min.js';
import '../../../assets/charts/amchart/ammap.min.js';
import '../../../assets/charts/amchart/usaLow.js';
import '../../../assets/charts/amchart/radar.js';
import '../../../assets/charts/amchart/worldLow.js';

import dataJson from 'src/fake-data/map_data';
import mapColor from 'src/fake-data/map-color-data.json';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [SharedModule],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
})
export default class DashboardComponent implements OnInit {
  ngOnInit() {
      
  }
  
  progressing = [
    {
      number: '5',
      amount: '384',
      progress: 70,
    },
    {
      number: '4',
      amount: '145',
      progress: 35,
    },
    {
      number: '3',
      amount: '24',
      progress: 25,
    },
    {
      number: '2',
      amount: '1',
      progress: 10,
    },
    {
      number: '1',
      amount: '0',
      progress: 0,
    },
  ];

  tables = [
    {
      src: 'assets/images/user/avatar-1.jpg',
      title: 'Isabella Christensen',
      text: 'Lorem Ipsum is simply dummy',
      time: '11 MAY 12:56',
      color: 'text-c-green',
    },
    {
      src: 'assets/images/user/avatar-2.jpg',
      title: 'Ida Jorgensen',
      text: 'Lorem Ipsum is simply',
      time: '11 MAY 10:35',
      color: 'text-c-red',
    },
    {
      src: 'assets/images/user/avatar-3.jpg',
      title: 'Mathilda Andersen',
      text: 'Lorem Ipsum is simply dummy',
      time: '9 MAY 17:38',
      color: 'text-c-green',
    },
    {
      src: 'assets/images/user/avatar-1.jpg',
      title: 'Karla Soreness',
      text: 'Lorem Ipsum is simply',
      time: '19 MAY 12:56',
      color: 'text-c-red',
    },
    {
      src: 'assets/images/user/avatar-2.jpg',
      title: 'Albert Andersen',
      text: 'Lorem Ipsum is',
      time: '21 July 12:56',
      color: 'text-c-green',
    },
  ];
}
