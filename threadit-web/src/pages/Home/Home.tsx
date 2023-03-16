import { Box, Container, HStack, VStack } from "@chakra-ui/react";
import { observer } from "mobx-react-lite";
import React, { useState } from "react";
import ThreadAPI from "../../api/ThreadAPI";
import { PageLayout } from "../../containers/PageLayout/PageLayout";
import { PostFeed } from "../../containers/Posts/PostFeed";
import { IThreadFull } from "../../models/ThreadFull";
import { ThreadSorter } from "../../containers/ThreadSorter/ThreadSorter";
import { spoolStore } from "../../stores/SpoolStore";
import { navStore } from "../../stores/NavStore";
import {Select, SingleValue, ActionMeta } from "chakra-react-select"

export const Home = observer(() => {
    const [threads, setThreads] = useState<IThreadFull[]>([]);

    const allSpools = spoolStore.allSpools;

    const options = allSpools ? allSpools.map((spool) => ({
        value: spool.id,
        label: spool.name
      })) : [];

    const onSpoolSelected = (selectedOption: SingleValue<{ value: string; label: string; }>, actionMeta: ActionMeta<{ value: string; label: string; }>) => {
        if(selectedOption){
            navStore.navigateTo(`/s/${selectedOption.label}`)
        }
    }

    const setSortedThreads = (threads: IThreadFull[]) => {
        setThreads(threads);
      };

    React.useEffect(() => {
        ThreadAPI.getAllThreads("", "").then((threads) => {
            setThreads(threads);
        });
    }, []);

    return (
        <PageLayout title="Home">
            <Container centerContent={false} maxW={"container.md"}>
                <HStack>
                    <VStack w="100%">
                        <Box border="1px solid gray" borderRadius="3px" bgColor={"white"} w="100%" h="50%" p="0.5rem">
                            <Select
                                options={options}
                                onChange={onSpoolSelected}
                                placeholder="Search"
                            />
                        </Box>
                        <ThreadSorter onThreadsSorted={setSortedThreads}></ThreadSorter>
                        <PostFeed threads={threads}/>
                    </VStack>
                </HStack>
            </Container>
        </PageLayout>
    );
});