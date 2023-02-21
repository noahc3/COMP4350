import { makeObservable, action, observable, computed, runInAction } from "mobx";
import { IUserProfile } from '../models/UserProfile';
import UserAPI from '../api/UserAPI';

export class UserStore {

    @observable
    _userProfile?: IUserProfile = undefined;
    
    constructor() {
        makeObservable(this);
    }

    @action
    async refreshUserProfile() {
        const profile = await UserAPI.getUserProfile();
        runInAction(() => {
            this._userProfile = profile;
        })
        return this._userProfile
    }

    @action
    async clearUserProfile() {
        runInAction(() => {
            this._userProfile = undefined;
        })
    }

    @computed
    get userProfile() {
        if (this._userProfile === undefined) this.refreshUserProfile();
        return this._userProfile;
    }
}

export const userStore = new UserStore();

