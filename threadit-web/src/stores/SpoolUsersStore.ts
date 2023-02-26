import { makeObservable, action, observable, computed, runInAction } from "mobx";
import { IUserProfile } from '../models/UserProfile';
import { spoolStore } from "./SpoolStore";
import SpoolAPI from '../api/SpoolAPI';

export class SpoolUsersStore {
    @observable
    _users?: IUserProfile[] = undefined;
    _moderators?: IUserProfile[] = undefined;

    constructor() {
        makeObservable(this);
    }

    @action
    async refreshAllUsers(spoolId: string, userId: string) {
        const users = await SpoolAPI.getAllUsers(spoolId, userId);
        runInAction(() => {
            this._users = users;
        })
        return this._users
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

    @computed
    get users() {
        return this._users;
    }
}

export const spoolUsersStore = new SpoolUsersStore();