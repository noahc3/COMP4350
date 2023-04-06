import { ApiEndpoint } from "../constants/ApiConstants";
import { ISpool } from "../models/Spool";
import { IThreadFull } from "../models/ThreadFull";
import { get, getWithAuth, postWithAuth } from "./Request";
import { spoolStore } from "../stores/SpoolStore";
import UserAPI from "../api/UserAPI";
import { IUserProfile } from "../models/UserProfile";

const spoolEndpoint = ApiEndpoint("/v1/spool/");
const spoolThreadsEndpoint = ApiEndpoint("/v1/spool/threads/");
const postSpoolEndpoint = ApiEndpoint("/v1/spool/create");
const allSpoolsEndpoint = ApiEndpoint("/v1/spool/all");
const joinedSpoolsEndpoint = ApiEndpoint("/v1/spool/joined/");
const suggestedSpoolsEndpoint = ApiEndpoint("/v1/spool/suggested/");
const allModsForSpoolEndpoint = ApiEndpoint("/v1/spool/mods/");
const removeModeratorEndpoint = ApiEndpoint("/v1/spool/mods/remove/");
const addModeratorEndpoint = ApiEndpoint("/v1/spool/mods/add/");
const changeOwnerEndpoint = ApiEndpoint("/v1/spool/change/");
const deleteSpoolEndpoint = ApiEndpoint("/v1/spool/delete/");
const saveSpoolEndpoint = ApiEndpoint("/v1/spool/save/");

export default class SpoolAPI {
  static async getSpoolThreads(
    spoolId: string,
    sortType: string,
    searchWord: string
  ): Promise<IThreadFull[]> {
    const response = await get(
      spoolThreadsEndpoint +
        spoolId +
        (sortType === "" ? "" : "/" + sortType) +
        (searchWord === "" ? "" : "/?q=" + searchWord)
    );

    if (!response.ok) {
      throw new Error(
        `Failed to get all spool threads: ${await response.text()}`
      );
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

  static async createSpool(
    name: string,
    ownerId: string,
    interests: string[],
    moderators: string[]
  ): Promise<ISpool> {
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

  static async getSuggestedSpools(): Promise<ISpool[]> {
    const userProfile = await UserAPI.getUserProfile();
    const response = await get(suggestedSpoolsEndpoint + userProfile.id);

    if (!response.ok) {
      throw new Error(
        `Failed to get suggested spools: ${await response.text()}`
      );
    }

    return await response.json();
  }

  static async getAllMods(spoolId: string): Promise<IUserProfile[]> {
    const response = await get(allModsForSpoolEndpoint + spoolId);

    if (!response.ok) {
      throw new Error(
        `Failed to get all mods for the spool: ${await response.text()}`
      );
    }

    return await response.json();
  }

  static async removeModerator(
    spoolId: string,
    userId: string
  ): Promise<IUserProfile> {
    const response = await getWithAuth(
      removeModeratorEndpoint + spoolId + "/" + userId
    );

    if (!response.ok) {
      throw new Error(
        `Failed to remove moderator from spool: ${await response.text()}`
      );
    }

    return await response.json();
  }

  static async addModerator(
    spoolId: string,
    userName: string
  ): Promise<boolean | string> {
    const response = await getWithAuth(
      addModeratorEndpoint + spoolId + "/" + userName
    );
    if (response.ok) {
      //returns 1 if the add was successful
      return true;
    } else {
      //otherwise change failed and need to see why
      const errorText = await response.text();
      return errorText;
    }
  }

  static async changeOwner(
    spoolId: string,
    userName: string
  ): Promise<boolean | string> {
    const response = await getWithAuth(
      changeOwnerEndpoint + spoolId + "/" + userName
    );

    if (response.ok) {
      //returns 1 if the change was successful
      return true;
    } else {
      //otherwise change failed and need to see why
      const errorText = await response.text();
      return errorText;
    }
  }

  static async deleteSpool(spoolId: string): Promise<void> {
    const response = await getWithAuth(deleteSpoolEndpoint + spoolId);

    if (!response.ok) {
      throw new Error(`Failed to delete the spool: ${await response.text()}`);
    }

    spoolStore.refreshAllSpools();
    spoolStore.refreshJoinedSpools();
  }

    static async saveSpool(spoolId: string, rules: string): Promise<void> {
        const response = await postWithAuth(saveSpoolEndpoint + spoolId, {
            rules,
        });

        if (!response.ok) {
            throw new Error(`Failed to save the rules: ${await response.text()}`);
        }
  }
}
