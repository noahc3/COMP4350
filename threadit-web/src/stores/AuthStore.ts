import { makeObservable, action, observable } from "mobx";
import AuthAPI from "../api/AuthAPI";

export class AuthStore {
    @observable
    userNeedsAuthentication: boolean = false;

    @observable
    isAuthenticated: boolean = false;

    @observable
    sessionToken: string | null = localStorage.getItem("session-token");

    constructor() {
        makeObservable(this);
    }

    @action
    async login(username: string, password: string): Promise<boolean | string> {
        const success = await AuthAPI.login(username, password);
        if (success) {
            this.sessionToken = localStorage.getItem("session-token");
            this.isAuthenticated = true;
            this.userNeedsAuthentication = false;
        }
        return success;
    }

    @action
    async register(email: string, username: string, password: string): Promise<boolean | string> {
        try {
            const success = await AuthAPI.register(email, username, password);
            return success;
        } catch (error: any) {
            return error.message;
        }
    }

    @action
    async checkAuthentication() {        
        if (this.sessionToken === null) {
            this.userNeedsAuthentication = true;
            this.isAuthenticated = false;
            return;
        }

        const success = await AuthAPI.checkSession(this.sessionToken);

        if (!success) {
            this.userNeedsAuthentication = true;
            this.isAuthenticated = false;
            return;
        }

        this.isAuthenticated = true;
        this.userNeedsAuthentication = false;
    }

}

export const authStore = new AuthStore();

