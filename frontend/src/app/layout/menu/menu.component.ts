import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'abs-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent implements OnInit {

  collapsed = true;

  constructor() { }

  ngOnInit() {
  }

}
