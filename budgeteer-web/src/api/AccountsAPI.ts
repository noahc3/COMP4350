import { ApiEndpoint } from "../constants/ApiConstants";
import { IAccount } from "../models/Account";
import { IUserProfile } from "../models/UserProfile";
import { deleteWithAuth, getWithAuth, postWithAuth, putWithAuth } from "./Request";

const accountEndpoint = ApiEndpoint('/v1/account');

export default class AccountsAPI {
    static async getAccounts(): Promise<IAccount[]> {
        const response = await getWithAuth(accountEndpoint);
    
        if (!response.ok) {
            throw new Error(`Failed to get accounts: ${await response.text()}`);
        }

        return await response.json();
    }

    static async addAccount(account: IAccount): Promise<IAccount> {
        const response = await postWithAuth(accountEndpoint, account);

        if (!response.ok) {
            throw new Error(`Failed to add account: ${await response.text()}`);
        }
        
        return await response.json();
    }

    static async updateAccount(account: IAccount): Promise<void> {
        const response = await putWithAuth(accountEndpoint, account);

        if (!response.ok) {
            throw new Error(`Failed to update account: ${await response.text()}`);
        }
    }

    static async deleteAccount(account: IAccount): Promise<void> {
        const route = `${accountEndpoint}/${account.id}`;
        const response = await deleteWithAuth(route);

        if (!response.ok) {
            throw new Error(`Failed to delete account: ${await response.text()}`);
        }
    }
}
