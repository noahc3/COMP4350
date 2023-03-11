import { makeObservable, action, observable, computed, runInAction } from "mobx";
import { IUserProfile } from '../models/UserProfile';
import SpoolAPI from '../api/SpoolAPI';

export class SpoolUsersStore {
    @observable
    _moderators?: IUserProfile[] = undefined;

    constructor() {
        makeObservable(this);
    }

    @action
    async refreshAllModerators(spoolId: string) {
        const moderators = await SpoolAPI.getAllMods(spoolId);
        runInAction(() => {
            this._moderators = moderators;
        })
        return this._moderators
    }

    @computed
    get moderators() {
        return this._moderators;
    }
}

export const spoolUsersStore = new SpoolUsersStore();