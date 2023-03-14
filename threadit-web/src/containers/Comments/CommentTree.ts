import { IComment } from '../../models/Comment';

function isNullOrWhitespace(str: string | null){
    return str === null || str.match(/^\s*$/) !== null;
}

export class CommentTree {
    public index: Map<string, CommentTreeNode>;
    public root: CommentTreeNode;

    constructor() {
        this.index = new Map<string, CommentTreeNode>();
        this.root = new CommentTreeNode('root', null, null);
    }

    public addComment(comment: IComment) {
        const node = isNullOrWhitespace(comment.parentCommentId) ? this.root.setChild(comment) : this.index.get(comment.parentCommentId!)?.setChild(comment);
        if (node) {
            this.index.set(comment.id, node);
        }
    }
    
    public addComments(comments: IComment[]) {
        comments.forEach(c => this.addComment(c));
    }
}

export class CommentTreeNode {
    public children = new Map<string, CommentTreeNode>();

    constructor(public id: string, public parentId: string | null, public comment: IComment | null) {
        
    }

    public setChild(child: IComment) {
        const node = new CommentTreeNode(child.id, child.parentCommentId, child);
        if (this.children.has(child.id)) {
            node.children = this.children.get(child.id)!.children;
        }
        this.children.set(child.id, node);
        return node;
    }
}