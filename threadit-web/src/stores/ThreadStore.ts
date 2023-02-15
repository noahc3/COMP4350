import { makeObservable, action, observable } from "mobx";
import { IThread } from '../models/Thread';
import ThreadAPI from "../api/ThreadAPI";

export class ThreadStore {
    @action
    async postThread(title: string, content: string, topic: string, spoolId: string): Promise<IThread> {
        const thread = await ThreadAPI.postThread(title, content, topic, spoolId);
        return thread;
    }
}

export const threadStore = new ThreadStore();