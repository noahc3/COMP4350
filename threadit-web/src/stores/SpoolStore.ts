import { makeObservable, action, observable, computed } from "mobx";
import { ISpool } from '../models/Spool';
import SpoolAPI from '../api/SpoolAPI';

export class SpoolStore {

    @observable
    _allSpools?: ISpool[] = undefined;
    _joinedSpools?: ISpool[] = undefined;

    constructor() {
        makeObservable(this);
    }

    @action
    async refreshAllSpools() {
        this._allSpools = await SpoolAPI.getAllSpools();
        return this._allSpools
    }

    @action
    async refreshJoinedSpools() {
        this._joinedSpools = await SpoolAPI.getJoinedSpools();
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
        if (this._joinedSpools === undefined) this.refreshJoinedSpools();

        return this._joinedSpools;
    }
}

export const spoolStore = new SpoolStore();