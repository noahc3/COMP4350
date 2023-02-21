import { makeObservable, action, observable, computed, runInAction } from "mobx";
import { ISpool } from '../models/Spool';
import SpoolAPI from '../api/SpoolAPI';
import { authStore } from "./AuthStore";

export class SpoolStore {

    @observable
    _allSpools?: ISpool[] = undefined;

    @observable
    _joinedSpools?: ISpool[] = undefined;

    constructor() {
        makeObservable(this);
    }

    @action
    async refreshAllSpools() {
        const spools = await SpoolAPI.getAllSpools();
        runInAction(() => {
            this._allSpools = spools;
        })
        return this._allSpools
    }

    @action
    async refreshJoinedSpools() {
        if (authStore.isAuthenticated) {
            const spools = await SpoolAPI.getJoinedSpools();
            runInAction(() => {
                this._joinedSpools = spools;
            })
        } else {
            runInAction(() => {
                this._joinedSpools = [];
            })
        }
        return this._joinedSpools
    }

    @action
    async clearAllSpools() {
        this._allSpools = undefined;
    }

    @action
    async clearJoinedSpools() {
        this._joinedSpools = undefined;
    }

    @computed
    get allSpools() {
        if (this._allSpools === undefined) this.refreshAllSpools();

        return this._allSpools;
    }

    @computed
    get joinedSpools() {
        return this._joinedSpools;
    }
}

export const spoolStore = new SpoolStore();