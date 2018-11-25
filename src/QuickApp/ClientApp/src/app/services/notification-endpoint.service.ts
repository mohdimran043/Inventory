// ====================================================

// Email: support@ebenmonney.com
// ====================================================

import { Injectable } from '@angular/core';
import { HttpResponse } from '@angular/common/http';
import { Observable, of } from 'rxjs';




@Injectable()
export class NotificationEndpoint {

  private demoNotifications = [
    {
      "id": 1,
      "header": "Patrol Car 1 ",
      "body": "Patrol Car 1-Engine Started In Corniche",
      "isRead": true,
      "isPinned": true,
      "date": "2017-05-30"
    },
    {
      "id": 2,
      "header": "Patrol Car 2",
      "body": "Patrol Car 2-Connection Lost in wakra Please Contact Driver",
      "isRead": false,
      "isPinned": false,
      "date": "2017-05-30"
    },
    {
      "id": 3,
      "header": "Patrol Car 4",
      "body": "Patrol Car 4-Stopped At Location : 2225,6665",
      "isRead": false,
      "isPinned": false,
      "date": "2017-05-30"
    }
  ];



  getNotificationEndpoint<T>(notificationId: number): Observable<T> {

    let notification = this.demoNotifications.find(val => val.id == notificationId);
    let response: HttpResponse<T>;

    if (notification) {
      response = this.createResponse<T>(notification, 200);
    }
    else {
      response = this.createResponse<T>(null, 404);
    }

    return of(response.body);
  }



  getNotificationsEndpoint<T>(page: number, pageSize: number): Observable<T> {

    let notifications = this.demoNotifications;
    let response = this.createResponse<T>(this.demoNotifications, 200);

    return of(response.body);
  }



  getUnreadNotificationsEndpoint<T>(userId?: string): Observable<T> {

    let unreadNotifications = this.demoNotifications.filter(val => !val.isRead);
    let response = this.createResponse<T>(unreadNotifications, 200);

    return of(response.body);
  }



  getNewNotificationsEndpoint<T>(lastNotificationDate?: Date): Observable<T> {

    let unreadNotifications = this.demoNotifications;
    let response = this.createResponse<T>(unreadNotifications, 200);

    return of(response.body);
  }



  getPinUnpinNotificationEndpoint<T>(notificationId: number, isPinned?: boolean, ): Observable<T> {

    let notification = this.demoNotifications.find(val => val.id == notificationId);
    let response: HttpResponse<T>;

    if (notification) {
      response = this.createResponse<T>(null, 204);

      if (isPinned == null)
        isPinned = !notification.isPinned;

      notification.isPinned = isPinned;
      notification.isRead = true;
    }
    else {
      response = this.createResponse<T>(null, 404);
    }


    return of(response.body);
  }



  getReadUnreadNotificationEndpoint<T>(notificationIds: number[], isRead: boolean, ): Observable<T> {

    for (let notificationId of notificationIds) {

      let notification = this.demoNotifications.find(val => val.id == notificationId);

      if (notification) {
        notification.isRead = isRead;
      }
    }

    let response = this.createResponse<T>(null, 204);
    return of(response.body);
  }



  getDeleteNotificationEndpoint<T>(notificationId: number): Observable<T> {

    let notification = this.demoNotifications.find(val => val.id == notificationId);
    let response: HttpResponse<T>;

    if (notification) {
      this.demoNotifications = this.demoNotifications.filter(val => val.id != notificationId)
      response = this.createResponse<T>(notification, 200);
    }
    else {
      response = this.createResponse<T>(null, 404);
    }

    return of(response.body);
  }



  private createResponse<T>(body, status: number) {
    return new HttpResponse<T>({ body: body, status: status });
  }
}
