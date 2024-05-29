// src/app/services/deadline.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

interface DeadlineResponse {
  secondsLeft: number;
  deadlineDate: string;
}

@Injectable({
  providedIn: 'root',
})
export class DeadlineService {
  private apiUrl = '/api/deadline';

  constructor(private http: HttpClient) {}

  getDeadline(): Observable<DeadlineResponse> {
    return this.http.get<DeadlineResponse>(this.apiUrl);
  }
}
