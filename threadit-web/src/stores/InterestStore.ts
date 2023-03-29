import {
  makeObservable,
  action,
  observable,
  computed,
  runInAction,
} from "mobx";
import { IInterest } from "../models/Interest";
import { authStore } from "./AuthStore";
import InterestAPI from "../api/InterestAPI";
import UserSettingsAPI from "../api/UserSettingsApi";

export class InterestStore{
  @observable
  _allInterests?: string[] = undefined;

  @observable
  _joinedInterests?: string[] = undefined;

  @observable
  _otherInterests?: string[] = undefined;

  constructor() {
    makeObservable(this);
  } 

  @action
  async refreshAllInterests() {
    const interests = await InterestAPI.getAllInterests();
    runInAction(() => {
      const array = [];
      for(let i = 0; i < interests.length; i++){
            array.push(interests[i].name);
      }
      this._allInterests = array;
    });
    return this._allInterests;
  }

  @action
  async refreshJoinedInterests() {
    if(authStore.isAuthenticated){
      const interests = await UserSettingsAPI.getUserInterests();
      runInAction(() => {
        this._joinedInterests = interests;
    });
    }
    else{
        this._joinedInterests = [];
    }
    return this._joinedInterests;
  }

  @action
  async refreshOtherInterests() {
    if(authStore.isAuthenticated){
      const userInterests = await UserSettingsAPI.getUserInterests();
      const interests = await InterestAPI.getAllInterests();
      runInAction(() => {
        const array = [];
        for(let i = 0; i < interests.length; i++){
          array.push(interests[i].name);
        }
        this._otherInterests = array.filter(x => !userInterests.includes(x));
    });
    }
    else{
        this._otherInterests = [];
    }
    return this._otherInterests;
  }

  @action
  async clearAllInterests() {
    this._allInterests = undefined;
  }

  @action
  async clearJoinedInterests() {
    this._joinedInterests = undefined;
  }

  @action
  async clearOtherInterests() {
    this._otherInterests = undefined;
  }

  @computed
  get allInterests() {
    if (this._allInterests === undefined) this.refreshAllInterests();

    return this._allInterests;
  }

  @computed
  get joinedInterests() {
    return this._joinedInterests;
  }

  @computed
  get otherInterests() {
    return this._otherInterests;
  }
}

export const interestStore = new InterestStore();