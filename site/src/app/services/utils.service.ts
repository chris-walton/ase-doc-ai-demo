import { FileInfo } from '@progress/kendo-angular-upload';
import { Observable } from 'rxjs';

export class Utils {
  static getFileAsync(file: FileInfo): Observable<ArrayBuffer> {
    return new Observable<ArrayBuffer>((obs) => {
      if (!file) {
        obs.complete();
        return;
      }
      const reader = new FileReader();

      reader.onload = function (ev) {
        const data = ev.target?.result;

        obs.next(<ArrayBuffer>data);
        obs.complete();
      };

      reader.readAsArrayBuffer(file.rawFile!);
    });
  }
}
