import { VStack } from "@chakra-ui/layout";
import { observer } from "mobx-react";
import { IThreadFull } from "../../models/ThreadFull";
import { FeedPostItem } from "./FeedPostItem";

export const PostFeed = observer(({threads}: {threads: IThreadFull[]}) => {
    const threadItems = threads.map((thread) => {
        return <FeedPostItem thread={thread} key={thread.id}/>
    });
    return (
        <>
            <VStack w="100%">
                {threadItems}
            </VStack>
        </>
    );
});