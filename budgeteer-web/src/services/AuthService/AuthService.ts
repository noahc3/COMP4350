import { User, UserManager, WebStorageStateStore, Log } from 'oidc-client-ts';
import { AuthServiceConstants } from './AuthServiceConstants';

export interface IAuthService {
    getUser(): Promise<User | null>;
    login(): Promise<void>;
    logout(): Promise<void>;
    renewToken(): Promise<void>;
    signinCallback(): Promise<void>;
}

export class AuthService implements IAuthService {
    private _userManager: UserManager;

    constructor() {
        const settings = {
            authority: AuthServiceConstants.stsAuthority,
            client_id: AuthServiceConstants.clientId,
            redirect_uri: `${AuthServiceConstants.clientRoot}signin-callback`,
            silent_redirect_uri: `${AuthServiceConstants.clientRoot}silent-renew.html`,
            // tslint:disable-next-line:object-literal-sort-keys
            post_logout_redirect_uri: `${AuthServiceConstants.clientRoot}`,
            response_type: 'code',
            scope: AuthServiceConstants.clientScope,
            userStore: new WebStorageStateStore({ store: window.localStorage }),
        };

        this._userManager = new UserManager(settings);

        Log.setLogger(console);
        Log.setLevel(Log.INFO);

        this._userManager.events.addAccessTokenExpired(() => console.warn("Access token expired"));
    }

    public async getUser(): Promise<User | null> {
        return await this._userManager.getUser();
    }

    public async login(): Promise<void> {
        await this._userManager.signinRedirect();
    }

    public async logout(): Promise<void> {
        await this._userManager.signoutRedirect();
    }

    public async renewToken(): Promise<void> {
        await this._userManager.signinSilent();
    }

    public async signinCallback(): Promise<void> {
        await this._userManager.signinRedirectCallback();
    }
}