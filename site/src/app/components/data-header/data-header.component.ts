import { ChangeDetectionStrategy, Component, Input } from '@angular/core';

@Component({
  standalone: true,
  selector: 'app-data-header',
  templateUrl: './data-header.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [],
})
export class DataHeaderComponent {
  @Input({ required: true }) dealerCode?: string;
  @Input({ required: true }) dealerName?: string;
  @Input({ required: true }) dateFrom?: string;
  @Input({ required: true }) dateTo?: string;
}
