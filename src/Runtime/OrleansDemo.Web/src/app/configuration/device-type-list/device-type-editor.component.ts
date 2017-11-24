import { Component, OnInit, ViewEncapsulation } from '@angular/core';

import { DetailsItemComponent, DetailsHostService } from '../../shared/details-host/details-host.service';
import { DeviceType, DeviceReadingType, DeviceTypeService } from '../../services/device-type.service';
import { ReadingType, ReadingTypeService } from '../../services/reading-type.service';

@Component({
  selector: 'app-device-type-editor',
  templateUrl: './device-type-editor.component.html',
  styleUrls: ['./device-type-editor.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class DeviceTypeEditorComponent implements OnInit, DetailsItemComponent {
  data: any;
  deviceType: DeviceType;
  readingTypes: ReadingType[];

  constructor(private detailsHost: DetailsHostService,
    private deviceTypeService: DeviceTypeService,
    private readingTypeService: ReadingTypeService) { }

  ngOnInit() {
    this.deviceType = this.data as DeviceType;
    this.readingTypeService.getList().subscribe(results => {
      this.setupDeviceType(results);
    });
  }

  save() {
    if (this.deviceType.id !== null) {
      this.deviceTypeService.update(this.deviceType).subscribe(result => {
        this.detailsHost.closeItem();
      });
    } else {
      this.deviceTypeService.save(this.deviceType).subscribe(result => {
        this.detailsHost.closeItem();
      });
    }
  }

  cancel() {
    this.detailsHost.closeItem();
  }

  private setupDeviceType(results: ReadingType[]) {
    results.forEach(readingType => {
      const reading = this.deviceType.readingTypes.find(r => r.readingTypeId === readingType.id);
      if (reading === undefined) {
        const newReadingType = new DeviceReadingType();
        newReadingType.id = null;
        newReadingType.readingTypeId = readingType.id;
        newReadingType.readingType = readingType.name;
        newReadingType.active = false;

        this.deviceType.readingTypes.push(newReadingType);
      }
    });
  }
}
