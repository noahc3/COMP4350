import { Box, Container, Heading, HStack, Spacer, Text, VStack} from "@chakra-ui/react";
import { observer } from "mobx-react-lite";
import React, { useState } from "react";
import { useParams } from "react-router";
import ThreadAPI from "../../api/ThreadAPI";
import { PageLayout } from "../../containers/PageLayout/PageLayout";
import { IThreadFull } from "../../models/ThreadFull";
import Moment from 'react-moment';


export const Thread = observer(() => {
    const { threadId } = useParams();
    const [thread, setThread] = useState<IThreadFull>();

    const dateString = (
        <Moment fromNow>{thread ? thread.dateCreated : ""}</Moment>
    )

    React.useEffect(() => {
        if (threadId) {
            ThreadAPI.getThreadById(threadId).then((thread) => {
                setThread(thread);
            });
        }
    }, [threadId]);

    return (
        <PageLayout title={"View thread"}>
            <Container centerContent={false} maxW={"container.md"}>
                <Box border="1px solid gray" borderRadius="3px" p="2rem" bgColor={"white"} w="100%">
                    <VStack alignItems="start">
                        <HStack><Text fontWeight={"bold"}>s/{thread ? thread.spoolName : ""}</Text><Text color={"blackAlpha.600"}> • Posted by u/{thread ? thread.authorName : ""} • {dateString}</Text></HStack>
                        <HStack>
                            <VStack alignItems="start">
                                <Heading as='h3' size='md'>
                                    {thread ? thread.title : ""}
                                </Heading>
                                <Spacer />
                                <Text>
                                    {thread ? thread.content : ""}
                                </Text>
                            </VStack>
                        </HStack>
                    </VStack>
                </Box>
            </Container>
        </PageLayout>
    );
});