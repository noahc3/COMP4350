import { makeObservable, action, observable, runInAction } from "mobx";
import AuthAPI from "../api/AuthAPI";
import { navStore } from "./NavStore";
import { spoolStore } from "./SpoolStore";
import { userStore } from "./UserStore";

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
      runInAction(() => {
        this.sessionToken = localStorage.getItem("session-token");
        this.isAuthenticated = true;
        this.userNeedsAuthentication = false;
      });
      await userStore.refreshUserProfile();
      await spoolStore.refreshJoinedSpools();
    }
    return success;
  }

  @action
  async register(
    email: string,
    username: string,
    password: string
  ): Promise<boolean | string> {
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
      runInAction(() => {
        this.userNeedsAuthentication = true;
        this.isAuthenticated = false;
      });
      return;
    }

    const success = await AuthAPI.checkSession(this.sessionToken);

    if (!success) {
      runInAction(() => {
        this.userNeedsAuthentication = true;
        this.isAuthenticated = false;
      });
      return;
    }

    runInAction(() => {
      this.isAuthenticated = true;
      this.userNeedsAuthentication = false;
    });
  }

  @action
  async logout(): Promise<void> {
    try {
      await AuthAPI.logout();
    } finally {
      runInAction(() => {
        this.sessionToken = null;
        this.isAuthenticated = false;
        this.userNeedsAuthentication = true;
      });
      userStore.clearUserProfile();
      localStorage.removeItem("session-token");
      navStore.navigateTo("/");
    }
  }
}

export const authStore = new AuthStore();
