import { Component, OnInit } from '@angular/core';
import { HubConnection, HubConnectionBuilder, LogLevel } from '@aspnet/signalr';
import { connect } from 'net';
import { AuthService } from './shared/services/auth.service';

@Component({
  selector: 'abs-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  title = 'awesome-bookstore';
  constructor(private readonly auth: AuthService) {

  }

  public ngOnInit(): void {
    const connection = new HubConnectionBuilder()
      .withUrl('/notifications')
      .configureLogging(LogLevel.Debug)
      .build();

    connection.on('message', data => {
      console.log(data);
    });

    connection.start()
    .then(() => {
      console.log('Connection started!');
      setTimeout(() => {
        connection.invoke('message', {
          type: 'connected',
          user: this.auth.user
        });
      }, 2000);
    })
    .catch(err => console.error('Error while establishing connection :(', err));
  }
}
