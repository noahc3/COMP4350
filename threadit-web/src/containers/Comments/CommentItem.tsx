import { HStack, VStack } from "@chakra-ui/layout";
import { Avatar, Box, Button, ButtonGroup, Flex, Spacer, Text, Textarea } from "@chakra-ui/react";
import { observer } from "mobx-react";
import { useState } from "react";
import { IoCreateOutline } from "react-icons/io5";
import { MdOutlineCancel, MdOutlineDelete, MdReply } from "react-icons/md";
import Moment from "react-moment";
import CommentAPI from "../../api/CommentAPI";
import { authStore } from "../../stores/AuthStore";
import { userStore } from "../../stores/UserStore";
import { CommentBox } from "./CommentBox";
import { CommentTree, CommentTreeNode } from "./CommentTree";

export const CommentItem = observer(
    ({ commentId, commentTree }: { commentId: string, commentTree: CommentTree }) => {
        const [isLoadingReplies, setIsLoadingReplies] = useState<boolean>(false);
        const [isReplying, setIsReplying] = useState<boolean>(false);
        const [isSubmittingReply, setIsSubmittingReply] = useState<boolean>(false);
        const [isEditing, setIsEditing] = useState<boolean>(false);
        const [isSubmittingEdit, setIsSubmittingEdit] = useState<boolean>(false);
        const [isConfirmingDelete, setIsConfirmingDelete] = useState<boolean>(false);
        const [isDeleting, setIsDeleting] = useState<boolean>(false);

        const commentNode = commentTree.index.get(commentId);
        const comment = commentNode?.comment ?? null;

        const [editText, setEditText] = useState<string>(comment?.content ?? '');

        const loadedReplyCount = commentNode?.children.size ?? 0;
        const unloadedReplyCount = (comment?.childCommentCount ?? 0) - loadedReplyCount;

        const isAuthenticated = authStore.isAuthenticated;
        const canEdit = isAuthenticated && comment?.ownerId === userStore.userProfile?.id;
        const canDelete = isAuthenticated && comment?.ownerId === userStore.userProfile?.id;
        const disableInputs = isLoadingReplies || isSubmittingReply || isSubmittingEdit || isDeleting;

        const mapComment = (comment: CommentTreeNode) => {
            return (
                <>
                    {comment.comment && <CommentItem key={comment.id} commentId={comment.comment.id} commentTree={commentTree} />}
                </>
            );
        }

        let childComments: CommentTreeNode[] = [];
        commentNode?.children.forEach((comment) => {
            childComments.push(comment);
        });

        childComments = childComments.sort((a, b) => {
            return (new Date(b.comment!.dateCreated).valueOf() ?? 0) - (new Date(a.comment!.dateCreated).valueOf() ?? 0);
        })

        const replies = childComments.map((comment) => {
            return mapComment(comment);
        });

        const dateString = (
            <Moment fromNow>{comment?.dateCreated ?? Date.now()}</Moment>
        )

        const loadReplies = async () => {
            if (comment) {
                setIsLoadingReplies(true);
                const replies = await CommentAPI.expandComments(comment.threadId, commentId);
                commentTree.addComments(replies);
                commentTree.index.forEach((node) => {
                    console.log(node.id, node.comment?.content);
                })
                setIsLoadingReplies(false);
            }
        }

        const olderReplies = async () => {
            if (comment) {
                setIsLoadingReplies(true);

                const oldestReply = childComments.reduce((oldest, current) => {
                    if (current.comment?.dateCreated! < oldest.comment?.dateCreated!) {
                        return current;
                    }
                    return oldest;
                })

                const replies = await CommentAPI.olderComments(comment.threadId, oldestReply.id);
                commentTree.addComments(replies);
                commentTree.index.forEach((node) => {
                    console.log(node.id, node.comment?.content);
                })
                setIsLoadingReplies(false);
            }
        }

        const submitReply = async (content: string) => {
            if (comment) {
                setIsSubmittingReply(true);
                const reply = await CommentAPI.postComment(comment.threadId, comment.id, content);
                commentTree.addComment(reply);
                setIsSubmittingReply(false);
            }
        }

        const submitEdit = async () => {
            if (comment) {
                setIsSubmittingEdit(true);
                comment.content = editText;
                const updatedComment = await CommentAPI.editComment(comment.threadId, comment);
                commentTree.addComment(updatedComment);
                setIsSubmittingEdit(false);
                setIsEditing(false);
            }
        }

        const submitDelete = async () => {
            if (comment) {
                setIsDeleting(true);
                const updatedReply = await CommentAPI.deleteComment(comment.threadId, comment.id);
                commentTree.addComment(updatedReply);
                setIsDeleting(false);
                setIsConfirmingDelete(false);
            }
        }

        const cancelReply = () => {
            setIsReplying(false);
        }

        return (
            <>
                <Flex flexDir={"row"} w="100%">
                    <Flex flexDir={"column"}>
                        <Avatar size={"sm"} src="/img/avatar_placeholder.png" />
                        <Box
                            marginLeft={"13px"}
                            marginRight={"13px"}
                            marginTop={"8px"}
                            h="100%"
                            bgColor={"gray.200"}
                        ></Box>
                    </Flex>
                    <Flex alignItems={'start'} paddingStart={"15px"} flexDir={"column"} w="100%">
                        <HStack>
                            <Text>{comment?.ownerName}</Text>
                            <Text color={"blackAlpha.600"}> • {dateString}</Text>
                        </HStack>
                        {!isEditing ? (
                            <>
                                <Text marginTop={"14px"} whiteSpace='pre-wrap'>
                                    {comment?.content}
                                </Text>

                                <HStack alignItems={'center'} marginTop={"14px"}>
                                    {!isConfirmingDelete ? (<ButtonGroup size={'sm'} isAttached>
                                        {isAuthenticated && 
                                            <Button onClick={() => { setIsReplying(true) }} isDisabled={disableInputs} variant='outline' leftIcon={<MdReply />}>Reply</Button>
                                        }
                                        {canEdit &&
                                            <Button onClick={() => { setIsEditing(true) }} isDisabled={disableInputs} variant='outline' leftIcon={<IoCreateOutline />}>Edit</Button>
                                        }
                                        {canDelete &&
                                            <Button onClick={() => { setIsConfirmingDelete(true) }} isDisabled={disableInputs} variant='outline' leftIcon={<MdOutlineDelete />}>Delete</Button>
                                        }
                                    </ButtonGroup>) : (
                                        <>
                                            <HStack >
                                                <Text color="red">Are you sure you want to delete?</Text>
                                                <ButtonGroup size={'sm'} isAttached>
                                                    <Button disabled={disableInputs} isLoading={isDeleting} loadingText="Deleting..." leftIcon={<MdOutlineDelete />} colorScheme="red" onClick={() => { submitDelete() }}>Delete</Button>
                                                    <Button disabled={disableInputs} leftIcon={<MdOutlineCancel />} onClick={() => { setIsConfirmingDelete(false) }}>Cancel</Button>
                                                </ButtonGroup>
                                            </HStack>
                                        </>
                                    )}
                                </HStack>
                            </>
                        ) : (
                            <>
                                <Textarea w='100%' value={editText} onChange={((e) => { setEditText(e.target.value) })} />
                                <Flex direction={'row'} w='100%' marginTop={"14px"}>
                                    <Spacer />
                                    <ButtonGroup size={'sm'}>
                                        <Button onClick={() => { setIsEditing(false) }} isDisabled={disableInputs}>Cancel</Button>
                                        <Button onClick={() => { submitEdit() }} isDisabled={disableInputs} colorScheme='purple'>Edit</Button>
                                    </ButtonGroup>
                                </Flex>
                            </>
                        )}
                        {isReplying &&
                            <Box w='100%' marginTop='14px'>
                                <CommentBox cancelCallback={() => { cancelReply() }} submitCallback={async (content: string) => { await submitReply(content) }} />
                            </Box>
                        }
                        {(replies.length === 0 && unloadedReplyCount > 0) &&
                            <Button isDisabled={disableInputs} marginTop='5px' variant='link' onClick={() => { loadReplies() }}>Show replies ({unloadedReplyCount})</Button>
                        }
                        {replies.length > 0 &&
                            <VStack marginTop='14px' spacing={'14px'} w="100%">
                                {replies}
                            </VStack>
                        }
                        {(replies.length > 0 && unloadedReplyCount > 0) &&
                            <Button isDisabled={disableInputs} marginTop='5px' variant='link' onClick={() => { olderReplies() }}>Show more replies ({unloadedReplyCount})</Button>
                        }
                    </Flex>
                </Flex>
            </>
        );
    }
);
