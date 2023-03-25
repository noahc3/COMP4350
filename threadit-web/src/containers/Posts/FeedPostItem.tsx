import { useRef, useState } from 'react';
import { Box, HStack, VStack, Text, Button, ButtonGroup, Heading, useColorMode, Image } from "@chakra-ui/react"
import { observer } from "mobx-react"
import { ArrowUpIcon, ArrowDownIcon } from "@chakra-ui/icons";
import { IThreadFull } from "../../models/ThreadFull";
import Moment from 'react-moment';
import { Link } from "react-router-dom";
import ThreadAPI from "../../api/ThreadAPI";
import "./FeedPostItem.scss";
import { userStore } from "../../stores/UserStore";
import { mode } from '@chakra-ui/theme-tools'
import { ThreadTypes } from '../../constants/ThreadTypes';
import { BiLinkExternal } from 'react-icons/bi';
import { FaCommentDots } from 'react-icons/fa';
import ChakraUIRenderer from 'chakra-ui-markdown-renderer';
import ReactMarkdown from 'react-markdown';
import React from 'react';


export const FeedPostItem = observer(({ thread }: { thread: IThreadFull | any }) => {
    const profile = userStore.userProfile;
    const { colorMode } = useColorMode()
    const [isStitched, setIsStitched] = useState(thread.stitches.includes(profile ? profile.id : ""));
    const [isRipped, setIsRipped] = useState(thread.rips.includes(profile ? profile.id : ""));
    const renderedMd = useRef<any>();
    const [renderedHeight, setRenderedHeight] = useState(0);
    const dateString = (
        <Moment fromNow>{thread.dateCreated}</Moment>
    )

    const stitchThread = async () => {
        if (thread) {
            const stitchedThread = await ThreadAPI.stitchThread(thread.id);
            if (stitchedThread) {
                updateStitchesAndRips(stitchedThread.stitches, stitchedThread.rips);
            }
        }
    }

    const ripThread = async () => {
        if (thread) {
            const rippedThread = await ThreadAPI.ripThread(thread.id);
            if (rippedThread) {
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

    React.useLayoutEffect(() => {
        setRenderedHeight(renderedMd.current?.clientHeight ?? 0);
    }, [renderedMd])

    const threadUrl = "/s/" + thread.spoolName + "/post/" + thread.id;
    let threadHeader;
    let threadContent;

    if (thread.threadType === ThreadTypes.TEXT) {
        threadHeader = (
            <Link to={threadUrl}>
                <Heading as='h3' size='md'>
                    {thread.title}
                </Heading>
            </Link>
        )
        threadContent = (
            <VStack alignItems={'start'} spacing={0}>
                <Box ref={renderedMd} maxHeight={'320px'} overflow={'hidden'} className={renderedHeight >= 320 ? 'fade-bottom' : ''}>
                    <ReactMarkdown components={ChakraUIRenderer()} disallowedElements={['h1', 'h2', 'h3', 'img']} children={thread.content} skipHtml/>
                </Box>
                {renderedHeight >= 320 && (
                    <Link to={threadUrl}>
                        <Button variant='link'>Read more</Button>
                    </Link>
                )}
            </VStack>
        )
    } else if (thread.threadType === ThreadTypes.IMAGE) {
        threadHeader = (
            <Link to={threadUrl}>
                <Heading as='h3' size='md'>
                    {thread.title}
                </Heading>
            </Link>
        )
        threadContent = (<Image maxHeight={'20rem'} alt={thread.content} loading='lazy' src={thread.content} />)
    } else if (thread.threadType === ThreadTypes.LINK) {
        const domain = new URL(thread.content).hostname
        threadHeader = (
            <a href={thread.content} target='_blank' rel="noreferrer">
                <VStack alignItems={'start'} spacing='1'>
                    <Heading as='h3' size='md'>
                        {thread.title}
                    </Heading>
                    <Button colorScheme={'blue'} variant={'link'} rightIcon={<BiLinkExternal/>}>{domain}</Button>
                </VStack>
            </a>
        )
        threadContent = (<></>)
    }

    return (
        <>
            <Box border="1px solid gray" borderRadius="3px" p="2rem" bgColor={mode("white", "gray.800")({ colorMode })} w="100%" className="feedPostItem">
                <VStack alignItems="start" spacing={'3'}>
                    <HStack>
                        <Link to={"/s/" + thread.spoolName}><Text fontWeight={"bold"}>s/{thread ? thread.spoolName : ""}</Text></Link>
                        <Text color={mode("blackAlpha.600", "gray.400")({ colorMode })}> • Posted by u/{thread ? thread.authorName : ""} • {dateString}</Text>
                    </HStack>
                    {threadHeader}
                    {threadContent}
                    <HStack>
                        <ButtonGroup size={'sm'} isAttached>
                            <Button leftIcon={<ArrowUpIcon />} onClick={() => { stitchThread() }} colorScheme={isStitched ? "blue" : "gray"}>{thread.stitches.length}</Button>
                            <Button leftIcon={<ArrowDownIcon />} onClick={() => { ripThread() }} colorScheme={isRipped ? "red" : "gray"}>{thread.rips.length}</Button>
                        </ButtonGroup>
                        <Link to={threadUrl}>
                            <Button size={'sm'} leftIcon={<FaCommentDots/>}>View Comments</Button>
                        </Link>
                    </HStack>
                </VStack>
            </Box>
        </>
    );
})