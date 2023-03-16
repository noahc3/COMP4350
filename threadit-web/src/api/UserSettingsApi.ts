import { ApiEndpoint } from "../constants/ApiConstants";
import { IUserSettings } from "../models/UserSettings";
import { getWithAuth } from "./Request";
import { spoolStore } from "../stores/SpoolStore";

const removeSpoolForUserEndpoint = ApiEndpoint("/v1/userSettings/remove/");
const joinSpoolForUserEndpoint = ApiEndpoint("/v1/userSettings/join/");
const checkSpoolForUserEndpoint = ApiEndpoint("/v1/userSettings/check/");
const getUserInterestsEndpoint = ApiEndpoint("/v1/userSettings/interests");
const addUserInterestsEndpoint = ApiEndpoint("/v1/userSettings/addInterest/");
const removeUserInterestsEndpoint = ApiEndpoint("/v1/userSettings/removeInterest/");

export default class UserSettingsAPI {

    static async removeSpoolUser(spoolName: string): Promise<IUserSettings[]> {
        const response = await getWithAuth(removeSpoolForUserEndpoint + spoolName);

        if (!response.ok) {
            throw new Error(`Failed to remove the user from the spool: ${await response.text()}`);
        }

        spoolStore.refreshJoinedSpools();

        return await response.json();
    }

    static async joinSpoolUser(spoolName: string): Promise<IUserSettings[]> {
        const response = await getWithAuth(joinSpoolForUserEndpoint + spoolName);

        if (!response.ok) {
            throw new Error(`Failed to add the user to the spool: ${await response.text()}`);
        }

        spoolStore.refreshJoinedSpools();

        return await response.json();
    }

    static async getJoinedSpool(spoolName: string): Promise<boolean> {
        const response = await getWithAuth(checkSpoolForUserEndpoint + spoolName);

        if (!response.ok) {
            throw new Error(`Failed to check if the user is in the spool: ${await response.text()}`);
        }

        return await response.json();
    }

    static async getUserInterests(): Promise<string[]> {
        const response = await getWithAuth(getUserInterestsEndpoint);

        if (!response.ok) {
            throw new Error(`Failed to get user interests: ${await response.text()}`);
        }

        return await response.json();
    }

    static async addUserInterest(interest: string): Promise<string[]> {
        const response = await getWithAuth(addUserInterestsEndpoint + interest);

        if (!response.ok) {
            throw new Error(`Failed to add new interest to user: ${await response.text()}`);
        }

        return await response.json();
    }

    static async removeUserInterest(interest: string): Promise<string[]> {
        const response = await getWithAuth(removeUserInterestsEndpoint + interest);

        if (!response.ok) {
            throw new Error(`Failed to remove interest from user: ${await response.text()}`);
        }

        return await response.json();
    }
}
