import { ApiEndpoint } from "../constants/ApiConstants";
import { ISpool } from "../models/Spool";
import { IThreadFull } from "../models/ThreadFull";
import { get, postWithAuth } from "./Request";
import { spoolStore } from "../stores/SpoolStore";
import UserAPI from '../api/UserAPI';
import { IUserProfile } from "../models/UserProfile";

const spoolEndpoint = ApiEndpoint("/v1/spool/");
const spoolThreadsEndpoint = ApiEndpoint('/v1/spool/threads/');
const postSpoolEndpoint = ApiEndpoint('/v1/spool/create');
const allSpoolsEndpoint = ApiEndpoint('/v1/spool/all');
const joinedSpoolsEndpoint = ApiEndpoint('/v1/spool/joined/');
const allUsersForSpoolEndpoint = ApiEndpoint('/v1/spool/users/');
const allModsForSpoolEndpoint = ApiEndpoint('/v1/spool/mods/');
const removeModeratorEndpoint = ApiEndpoint('/v1/spool/mods/remove/');
const addModeratorEndpoint = ApiEndpoint('/v1/spool/mods/add/');

export default class SpoolAPI {
    static async getSpoolThreads(spoolId: string): Promise<IThreadFull[]> {
        const response = await get(spoolThreadsEndpoint + spoolId);

        if (!response.ok) {
            throw new Error(`Failed to get all spool threads: ${await response.text()}`);
        }

        return await response.json();
    }

    static async getSpoolById(spoolId: string): Promise<ISpool> {
        const response = await get(spoolEndpoint + spoolId);

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

    static async getAllUsers(spoolId: string, userId: string): Promise<IUserProfile[]> {
        const response = await get(allUsersForSpoolEndpoint + spoolId + "/" + userId);

        if (!response.ok) {
            throw new Error(`Failed to get all users who joined spool: ${await response.text()}`);
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

    static async removeModerator(spoolId: string, userId: string): Promise<IUserProfile[]> {
        const response = await get(removeModeratorEndpoint + spoolId + '/' + userId);

        if (!response.ok) {
            throw new Error(`Failed to remove moderator from spool: ${await response.text()}`);
        }

        return await response.json();
    }

    static async addModerator(spoolId: string, userId: string): Promise<IUserProfile[]> {
        const response = await get(addModeratorEndpoint + spoolId + '/' + userId);

        if (!response.ok) {
            throw new Error(`Failed to add moderator to spool: ${await response.text()}`);
        }

        return await response.json();
    }
}
