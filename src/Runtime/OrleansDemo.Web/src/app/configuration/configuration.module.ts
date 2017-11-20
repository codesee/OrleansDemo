import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { MaterialModule } from '../material/material.module';
import { ServicesModule } from '../services/services.module';

import { ConfigurationRoutingModule } from './configuration-routing.module';
import { ConfigurationComponent } from './configuration.component';
import { DeviceListComponent } from './device-list/device-list.component';
import { DeviceTypeListComponent } from './device-type-list/device-type-list.component';
import { DeviceTypeDialogComponent } from './device-type-list/device-type-dialog.component';
import { ReadingTypeListComponent } from './reading-type-list/reading-type-list.component';
import { ReadingTypeDialogComponent } from './reading-type-list/reading-type-dialog.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    ConfigurationRoutingModule,
    MaterialModule,
    ServicesModule
  ],
  declarations: [
    ConfigurationComponent,
    DeviceListComponent,
    DeviceTypeListComponent,
    DeviceTypeDialogComponent,
    ReadingTypeListComponent,
    ReadingTypeDialogComponent
  ],
  bootstrap: [
    DeviceTypeDialogComponent,
    ReadingTypeDialogComponent
  ]
})
export class ConfigurationModule { }