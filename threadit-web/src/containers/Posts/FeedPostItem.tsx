import { Box, HStack, VStack, Text, Button, Heading } from "@chakra-ui/react"
import { observer } from "mobx-react"
import { CgNotes } from "react-icons/cg"
import { ArrowUpIcon, ArrowDownIcon } from "@chakra-ui/icons"
import { IThreadFull } from "../../models/ThreadFull";
import { NavLink } from "../Router/NavLink";
import Moment from 'react-moment';
import { Link } from "react-router-dom";
import "./FeedPostItem.scss"

export const FeedPostItem = observer(({thread}: {thread: IThreadFull | any}) => {
    const dateString = (
        <Moment fromNow>{thread.dateCreated}</Moment>
    )

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
                        <NavLink to={"/s/" + thread.spoolName + "/post/" + thread.id}>
                            <Button leftIcon={<CgNotes />}>View Thread </Button>
                        </NavLink>
                    </HStack>
                    <HStack>
                        <Button leftIcon={<ArrowUpIcon />}>Upvote </Button>
                        <Text>{thread.stitches.length}</Text>
                        <Button leftIcon={<ArrowDownIcon />}>Downvote </Button>
                        <Text>{thread.rips.length}</Text>
                    </HStack>
                </VStack>
            </Box>
        </>
    );
})