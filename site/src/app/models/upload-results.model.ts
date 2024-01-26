export type UploadResults = { [code: string]: UploadValueResults };

export interface UploadValueResults {
  confidence: number;
  content: string;
}
