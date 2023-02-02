import { createBrowserHistory } from "history";
import { makeObservable, observable, action } from "mobx";

export class NavStore {
    history = createBrowserHistory();

    @observable
    currentPath: string = "/"

    constructor() {
        makeObservable(this);
        this.history.listen(() => {
            this.currentPath = this.history.location.pathname;
        });

        this.currentPath = this.history.location.pathname;
    }

    @action
    async navigateTo(path: string) {
        this.history.push(path);
    }
}

export const navStore = new NavStore();