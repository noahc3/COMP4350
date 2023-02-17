import { Container, HStack, VStack } from "@chakra-ui/react";
import { observer } from "mobx-react-lite";
import React, { useState } from "react";
import ThreadAPI from "../../api/ThreadAPI";
import { PageLayout } from "../../containers/PageLayout/PageLayout";
import { PostFeed } from "../../containers/Posts/PostFeed";
import { IThreadFull } from "../../models/ThreadFull";

export const Home = observer(() => {

    const [threads, setThreads] = useState<IThreadFull[]>([]);

    React.useEffect(() => {
        ThreadAPI.getAllThreads().then((threads) => {
            setThreads(threads);
        });
    }, []);

    return (
        <PageLayout title="New Posts">
            <Container centerContent={false} maxW={"container.md"}>
                <HStack>
                    <VStack w="100%">
                        <PostFeed threads={threads}/>
                    </VStack>
                </HStack>
            </Container>
        </PageLayout>
    );
});