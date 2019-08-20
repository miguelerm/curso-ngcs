import { Component, OnInit, Input } from '@angular/core';
import { HttpClient, HttpRequest, HttpEventType } from '@angular/common/http';

@Component({
  selector: 'abs-covers-editor',
  templateUrl: './covers-editor.component.html',
  styleUrls: ['./covers-editor.component.css']
})
export class CoversEditorComponent implements OnInit {

  @Input()
  public covers: ICoverFile[];

  constructor(private readonly http: HttpClient) { }

  ngOnInit() {
  }

  public addFiles(input: HTMLInputElement) {
    const files = Array.from(input.files);
    const form = new FormData();
    const covers: ICoverFile[] = [];

    form.append('tag', 'books-catalog');

    files.forEach(file => {
      form.append('files', file);
      const {size, name} = file;
      const progress = 0;
      const cover: ICoverFile = {file, size, name, progress};
      const reader = new FileReader();
      reader.readAsDataURL(file);
      reader.addEventListener('load', (event) => {
        cover.src = reader.result;
      });
      covers.push(cover);
    });

    this.covers.push(...covers);

    const request = new HttpRequest('POST', '/api/files', form, {
      reportProgress: true,
    });

    this.http.request<IFilesUploaded>(request).subscribe(event => {
      let progress: number;

      if (event.type === HttpEventType.UploadProgress) {
        progress = Math.round(100 * event.loaded / event.total);
      } else if (event.type === HttpEventType.Response) {
        progress = 0;
        setCoverCodes(event.body, covers);
      } else {
        console.log('event: ', event);
      }

      covers.forEach(x => x.progress = progress);
    });

    input.value = null;
  }
}

function setCoverCodes(response: IFilesUploaded, covers: ICoverFile[]) {
  const files = response.files;
  for (const code in files) {
    if (files.hasOwnProperty(code)) {
      const file = files[code];
      const cover = covers.filter(x => x.name === file).pop();
      if (cover) {
        cover.code = code;
      }
    }
  }
}

export interface IFilesUploaded {
  files: {
    [key: string]: string;
  };
  totalSize: number;
}

export interface ICoverFile {
  name?: string;
  size?: number;
  file?: File;
  progress?: number;
  code?: string;
  src?: string | ArrayBuffer;
}
