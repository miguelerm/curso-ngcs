import { BrowserModule } from '@angular/platform-browser';
import { NgModule, LOCALE_ID } from '@angular/core';
import { registerLocaleData } from '@angular/common';
import localeEs from '@angular/common/locales/es';
import localeEsGT from '@angular/common/locales/es-GT';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LayoutModule } from './layout/layout.module';
import { SharedModule } from './shared/shared.module';

registerLocaleData(localeEs, 'es-GT', localeEsGT);
@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    LayoutModule,
    SharedModule
  ],
  providers: [{ provide: LOCALE_ID, useValue: 'es-GT' }],
  bootstrap: [AppComponent]
})
export class AppModule { }
