import { Container } from "@chakra-ui/react";
import { observer } from "mobx-react-lite";
import React from "react";
import { useParams } from "react-router";
import SpoolAPI from "../../api/SpoolAPI";
import ThreadAPI from "../../api/ThreadAPI";
import { CommentFeed } from "../../containers/Comments/CommentFeed";
import { PageLayout } from "../../containers/PageLayout/PageLayout";
import { ISpool } from "../../models/Spool";
import { IThreadFull } from "../../models/ThreadFull";
import { ThreadPost } from "./ThreadPost";

export const Thread = observer(() => {
  const { threadId } = useParams();
  const [thread, setThread] = React.useState<IThreadFull>();
  const [spool, setSpool] = React.useState<ISpool>();

  React.useEffect(() => {
    if (threadId) {
      ThreadAPI.getThreadById(threadId).then((thread) => {
        setThread(thread);
      });
    }
  }, [threadId]);

  React.useEffect(() => {
    if (thread && thread.spoolName) {
      SpoolAPI.getSpoolByName(thread.spoolName).then((spool) => {
        setSpool(spool);
      });
    }
  }, [thread, thread?.spoolName]);

  return (
    <PageLayout title={"View thread"}>
      <Container centerContent={false} maxW={"container.lg"}>
        {thread && spool && (
          <>
            <ThreadPost spool={spool} thread={thread} />
            <br />
            <CommentFeed spool={spool} thread={thread} />
          </>
        )}
      </Container>
    </PageLayout>
  );
});
