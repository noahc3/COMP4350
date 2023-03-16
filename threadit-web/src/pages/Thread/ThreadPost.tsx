import { Box, Button, ButtonGroup, Heading, HStack, Spinner, Text, Textarea, VStack } from "@chakra-ui/react";
import { observer } from "mobx-react-lite";
import React from "react";
import ThreadAPI from "../../api/ThreadAPI";
import { IThreadFull } from "../../models/ThreadFull";
import { ArrowUpIcon, ArrowDownIcon } from "@chakra-ui/icons";
import Moment from 'react-moment';
import { authStore } from "../../stores/AuthStore";
import { userStore } from "../../stores/UserStore";
import { IoCreateOutline } from "react-icons/io5";
import { MdOutlineCancel, MdOutlineDelete } from "react-icons/md";
import { BiSave } from "react-icons/bi";
import { navStore } from "../../stores/NavStore";
import { Link } from "react-router-dom";
import "./ThreadPost.scss";
import { ISpool } from "../../models/Spool";

export const ThreadPost = observer(({ spool, thread }: { spool: ISpool, thread: IThreadFull }) => {
    const [isEditing, setIsEditing] = React.useState(false);
    const [isConfirmingDelete, setIsConfirmingDelete] = React.useState(false);
    const [isDeleting, setIsDeleting] = React.useState(false);
    const [isSaving, setIsSaving] = React.useState(false);
    const [editedText, setEditedText] = React.useState("");
    const threadId = thread.id;
    const isAuthenticated = authStore.isAuthenticated;
    const isThreadOwner = (isAuthenticated && thread) ? thread.ownerId === userStore.userProfile?.id : false;
    const isSpoolOwner = (isAuthenticated && thread) ? spool?.ownerId === userStore.userProfile?.id : false;
    const isModerator = (isAuthenticated && thread) ? spool?.moderators.includes(userStore.userProfile!.id) : false;
    const disableInputs = isSaving || isDeleting;
    const profile = userStore.userProfile;
    const [isStitched, setIsStitched] = useState(false);
    const [isRipped, setIsRipped] = useState(false);

    const dateString = (
        <Moment fromNow>{thread ? thread.dateCreated : ""}</Moment>
    )

    const startEdit = () => {
        setIsEditing(true);
        setEditedText(thread ? thread.content : "");
    }

    const saveEdit = async () => {
        if (thread) {
            setIsSaving(true);
            thread.content = editedText;
            try {
                await ThreadAPI.editThread(thread);
            } finally {
                ThreadAPI.getThreadById(threadId).then((thread) => {
                    setIsSaving(false);
                    setIsEditing(false);
                });
            }
        }
    }

    const deleteThread = async () => {
        if (thread) {
            setIsDeleting(true);
            try {
                await ThreadAPI.deleteThread(thread.id);
            } finally {
                navStore.navigateTo("/s/" + thread.spoolName)
            }
        }
    }

    const stitchThread = async () => {
        if (thread) {
            const stitchedThread = await ThreadAPI.stitchThread(thread.id);
            if(stitchedThread != null){
                setThread(stitchedThread);
            }
          }
    }

    const ripThread = async () => {
        if (thread) {
            const rippedThread = await ThreadAPI.ripThread(thread.id);
            if(rippedThread != null){
                setThread(rippedThread);
            }
          }
    }

    React.useEffect(() => {
        if(thread){
            setIsStitched(thread.stitches.includes(profile ? profile.id : ""));
            setIsRipped(thread.rips.includes(profile ? profile.id : ""));
        }
    }, [thread, profile])

    return (
        <Box border="1px solid gray" borderRadius="3px" p="2rem" bgColor={"white"} w="100%" className="threadPost">
            {thread ? (
                <VStack alignItems="start">
                    <HStack>
                        <Link to={"/s/" + thread.spoolName}><Text fontWeight={"bold"}>s/{thread ? thread.spoolName : ""}</Text></Link>
                        <Text color={"blackAlpha.600"}> • Posted by u/{thread ? thread.authorName : ""} • {dateString}</Text>
                    </HStack>
                    <Heading as='h3' size='md'>
                        {thread ? thread.title : ""}
                    </Heading>
                    {(thread && thread.content) &&
                        <>
                            {!isEditing ?
                                (
                                    <Text>
                                        {thread.content}
                                    </Text>
                                ) : (
                                    <>
                                        <Textarea disabled={disableInputs} value={editedText} onChange={(e) => { setEditedText(e.target.value) }} />
                                    </>
                                )}
                        </>
                    }

                    <HStack>
                        <ButtonGroup size={'sm'} isAttached>
                            <Button leftIcon={<ArrowUpIcon />} onClick={() => { stitchThread() }} colorScheme={isStitched ? "blue" : "gray"}>{thread ? thread.stitches.length : 0}</Button>
                            <Button leftIcon={<ArrowDownIcon />} onClick={() => { ripThread() }} colorScheme={isRipped ? "red" : "gray"}>{thread ? thread.rips.length: 0}</Button>
                        </ButtonGroup>
                    </HStack>

                    {(isThreadOwner || isSpoolOwner || isModerator) &&
                        <HStack>
                            {!isConfirmingDelete ?
                                (
                                    <ButtonGroup size={'sm'} isAttached>
                                        {!isEditing ? (
                                            <>
                                                <Button leftIcon={<IoCreateOutline />} onClick={() => { startEdit() }}>Edit</Button>
                                                <Button leftIcon={<MdOutlineDelete />} onClick={() => { setIsConfirmingDelete(true) }}>Delete</Button>
                                            </>
                                        ) : (
                                            <>
                                                <Button isLoading={isSaving} loadingText='Saving...' disabled={disableInputs} leftIcon={<BiSave />} onClick={() => { saveEdit() }}>Save</Button>
                                                <Button disabled={disableInputs} leftIcon={<MdOutlineCancel />} onClick={() => { setIsEditing(false) }}>Cancel</Button>
                                            </>
                                        )}
                                    </ButtonGroup>
                                ) : (
                                    <>
                                        <Text color="red">Are you sure you want to delete?</Text>
                                        <ButtonGroup size={'sm'} isAttached>
                                            <Button disabled={disableInputs} isLoading={isDeleting} loadingText="Deleting..." leftIcon={<MdOutlineDelete />} colorScheme="red" onClick={() => { deleteThread() }}>Delete</Button>
                                            <Button disabled={disableInputs} leftIcon={<MdOutlineCancel />} onClick={() => { setIsConfirmingDelete(false) }}>Cancel</Button>
                                        </ButtonGroup>
                                    </>
                                )}
                        </HStack>
                    }
                </VStack>
            ) : (
                <Box textAlign='center' width='100%'>
                    <Spinner size={'xl'} thickness='4px' emptyColor='gray.200' />
                </Box>
            )}
        </Box>
    )
})