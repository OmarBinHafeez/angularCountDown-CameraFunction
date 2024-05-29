import { Component, OnInit, OnDestroy } from '@angular/core';
import { interval, Subscription } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { DeadlineService } from './deadline.service';

@Component({
  selector: 'app-countdown',
  templateUrl: './countdown.component.html',
  styleUrls: ['./countdown.component.css'],
})
export class CountdownComponent implements OnInit, OnDestroy {
  secondsLeft: number = 0;
  deadlineDate: string = '';
  private updateSubscription: Subscription | null = null;

  constructor(private deadlineService: DeadlineService) {}

  ngOnInit() {
    // Fetch initial deadline and set up periodic updates
    this.updateSubscription = this.deadlineService
      .getDeadline()
      .pipe(
        switchMap((response) => {
          this.secondsLeft = response.secondsLeft;
          this.deadlineDate = response.deadlineDate;
          // Use interval to create a timer that decrements every second
          return interval(1000);
        })
      )
      .subscribe(() => {
        if (this.secondsLeft > 0) {
          this.secondsLeft--;
        } else {
          //Handle the case where the countdown reaches zero
          this.updateSubscription?.unsubscribe();
        }
      });
  }

  ngOnDestroy() {
    // Clean up subscription to avoid memory leaks
    this.updateSubscription?.unsubscribe();
  }
}
