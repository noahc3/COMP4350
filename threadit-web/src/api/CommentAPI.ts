import { ApiEndpoint } from "../constants/ApiConstants";
import { IComment } from "../models/Comment";
import { deleteWithAuth, get, patchWithAuth, postWithAuth } from "./Request";

export default class CommentAPI {
  static async getBaseComments(threadId: string): Promise<IComment[]> {
    const endpoint = ApiEndpoint(`/v1/comments/${threadId}/`);
    const response = await get(endpoint);

    if (!response.ok) {
      throw new Error(
        `Failed to get base comments for thread ${threadId}: ${await response.text()}`
      );
    }

    return await response.json();
  }

  static async expandComments(
    threadId: string,
    parentCommentId: string
  ): Promise<IComment[]> {
    const endpoint = ApiEndpoint(
      `/v1/comments/${threadId}/expand/${parentCommentId}`
    );
    const response = await get(endpoint);

    if (!response.ok) {
      throw new Error(
        `Failed to get expand comments for thread ${threadId}: ${await response.text()}`
      );
    }

    return await response.json();
  }

  static async olderComments(
    threadId: string,
    siblingCommentId: string
  ): Promise<IComment[]> {
    const endpoint = ApiEndpoint(
      `/v1/comments/${threadId}/older/${siblingCommentId}`
    );
    const response = await get(endpoint);

    if (!response.ok) {
      throw new Error(
        `Failed to get older comments for thread ${threadId}: ${await response.text()}`
      );
    }

    return await response.json();
  }

  static async newerComments(
    threadId: string,
    siblingCommentId: string
  ): Promise<IComment[]> {
    const endpoint = ApiEndpoint(
      `/v1/comments/${threadId}/newer/${siblingCommentId}`
    );
    const response = await get(endpoint);

    if (!response.ok) {
      throw new Error(
        `Failed to get newer comments for thread ${threadId}: ${await response.text()}`
      );
    }

    return await response.json();
  }

  static async postComment(
    threadId: string,
    parentCommentId: string | null,
    commentContent: string
  ): Promise<IComment> {
    const endpoint = ApiEndpoint(
      `/v1/comments/${threadId}/${parentCommentId ?? ""}`
    );
    const response = await postWithAuth(endpoint, commentContent);

    if (!response.ok) {
      throw new Error(
        `Failed to post comment for thread ${threadId} on parent ${parentCommentId}: ${await response.text()}`
      );
    }

    return await response.json();
  }

  static async editComment(
    threadId: string,
    comment: IComment
  ): Promise<IComment> {
    const endpoint = ApiEndpoint(`/v1/comments/${threadId}/edit`);
    const response = await patchWithAuth(endpoint, comment);

    if (!response.ok) {
      throw new Error(
        `Failed to edit comment ${
          comment.id
        } for thread ${threadId}: ${await response.text()}`
      );
    }

    return await response.json();
  }

  static async deleteComment(
    threadId: string,
    commentId: string
  ): Promise<IComment> {
    const endpoint = ApiEndpoint(`/v1/comments/${threadId}/${commentId}`);
    const response = await deleteWithAuth(endpoint);

    if (!response.ok) {
      throw new Error(
        `Failed to delete comment ${commentId} for thread ${threadId}: ${await response.text()}`
      );
    }

    return await response.json();
  }
}
