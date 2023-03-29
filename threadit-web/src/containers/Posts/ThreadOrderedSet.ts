import { IThreadFull } from "../../models/ThreadFull";

export class ThreadOrderedSet {
  public list: IThreadFull[];

  constructor() {
    this.list = [];
  }

  public addThread(thread: IThreadFull) {
    const index = this.list.findIndex((t) => t.id === thread.id);

    if (index === -1) {
      this.list.push(thread);
    } else {
      this.list[index] = thread;
    }
  }

  public addThreads(threads: IThreadFull[]) {
    threads.forEach((t) => this.addThread(t));
  }

  public setThreads(threads: IThreadFull[]) {
    this.list = threads;
  }
}
