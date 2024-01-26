import { ChangeDetectionStrategy, Component, signal } from '@angular/core';
import { IndicatorsModule } from '@progress/kendo-angular-indicators';
import { FileInfo } from '@progress/kendo-angular-upload';
import { switchMap } from 'rxjs/operators';
import { DataListComponent } from './components/data-list/data-list.component';
import { ModelListComponent } from './components/model-list.component';
import { UploaderComponent } from './components/uploader/uploader.component';
import { AI_MODELS } from './AI_MODELS.const';
import { AiModelInformation, DataListRow } from './models';
import { ProcessHttpService, Utils } from './services';
import { DataHeaderComponent } from './components/data-header/data-header.component';

@Component({
  selector: 'app-root',
  standalone: true,
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    DataHeaderComponent,
    DataListComponent,
    IndicatorsModule,
    ModelListComponent,
    UploaderComponent,
  ],
})
export class AppComponent {
  readonly model = signal<AiModelInformation | undefined>(undefined);
  readonly file = signal<FileInfo | undefined>(undefined);
  readonly processing = signal<boolean>(false);
  readonly rows = signal<DataListRow[] | undefined>(undefined);

  readonly dealerCode = signal<string | undefined>(undefined);
  readonly dealerName = signal<string | undefined>(undefined);
  readonly dateFrom = signal<string | undefined>(undefined);
  readonly dateTo = signal<string | undefined>(undefined);
  readonly models = AI_MODELS;

  constructor(private readonly httpService: ProcessHttpService) {}

  submit(): void {
    const file = this.file();
    const model = this.model();

    if (!file || !model) return;

    this.processing.set(true);

    console.log(model);
    Utils.getFileAsync(file)
      .pipe(
        switchMap((fileBytes) =>
          this.httpService.runAsync(model.id, file.name, fileBytes)
        )
      )
      .subscribe((data) => {
        const rows: DataListRow[] = [];

        for (const [key, value] of Object.entries(data)) {
          if (key === model.dateFromField) this.dateFrom.set(value.content);
          else if (key === model.dateToField) this.dateTo.set(value.content);
          else if (key === model.dealerCodeField)
            this.dealerCode.set(value.content);
          else if (key === model.dealerNameField)
            this.dealerName.set(value.content);
          else
            rows.push({
              code: key,
              value: value.content,
              confidence: value.confidence,
            });
        }
        this.rows.set(rows);
        this.processing.set(false);
      });
  }
}
