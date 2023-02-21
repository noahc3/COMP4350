import { ApiEndpoint } from "../constants/ApiConstants";
import { IUserSettings } from "../models/UserSettings";
import { get } from "./Request";
import { spoolStore } from "../stores/SpoolStore";

const removeSpoolForUserEndpoint = ApiEndpoint("/v1/userSettings/remove/");
const joinSpoolForUserEndpoint = ApiEndpoint("/v1/userSettings/join/");
const checkSpoolForUserEndpoint = ApiEndpoint("/v1/userSettings/check/");

export default class UserSettingsAPI {

    static async removeSpoolUser(userId: string, spoolName: string): Promise<IUserSettings[]> {
        const response = await get(removeSpoolForUserEndpoint + userId + "/" + spoolName);

        if (!response.ok) {
            throw new Error(`Failed to remove the user from the spool: ${await response.text()}`);
        }

        spoolStore.refreshJoinedSpools();

        return await response.json();
    }

    static async joinSpoolUser(userId: string, spoolName: string): Promise<IUserSettings[]> {
        const response = await get(joinSpoolForUserEndpoint + userId + "/" + spoolName);

        if (!response.ok) {
            throw new Error(`Failed to add the user to the spool: ${await response.text()}`);
        }

        spoolStore.refreshJoinedSpools();

        return await response.json();
    }

    static async getJoinedSpool(userId: string, spoolName: string): Promise<boolean> {
        const response = await get(checkSpoolForUserEndpoint + userId + "/" + spoolName);

        if (!response.ok) {
            throw new Error(`Failed to check if the user is in the spool: ${await response.text()}`);
        }

        return await response.json();
    }
}
