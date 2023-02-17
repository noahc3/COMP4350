import { Container } from "@chakra-ui/react";
import { observer } from "mobx-react-lite";
import { useParams } from "react-router";
import { PageLayout } from "../../containers/PageLayout/PageLayout";
import { ThreadPost } from "./ThreadPost";


export const Thread = observer(() => {
    const { threadId } = useParams();

    return (
        <PageLayout title={"View thread"}>
            <Container centerContent={false} maxW={"container.md"}>
                {threadId && <ThreadPost threadId={threadId} />}   
            </Container>
        </PageLayout>
    );
});