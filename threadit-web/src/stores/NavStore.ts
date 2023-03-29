import { createBrowserHistory } from "history";
import { makeObservable, observable, action } from "mobx";
import queryString from "query-string";

export class NavStore {
  history = createBrowserHistory();

  @observable
  currentPath: string = "/";

  constructor() {
    makeObservable(this);
    this.history.listen(() => {
      this.currentPath = this.history.location.pathname;
    });

    this.currentPath = this.history.location.pathname;
  }

  @action
  async navigateTo(path: string, query?: Record<string, string>) {
    const search = query ? `/?${queryString.stringify(query)}` : "";
    this.history.push(`${path}${search}`);
  }
}

export const navStore = new NavStore();
