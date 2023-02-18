import { makeObservable, action, observable, computed } from "mobx";
import { ISpool } from '../models/Spool';
import SpoolAPI from '../api/SpoolAPI';

export class SpoolStore {

    @observable
    _spools?: ISpool[] = undefined;

    constructor() {
        makeObservable(this);
    }

    @action
    async refreshSpools() {
        this._spools = await SpoolAPI.getAllSpools();
        return this._spools
    }

    @action
    async clearSpools() {
        this._spools = undefined;
    }

    @computed
    get spools() {
        if (this._spools === undefined) this.refreshSpools();

        return this._spools;
    }
}

export const spoolStore = new SpoolStore();