import { makeObservable, action, observable, computed } from "mobx";
import { User } from "oidc-client-ts";
import { AuthService } from "../services/AuthService/AuthService";
import { navStore } from "./NavStore";

export class AuthStore {
    @observable
    authService = new AuthService();

    @observable
    currentAuthUser: User | null = null;

    @observable
    userNeedsAuthentication: boolean = false;

    constructor() {
        makeObservable(this);
    }

    @action
    async getAuthUser() {
        const authUser = await this.authService.getUser();
        this.currentAuthUser = authUser;
        return authUser;
    }

    @action
    async login() {
        await this.authService.login();
    }

    @action
    async logout() {
        await this.authService.logout();
    }

    @action
    async signinCallback() {
        try {
            await this.authService.signinCallback();
            await this.getAuthUser();
            navStore.navigateTo("/dashboard");
        } catch (e) {
            console.error(e);
            navStore.navigateTo("/");
        }
    }

    @action
    async checkAuthentication() {
        if (navStore.currentPath === "/signin-callback") {
            await this.signinCallback();
            return;
        }

        const authUser = await authStore.getAuthUser();
    
        if (authUser) {
            console.info("User has been loaded from store");
        } else {
            console.info("User is not logged in")
            this.userNeedsAuthentication = true;
        }
    }

    @computed
    get isAuthenticated() {
        return this.currentAuthUser !== null;
    }

}

export const authStore = new AuthStore();

