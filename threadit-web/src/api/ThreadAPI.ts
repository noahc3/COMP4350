import { ApiEndpoint } from "../constants/ApiConstants";
import { IThread } from "../models/Thread";
import { postWithAuth } from "./Request";

const threadEndpoint = ApiEndpoint('/v1/thread');

export default class UserAPI {
    static async postThread(title: string, content: string, topic: string, spoolId: string): Promise<IThread> {
        const response = await postWithAuth(threadEndpoint, {
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
}
