export interface IThreadFull {
    id: string
    topic: string
    title: string
    content: string
    spoolId: string
    ownerId: string
    authorName: string
    spoolName: string
    dateCreated: string
    stitches: string[]
    rips: string[]
    topLevelCommentCount: number
    commentCount: number
}