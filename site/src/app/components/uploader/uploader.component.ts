import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Output,
} from '@angular/core';
import { FileInfo, FileSelectModule } from '@progress/kendo-angular-upload';

@Component({
  standalone: true,
  selector: 'app-uploader',
  templateUrl: './uploader.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [FileSelectModule],
})
export class UploaderComponent {
  @Output() readonly removed = new EventEmitter<void>();
  @Output() readonly uploaded = new EventEmitter<FileInfo>();
}
