import { ApiEndpoint } from "../constants/ApiConstants";
import { SortTypes } from "../constants/SortTypes";
import { ThreadTypes } from "../constants/ThreadTypes";
import { IThread } from "../models/Thread";
import { IThreadFull } from "../models/ThreadFull";
import { deleteWithAuth, get, postWithAuth } from "./Request";

const threadEndpoint = ApiEndpoint('/v1/thread/');
const postThreadEndpoint = ApiEndpoint('/v1/thread/create');
const editThreadEndpoint = ApiEndpoint('/v1/thread/edit');
const stitchThreadEndpoint = ApiEndpoint('/v1/thread/stitch');
const ripThreadEndpoint = ApiEndpoint('/v1/thread/rip');
const allThreadsEndpoint = ApiEndpoint('/v1/thread/threads/');

export default class ThreadAPI {
    static async getThreadById(threadId: string): Promise<IThreadFull> {
        const response = await get(threadEndpoint + threadId);    

        if (!response.ok) {
            throw new Error(`Failed to get thread: ${await response.text()}`);
        }

        return await response.json();
    }

    static async postThread(title: string, content: string, topic: string, spoolId: string, threadType: ThreadTypes): Promise<IThread> {
        const response = await postWithAuth(postThreadEndpoint, {
            title,
            content,
            topic,
            spoolId,
            threadType
        });
    
        if (!response.ok) {
            throw new Error(`Failed to post thread: ${await response.text()}`);
        }

        return await response.json();
    }

    static async getThreads(sortType: SortTypes, searchWord?: string | undefined, skip?: number | undefined, spoolId?: string | undefined): Promise<IThreadFull[]> {
        const params = new URLSearchParams();
        if (searchWord) params.append('q', searchWord);
        if (skip) params.append('skip', skip.toString());
        if (spoolId) params.append('spoolId', spoolId);

        const response = await get(allThreadsEndpoint + sortType + '?' + params.toString());
    
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

    static async stitchThread(threadId: string): Promise<IThreadFull> {
        const response = await postWithAuth(stitchThreadEndpoint, threadId);

        if (!response.ok) {
            throw new Error(`Failed to stitch thread: ${await response.text()}`);
        }

        return await response.json();
    }

    static async ripThread(threadId: string): Promise<IThreadFull> {
        const response = await postWithAuth(ripThreadEndpoint, threadId);

        if (!response.ok) {
            throw new Error(`Failed to rip thread: ${await response.text()}`);
        }

        return await response.json();
    }
}