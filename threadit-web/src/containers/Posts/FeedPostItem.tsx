import { Box, HStack, VStack, Text, Button, Heading } from "@chakra-ui/react"
import { observer } from "mobx-react"
import { FaRegComment } from "react-icons/fa"
import { IThreadFull } from "../../models/ThreadFull";
import { NavLink } from "../Router/NavLink";
import Moment from 'react-moment';

export const FeedPostItem = observer(({thread}: {thread: IThreadFull | any}) => {
    const dateString = (
        <Moment fromNow>{thread.dateCreated}</Moment>
    )

    return (
        <>
            <Box border="1px solid gray" borderRadius="3px" p="2rem" bgColor={"white"} w="100%">
                <VStack alignItems="start">
                    <HStack><Text fontWeight={"bold"}>s/{thread.spoolName}</Text><Text color={"blackAlpha.600"}> • Posted by u/{thread.authorName} • {dateString}</Text></HStack>
                    <HStack>
                        <VStack alignItems="start">
                            <NavLink to={"/s/" + thread.spoolName + "/post/" + thread.id}>
                                <Heading as='h3' size='md'>
                                    {thread.title}
                                </Heading>
                            </NavLink>
                        </VStack>
                    </HStack>
                    <HStack>
                        <NavLink to={"/s/" + thread.spoolName + "/post/" + thread.id}>
                            <Button leftIcon={<FaRegComment />}>View Comments </Button>
                        </NavLink>
                    </HStack>
                </VStack>
            </Box>
        </>
    );
})