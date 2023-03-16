import { ApiEndpoint } from "../constants/ApiConstants";
import { getWithAuth, post } from "./Request";

const loginEndpoint = ApiEndpoint('/v1/auth/login');
const registerEndpoint = ApiEndpoint('/v1/auth/register');
const checkSessionEndpoint = ApiEndpoint('/v1/auth/checksession');
const logoutEndpoint = ApiEndpoint('/v1/auth/logout');

export default class AuthAPI {
    static async login(username: string, password: string): Promise<boolean> {
        const response = await post(loginEndpoint, {
            username,
            password
        });
    
        if (!response.ok) {
            console.error(`Failed to login: ${await response.text()}`);
        } else {
            localStorage.setItem("session-token", await response.text());
        }

        return response.ok;
    }

    static async register(email: string, username: string, password: string): Promise<boolean> {
        const response = await post(registerEndpoint, {
            email,
            username,
            password,
        });
    
        if (!response.ok) {
            throw new Error(`Failed to register: ${await response.text()}`);
        }
    
        return true;
    }

    static async checkSession(token: string): Promise<boolean> {
        const response = await post(checkSessionEndpoint, {
            token
        });

        return response.ok;
    }

    static async logout(): Promise<void> {
        const response = await getWithAuth(logoutEndpoint);
        if (!response.ok) {
            throw new Error(`Failed to logout: ${await response.text()}`);
        }
    }
}
