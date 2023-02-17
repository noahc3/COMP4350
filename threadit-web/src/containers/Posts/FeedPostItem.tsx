import { Box, HStack, VStack, Text, Button, Heading } from "@chakra-ui/react"
import { observer } from "mobx-react"
import { FaRegComment } from "react-icons/fa"
import { IThreadFull } from "../../models/ThreadFull";
import Moment from 'react-moment';

export const FeedPostItem = observer(({thread}: {thread: IThreadFull}) => {
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
                            <Heading as='h3' size='md'>
                                {thread.title}
                            </Heading>
                        </VStack>
                    </HStack>
                    <HStack>
                        <Button leftIcon={<FaRegComment />}>View Comments</Button>
                    </HStack>
                </VStack>
            </Box>
        </>
    );
})