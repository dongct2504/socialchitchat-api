import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, finalize } from 'rxjs';
import { BusyLoadingService } from '../_services/busy-loading.service';

@Injectable()
export class LoadingInterceptor implements HttpInterceptor {

  constructor(private busyLoadingService: BusyLoadingService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    this.busyLoadingService.busy();

    return next.handle(request).pipe(
      finalize(() => {
        this.busyLoadingService.idle();
      })
    );
  }
}