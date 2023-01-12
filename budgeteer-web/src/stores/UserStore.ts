import { v4 as uuidv4 } from 'uuid';
import { makeObservable, action, observable, computed } from "mobx";
import { fakeApiDelay } from "../constants/helpers";
import { AccountBank, AccountType, IAccount } from "../models/Account";
import { IUserProfile } from '../models/UserProfile';
import UserAPI from '../api/UserAPI';

export class UserStore {
    _accountsStub: IAccount[] = [
        {
            id: "0",
            name: "CIBC Chequing",
            balance: 729422,
            type: AccountType.CHEQUING,
            bank: AccountBank.CIBC_PERSONAL,
            lastImport: new Date('2021-01-01')
        },
        {
            id: "1",
            name: "CIBC Savings",
            balance: 519422,
            type: AccountType.SAVINGS,
            bank: AccountBank.CIBC_PERSONAL,
            lastImport: new Date('2021-01-01')
        },
        {
            id: "2",
            name: "CIBC VISA",
            balance: -25333,
            type: AccountType.CREDIT_CARD,
            bank: AccountBank.CIBC_PERSONAL,
            lastImport: new Date('2021-01-01')
        },
        {
            id: "3",
            name: "PayPal",
            balance: 3724,
            type: AccountType.CHEQUING,
            bank: AccountBank.PAYPAL_PERSONAL,
            lastImport: new Date('2021-01-01')
        },
        {
            id: "4",
            name: "Stack Prepaid",
            balance: 14212,
            type: AccountType.CHEQUING,
            bank: AccountBank.STACK,
            lastImport: new Date('2021-01-01')
        }
    ];

    @observable
    _userProfile?: IUserProfile = undefined;

    @observable
    accounts?: IAccount[] = undefined;

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

    @action
    async getAccounts() {
        await fakeApiDelay();
        this.accounts = [...this._accountsStub]
    }

    @action
    async addAccount(account: IAccount) {
        await fakeApiDelay();
        account.id = uuidv4();
        this._accountsStub.push(account);
        await this.getAccounts();
    }

    @action 
    async updateAccount(account: IAccount) {
        await fakeApiDelay();
        this._accountsStub = this._accountsStub?.map(a => a.id === account.id ? account : a);
        await this.getAccounts();
    }

    @action
    async deleteAccount(account: IAccount) {
        await fakeApiDelay();
        this._accountsStub = this._accountsStub?.filter(a => a.id !== account.id);
        await this.getAccounts();
    }

}

export const userStore = new UserStore();

