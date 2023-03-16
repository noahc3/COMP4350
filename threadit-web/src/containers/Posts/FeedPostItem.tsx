import React, { useState } from 'react';
import { Box, HStack, VStack, Text, Button, ButtonGroup, Heading } from "@chakra-ui/react"
import { observer } from "mobx-react"
import { ArrowUpIcon, ArrowDownIcon } from "@chakra-ui/icons";
import { IThreadFull } from "../../models/ThreadFull";
import Moment from 'react-moment';
import { Link } from "react-router-dom";
import ThreadAPI from "../../api/ThreadAPI";
import "./FeedPostItem.scss";
import { userStore } from "../../stores/UserStore";


export const FeedPostItem = observer(({thread}: {thread: IThreadFull | any}) => {
    const [stateThread, setStateThread] = useState(thread);
    const profile = userStore.userProfile;
    const [isStitched, setIsStitched] = useState(false);
    const [isRipped, setIsRipped] = useState(false);
    const dateString = (
        <Moment fromNow>{thread.dateCreated}</Moment>
    )

    const stitchThread = async () => {
        if (thread) {
            const stitchedThread = await ThreadAPI.stitchThread(thread.id);
            if(stitchedThread != null){
                setStateThread(stitchedThread);
            }
          }
    }

    const ripThread = async () => {
        if (thread) {
            const rippedThread = await ThreadAPI.ripThread(thread.id);
            if(rippedThread != null){
                setStateThread(rippedThread);
            }
          }
    }

    React.useEffect(() => {
        setIsStitched(stateThread.stitches.includes(profile ? profile.id : ""));
        setIsRipped(stateThread.rips.includes(profile ? profile.id : ""));
    }, [profile, stateThread])

    return (
        <>
            <Box border="1px solid gray" borderRadius="3px" p="2rem" bgColor={"white"} w="100%" className="feedPostItem">
                <VStack alignItems="start">
                    <HStack>
                        <Link to={"/s/" + thread.spoolName}><Text fontWeight={"bold"}>s/{thread ? thread.spoolName : ""}</Text></Link>
                        <Text color={"blackAlpha.600"}> • Posted by u/{thread ? thread.authorName : ""} • {dateString}</Text>
                    </HStack>
                    <HStack>
                        <VStack alignItems="start">
                            <Link to={"/s/" + thread.spoolName + "/post/" + thread.id}>
                                <Heading as='h3' size='md'>
                                    {thread.title}
                                </Heading>
                            </Link>
                        </VStack>
                    </HStack>
                    <HStack>
                        <Text>
                            {thread.content}
                        </Text>
                    </HStack>
                    <HStack>
                        <ButtonGroup size={'sm'} isAttached>
                            <Button leftIcon={<ArrowUpIcon />} onClick={() => { stitchThread() }} colorScheme={isStitched ? "blue" : "gray"}>{stateThread.stitches.length}</Button>
                            <Button leftIcon={<ArrowDownIcon />} onClick={() => { ripThread() }} colorScheme={isRipped ? "red" : "gray"}>{stateThread.rips.length}</Button>
                        </ButtonGroup>
                    </HStack>
                </VStack>
            </Box>
        </>
    );
})