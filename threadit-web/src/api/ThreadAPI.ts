import { ApiEndpoint } from "../constants/ApiConstants";
import { IThread } from "../models/Thread";
import { IThreadFull } from "../models/ThreadFull";
import { get, postWithAuth } from "./Request";

const postThreadEndpoint = ApiEndpoint('/v1/thread/create');
const allThreadsEndpoint = ApiEndpoint('/v1/thread/all');

export default class ThreadAPI {
    static async postThread(title: string, content: string, topic: string, spoolId: string): Promise<IThread> {
        const response = await postWithAuth(postThreadEndpoint, {
            title,
            content,
            topic,
            spoolId,
        });
    
        if (!response.ok) {
            throw new Error(`Failed to post thread: ${await response.text()}`);
        }

        return await response.json();
    }

    static async getAllThreads(): Promise<IThreadFull[]> {
        const response = await get(allThreadsEndpoint);
    
        if (!response.ok) {
            throw new Error(`Failed to get all threads: ${await response.text()}`);
        }

        return await response.json();
    }
}
