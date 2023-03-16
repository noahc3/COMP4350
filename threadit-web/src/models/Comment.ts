export interface IComment {
    id: string
    content: string
    edited: boolean
    ownerId: string
    threadId: string
    parentCommentId: string | null
    dateCreated: string
    isDeleted: boolean
    ownerName: string
    childCommentCount: number
}