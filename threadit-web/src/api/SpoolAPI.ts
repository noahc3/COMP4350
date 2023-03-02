import { ApiEndpoint } from "../constants/ApiConstants";
import { ISpool } from "../models/Spool";
import { IThreadFull } from "../models/ThreadFull";
import { get, post, postWithAuth } from "./Request";
import { spoolStore } from "../stores/SpoolStore";
import UserAPI from '../api/UserAPI';
import { IUserProfile } from "../models/UserProfile";

const spoolEndpoint = ApiEndpoint("/v1/spool/");
const spoolThreadsEndpoint = ApiEndpoint('/v1/spool/threads/');
const postSpoolEndpoint = ApiEndpoint('/v1/spool/create');
const allSpoolsEndpoint = ApiEndpoint('/v1/spool/all');
const joinedSpoolsEndpoint = ApiEndpoint('/v1/spool/joined/');
const allModsForSpoolEndpoint = ApiEndpoint('/v1/spool/mods/');
const removeModeratorEndpoint = ApiEndpoint('/v1/spool/mods/remove/');
const addModeratorEndpoint = ApiEndpoint('/v1/spool/mods/add/');
const changeOwnerEndpoint = ApiEndpoint('/v1/spool/change/');
const deleteSpoolEndpoint = ApiEndpoint('/v1/spool/delete/');
const saveSpoolEndpoint = ApiEndpoint('/v1/spool/save/');

export default class SpoolAPI {
    static async getSpoolThreads(spoolId: string): Promise<IThreadFull[]> {
        const response = await get(spoolThreadsEndpoint + spoolId);

        if (!response.ok) {
            throw new Error(`Failed to get all spool threads: ${await response.text()}`);
        }

        return await response.json();
    }

    static async getSpoolByName(spoolName: string): Promise<ISpool> {
        const response = await get(spoolEndpoint + spoolName);

        if (!response.ok) {
            throw new Error(`Failed to get spool: ${await response.text()}`);
        }

        return await response.json();
    }

    static async createSpool(name: string, ownerId: string, interests: string[], moderators: string[]): Promise<ISpool> {
        const response = await postWithAuth(postSpoolEndpoint, {
            name,
            ownerId,
            interests,
            moderators,
        });

        if (!response.ok) {
            throw new Error(`Failed to post spool: ${await response.text()}`);
        }

        spoolStore.refreshAllSpools();
        spoolStore.refreshJoinedSpools();

        return await response.json();
    }

    static async getAllSpools(): Promise<ISpool[]> {
        const response = await get(allSpoolsEndpoint);

        if (!response.ok) {
            throw new Error(`Failed to get all spools: ${await response.text()}`);
        }

        return await response.json();
    }

    static async getJoinedSpools(): Promise<ISpool[]> {
        const userProfile = await UserAPI.getUserProfile();
        const response = await get(joinedSpoolsEndpoint + userProfile.id);

        if (!response.ok) {
            throw new Error(`Failed to get joined spools: ${await response.text()}`);
        }

        return await response.json();
    }

    static async getAllMods(spoolId: string): Promise<IUserProfile[]> {
        const response = await get(allModsForSpoolEndpoint + spoolId);

        if (!response.ok) {
            throw new Error(`Failed to get all mods for the spool: ${await response.text()}`);
        }

        return await response.json();
    }

    static async removeModerator(spoolId: string, userId: string): Promise<IUserProfile> {
        const response = await get(removeModeratorEndpoint + spoolId + '/' + userId);

        if (!response.ok) {
            throw new Error(`Failed to remove moderator from spool: ${await response.text()}`);
        }

        return await response.json();
    }

    static async addModerator(spoolId: string, userName: string): Promise<number> {
        const response = await get(addModeratorEndpoint + spoolId + '/' + userName);
        if (response.ok) {
            //returns 1 if the add was successful
            return 1;
        }

        else {
            //otherwise change failed and need to see why
            const errorText = await response.text();
            if (errorText === "User does not exist.") {
                return 2; // return type 2 if user does not exist
            } else if (errorText === "User is already a mod.") {
                return 3; // return type 3 if user is already a mod
            } else if (errorText === "Cannot add owner as moderator.") {
                return 4; // return type 3 if user is already a mod
            } else {
                console.error(`Failed to add the user as a mod: ${errorText}`);
                return 0; // return type 0 for any other error
            }
        }
    }

    

    static async changeOwner(spoolId: string, userName: string): Promise<number> {
        const response = await get(changeOwnerEndpoint + spoolId + '/' + userName);

        if (response.ok) {
            //returns 1 if the change was successful
            return 1;
        }

        else {
            //otherwise change failed and need to see why
            const errorText = await response.text();
            if (errorText === "User is already the owner.") {
                return 2; // return type 2 if user is already the owner
            } else if (errorText === "User does not exist.") {
                return 3; // return type 3 if user does not exist
            } else {
                console.error(`Failed to change the owner of the spool: ${errorText}`);
                return 0; // return type 0 for any other error
            }
        }
    }

    static async deleteSpool(spoolId: string): Promise<void> {
        const response = await get(deleteSpoolEndpoint + spoolId);

        if (!response.ok) {
            throw new Error(`Failed to delete the spool: ${await response.text()}`);
        }

        spoolStore.refreshAllSpools();
        spoolStore.refreshJoinedSpools();
    }

    static async saveSpool(spoolId: string, rules: string): Promise<void> {
        const response = await post(saveSpoolEndpoint + spoolId, {
            rules
        });

        if (!response.ok) {
            console.error(`Failed to save the spool: ${await response.text()}`);
        }
    }
}
