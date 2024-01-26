import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { GridModule } from '@progress/kendo-angular-grid';
import { DataListRow } from '../../models';

@Component({
  standalone: true,
  selector: 'app-data-list',
  templateUrl: './data-list.component.html',
  styleUrl: './data-list.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [GridModule],
})
export class DataListComponent {
  @Input({ required: true }) rows!: DataListRow[];
}
