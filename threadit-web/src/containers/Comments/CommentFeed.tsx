import { VStack } from "@chakra-ui/layout";
import { Button } from "@chakra-ui/react";
import { observer } from "mobx-react";
import { useEffect, useState } from "react";
import CommentAPI from "../../api/CommentAPI";
import { IThreadFull } from "../../models/ThreadFull";
import { CommentBox } from "./CommentBox";
import { CommentItem } from "./CommentItem";
import { CommentTree, CommentTreeNode } from "./CommentTree";

export const CommentFeed = observer(({thread}: {thread: IThreadFull}) => {
    const [commentTree, setCommentTree] = useState<CommentTree>(new CommentTree());
    const [isLoadingReplies, setIsLoadingReplies] = useState<boolean>(false);
    // eslint-disable-next-line @typescript-eslint/no-unused-vars
    const [_, setIsSubmittingComment] = useState<boolean>(false);

    const loadedReplyCount = commentTree.root.children.size ?? 0;
    const unloadedReplyCount = thread.topLevelCommentCount - loadedReplyCount;

    const disableInputs = isLoadingReplies;

    useEffect(() => {
        CommentAPI.getBaseComments(thread.id).then((comments) => {
            const commentTree = new CommentTree();
            commentTree.addComments(comments);
            setCommentTree(commentTree);
        });
    }, [thread]);


    const mapComment = (comment: CommentTreeNode) => {
        return (
            <>
                {comment.comment && <CommentItem key={comment.id} commentId={comment.comment.id} commentTree={commentTree} />}
            </>
        );
    }

    let topLevelComments: CommentTreeNode[] = [];
    commentTree.root.children.forEach((comment) => {
        topLevelComments.push(comment);
    });

    topLevelComments = topLevelComments.sort((a, b) => {
        return (new Date(b.comment!.dateCreated).valueOf() ?? 0) - (new Date(a.comment!.dateCreated).valueOf() ?? 0);
    })

    const comments = topLevelComments.map((comment) => {
        return mapComment(comment);
    });

    const olderReplies = async () => {
        if (thread) {
            setIsLoadingReplies(true);

            const oldestReply = topLevelComments.reduce((oldest, current) => {
                if (current.comment?.dateCreated! < oldest.comment?.dateCreated!) {
                    return current;
                }
                return oldest;
            })

            const replies = await CommentAPI.olderComments(thread.id, oldestReply.id);
            commentTree.addComments(replies);
            commentTree.index.forEach((node) => {
                console.log(node.id, node.comment?.content);
            })
            setIsLoadingReplies(false);
        }
    }

    const submitComment = async (content: string) => {
        setIsSubmittingComment(true);
        const comment = await CommentAPI.postComment(thread.id, null, content);
        commentTree.addComment(comment);
        thread.topLevelCommentCount++;
        thread.commentCount++;
        setIsSubmittingComment(false);
    }

    return (
        <>
            <VStack marginBottom={'20rem'} alignItems={'start'} spacing={'30px'} p="2rem" w="100%" border="1px solid gray" borderRadius="3px" bgColor={"white"}>
                <CommentBox submitCallback={async (content: string) => {await submitComment(content)}}/>
                {comments}
                {(unloadedReplyCount > 0) && 
                    <Button isDisabled={disableInputs} marginTop='5px' variant='link' onClick={() => {olderReplies()}}>Show more replies ({unloadedReplyCount})</Button>
                }
            </VStack>
        </>
    );
});