import { Observable } from 'rxjs';
import { Router } from '@angular/router';


export abstract class BaseService {

  constructor() { }

  protected handleError(error: any) {
    var applicationError = error.headers.get('Application-Error');

    // either applicationError in header or model error in body


    if (error.status == 401 || error.status == 403) {
      if (applicationError) {
        return Observable.throw(applicationError);
      }


      var modelStateErrors: any = '';
      modelStateErrors = error.status;
      return Observable.throw(modelStateErrors || 'Server error');
    }


    if (applicationError) {
      return Observable.throw(applicationError);
    }

    var modelStateErrors: any = '';
    var serverError = error.json();

    if (!serverError.type) {
      for (var key in serverError) {
        if (serverError[key])
          modelStateErrors += serverError[key] + '\n';
      }
    }

    modelStateErrors = modelStateErrors = '' ? null : modelStateErrors;
    return Observable.throw(modelStateErrors || 'Server error');

  }
}