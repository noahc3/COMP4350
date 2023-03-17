import React, { useState } from 'react';
import { Box, HStack, VStack, Text, Button, ButtonGroup, Heading, useColorMode } from "@chakra-ui/react"
import { observer } from "mobx-react"
import { ArrowUpIcon, ArrowDownIcon } from "@chakra-ui/icons";
import { IThreadFull } from "../../models/ThreadFull";
import Moment from 'react-moment';
import { Link } from "react-router-dom";
import ThreadAPI from "../../api/ThreadAPI";
import "./FeedPostItem.scss";
import { userStore } from "../../stores/UserStore";
import { mode } from '@chakra-ui/theme-tools'


export const FeedPostItem = observer(({thread}: {thread: IThreadFull | any}) => {
    const profile = userStore.userProfile;
    const { colorMode } = useColorMode()
    const [isStitched, setIsStitched] = useState(thread.stitches.includes(profile ? profile.id : ""));
    const [isRipped, setIsRipped] = useState(thread.rips.includes(profile ? profile.id : ""));
    const dateString = (
        <Moment fromNow>{thread.dateCreated}</Moment>
    )

    const stitchThread = async () => {
        if (thread) {
            const stitchedThread = await ThreadAPI.stitchThread(thread.id);
            if(stitchedThread){
                updateStitchesAndRips(stitchedThread.stitches, stitchedThread.rips);                   
            }
          }
    }

    const ripThread = async () => {
        if (thread) {
            const rippedThread = await ThreadAPI.ripThread(thread.id);
            if(rippedThread){
                updateStitchesAndRips(rippedThread.stitches, rippedThread.rips);         
            }
          }
    }

    const updateStitchesAndRips = (newStitches: string[], newRips: string[]) => {
        if (thread) {
          thread.stitches = newStitches;
          thread.rips = newRips;
          setIsStitched(thread.stitches.includes(profile ? profile.id : ""));
          setIsRipped(thread.rips.includes(profile ? profile.id : ""));
        }
    }

    return (
        <>
            <Box border="1px solid gray" borderRadius="3px" p="2rem" bgColor={mode("white", "gray.800")({colorMode})} w="100%" className="feedPostItem">
                <VStack alignItems="start">
                    <HStack>
                        <Link to={"/s/" + thread.spoolName}><Text fontWeight={"bold"}>s/{thread ? thread.spoolName : ""}</Text></Link>
                        <Text color={mode("blackAlpha.600", "gray.400")({colorMode})}> • Posted by u/{thread ? thread.authorName : ""} • {dateString}</Text>
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
                            <Button leftIcon={<ArrowUpIcon />} onClick={() => { stitchThread() }} colorScheme={isStitched ? "blue" : "gray"}>{thread.stitches.length}</Button>
                            <Button leftIcon={<ArrowDownIcon />} onClick={() => { ripThread() }} colorScheme={isRipped ? "red" : "gray"}>{thread.rips.length}</Button>
                        </ButtonGroup>
                    </HStack>
                </VStack>
            </Box>
        </>
    );
})