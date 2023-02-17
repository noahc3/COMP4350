import { ApiEndpoint } from "../constants/ApiConstants";
import { IThread } from "../models/Thread";
import { IThreadFull } from "../models/ThreadFull";
import { deleteWithAuth, get, postWithAuth } from "./Request";

const threadEndpoint = ApiEndpoint('/v1/thread/');
const postThreadEndpoint = ApiEndpoint('/v1/thread/create');
const editThreadEndpoint = ApiEndpoint('/v1/thread/edit');
const allThreadsEndpoint = ApiEndpoint('/v1/thread/all');

export default class ThreadAPI {
    static async getThreadById(threadId: string): Promise<IThreadFull> {
        const response = await get(threadEndpoint + threadId);
    
        if (!response.ok) {
            throw new Error(`Failed to get thread: ${await response.text()}`);
        }

        return await response.json();
    }

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

    static async editThread(thread: IThreadFull): Promise<void> {
        const response = await postWithAuth(editThreadEndpoint, thread);

        if (!response.ok) {
            throw new Error(`Failed to edit thread: ${await response.text()}`);
        }
    }

    static async deleteThread(threadId: string): Promise<void> {
        const response = await deleteWithAuth(threadEndpoint + threadId);

        if (!response.ok) {
            throw new Error(`Failed to delete thread: ${await response.text()}`);
        }
    }
}