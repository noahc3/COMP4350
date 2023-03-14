import { Container } from "@chakra-ui/react";
import { observer } from "mobx-react-lite";
import React from "react";
import { useParams } from "react-router";
import ThreadAPI from "../../api/ThreadAPI";
import { CommentFeed } from "../../containers/Comments/CommentFeed";
import { PageLayout } from "../../containers/PageLayout/PageLayout";
import { IThreadFull } from "../../models/ThreadFull";
import { ThreadPost } from "./ThreadPost";


export const Thread = observer(() => {
    const { threadId } = useParams();
    const [thread, setThread] = React.useState<IThreadFull>();

    React.useEffect(() => {
        if (threadId) {
            ThreadAPI.getThreadById(threadId).then((thread) => {
                setThread(thread);
            });
        }
    }, [threadId]);

    return (
        <PageLayout title={"View thread"}>
            <Container centerContent={false} maxW={"container.lg"}>
                {thread && <>
                    <ThreadPost thread={thread} />
                    <br/>
                    <CommentFeed thread={thread} />
                </>}   
            </Container>
        </PageLayout>
    );
});