import { makeObservable, action, observable, computed, runInAction } from "mobx";
import { ISpool } from '../models/Spool';
import { authStore } from "./AuthStore";
import SpoolAPI from '../api/SpoolAPI';

export class SpoolStore {

    @observable
    _allSpools?: ISpool[] = undefined;

    @observable
    _joinedSpools?: ISpool[] = undefined;

    @observable
    _suggestedSpools?: ISpool[] = undefined;

    @observable
    _currentSpool?: ISpool = undefined;

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
    async refreshSpool(spool: ISpool) {
        this._currentSpool = spool;
        return this._currentSpool
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
    async refreshSuggestedSpools() {
        if (authStore.isAuthenticated) {
            const spools = await SpoolAPI.getSuggestedSpools();
            runInAction(() => {
                this._suggestedSpools = spools;
            })
        } else {
            runInAction(() => {
                this._suggestedSpools = [];
            })
        }
        return this._suggestedSpools
    }

    @action
    async clearAllSpools() {
        this._allSpools = undefined;
    }

    @action
    async clearJoinedSpools() {
        this._joinedSpools = undefined;
    }

    @action
    async clearCurrentSpool() {
        this._currentSpool = undefined;
    }

    @action
    async clearSuggestedSpools() {
        this._suggestedSpools = undefined;
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

    @computed
    get currentSpool() {
        return this._currentSpool;
    }

    @computed
    get suggestedSpools() {
        return this._suggestedSpools;
    }
}

export const spoolStore = new SpoolStore();