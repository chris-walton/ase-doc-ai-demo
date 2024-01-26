import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UploadResults } from '../models';

@Injectable({ providedIn: 'root' })
export class ProcessHttpService {
  constructor(private readonly http: HttpClient) {}

  runAsync(
    model: string,
    fileName: string,
    file: ArrayBuffer
  ): Observable<UploadResults> {
    return this.http.post<UploadResults>(
      `http://localhost:88/api/process/${model}/${fileName}`,
      file
    );
  }
}
