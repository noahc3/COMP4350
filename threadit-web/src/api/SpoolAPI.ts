import { ApiEndpoint } from "../constants/ApiConstants";
import { ISpool } from "../models/Spool";
import { IThreadFull } from "../models/ThreadFull";
import { get } from "./Request";

const spoolEndpoint = ApiEndpoint("/v1/spool/");
const spoolThreadsEndpoint = ApiEndpoint('/v1/spool/threads/');
const postSpoolEndpoint = ApiEndpoint('/v1/spool/create');

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

    static async post(name: string, ownerId: string, interests: string[], moderators: string[]): Promise<ISpool> {
        const response = await postWithAuth(postSpoolEndpoint, {
            name,
            ownerId,
            interests,
            moderators,
        });

        if (!response.ok) {
            throw new Error(`Failed to post spool: ${await response.text()}`);
        }

        return await response.json();
    }
}
