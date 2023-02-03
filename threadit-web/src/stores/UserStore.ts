import { makeObservable, action, observable, computed } from "mobx";
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
        this._userProfile = await UserAPI.getUserProfile();
        return this._userProfile
    }

    @computed
    get userProfile() {
        if (this._userProfile === undefined) this.refreshUserProfile();

        return this._userProfile;
    }

}

export const userStore = new UserStore();

