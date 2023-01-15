import { v4 as uuidv4 } from 'uuid';
import { makeObservable, action, observable, computed } from "mobx";
import { fakeApiDelay } from "../constants/helpers";
import AccountsAPI from "../api/AccountsAPI";
import { IAccount } from '../models/Account';

export class AccountStore {

    @observable
    accounts?: IAccount[] = undefined;

    constructor() {
        makeObservable(this);
    }

    @action
    async getAccounts() {
        this.accounts = await AccountsAPI.getAccounts();
    }

    @action
    async addAccount(account: IAccount) {
        await AccountsAPI.addAccount(account);
        await this.getAccounts();
    }

    @action 
    async updateAccount(account: IAccount) {
        await AccountsAPI.updateAccount(account);
        await this.getAccounts();
    }

    @action
    async deleteAccount(account: IAccount) {
        await AccountsAPI.deleteAccount(account);
        await this.getAccounts();
    }

}

export const accountStore = new AccountStore();

