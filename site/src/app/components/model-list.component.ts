import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  Output,
} from '@angular/core';
import { DropDownListModule } from '@progress/kendo-angular-dropdowns';
import { AiModelInformation } from '../models';

@Component({
  standalone: true,
  selector: 'app-model-list',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [DropDownListModule],
  template: `<kendo-dropdownlist
    [data]="models"
    textField="name"
    valueField="id"
    (selectionChange)="modelSelected.emit($event)"
  />`,
})
export class ModelListComponent {
  @Input({ required: true }) models!: AiModelInformation[];
  @Output() readonly modelSelected = new EventEmitter<AiModelInformation>();
}
