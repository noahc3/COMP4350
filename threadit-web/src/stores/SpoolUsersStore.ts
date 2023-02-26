import { makeObservable, action, observable, computed, runInAction } from "mobx";
import { IUserProfile } from '../models/UserProfile';
import { spoolStore } from "./SpoolStore";
import SpoolAPI from '../api/SpoolAPI';

export class SpoolUsersStore {
    @observable
    _nonModerators?: IUserProfile[] = undefined;
    _moderators?: IUserProfile[] = undefined;
    _users?: IUserProfile[] = undefined;

    constructor() {
        makeObservable(this);
    }

    @action
    async refreshAllNonModerator(spoolId: string, userId: string) {
        const _nonModerators = await SpoolAPI.getAllNonModerator(spoolId, userId);
        runInAction(() => {
            this._nonModerators = _nonModerators;
        })
        return this._nonModerators
    }

    @action
    async refreshAllModerators(spoolId: string) {
        const moderators = await SpoolAPI.getAllMods(spoolId);
        runInAction(() => {
            this._moderators = moderators;
        })
        return this._moderators
    }

    @action
    async refreshAllUsers(spoolId: string, userId: string) {
        const _users = await SpoolAPI.getAllUsers(spoolId, userId);
        runInAction(() => {
            this._users = _users;
        })
        return this._users
    }

    @computed
    get moderators() {
        return this._moderators;
    }

    @computed
    get nonModerators() {
        return this._nonModerators;
    }

    @computed
    get users() {
        return this._users;
    }
}

export const spoolUsersStore = new SpoolUsersStore();