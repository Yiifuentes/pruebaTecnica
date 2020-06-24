//import { Injectable } from '@angular/core';
//import 'rxjs/add/operator/map';
//import 'rxjs/add/operator/toPromise';
//import { BaseService } from '../comun/BaseService';
//import { HttpClient, HttpErrorResponse } from '@angular/common/http';
//import { Observable } from 'rxjs';
//import 'rxjs/add/observable/of';


//@Injectable()
//export class UsuarioService extends BaseService {
//  constructor(
//    http: HttpClient,
//    private appConfigService: AppConfigService,
//    private dataService: DataService) {
//    super(http, environment.apiBaseUrl);
//  }

//  getList() {
//    if (this.appConfigService.appConfig.useInMemoryData) {
//      return Observable.of(this.dataService.activities);
//    }

//    return super.getRequest<Activity[]>('activities');
//  }

//}


//export const environment = {
//  production: false,
//  apiBaseUrl: 'http://localhost:51487/api/'
//};


//export function AppConfigServiceFactory(config: AppConfigService) {
//  return () => config.load();
//}

//interface AppConfig {
//  useInMemoryData: boolean;
//}

//@Injectable()
//export class AppConfigService {
//  appConfig: AppConfig;

//  constructor(
//    private http: HttpClient) { }

//  load() {
//    return new Promise((resolve, reject) => {
//      this.http.get<AppConfig>(`client-app-config.json?${Date.now()}`)
//        .subscribe(appConfig => {
//          this.appConfig = appConfig;
//          resolve(true);
//        }, (err: HttpErrorResponse) => {
//          reject(err);
//        });
//    });
//  }
//}