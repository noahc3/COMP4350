import { Box, Button, ButtonGroup, Container, Heading, HStack, Spacer, Spinner, Text, VStack } from "@chakra-ui/react";
import { observer } from "mobx-react-lite";
import React, { useState } from "react";
import { useParams } from "react-router";
import ThreadAPI from "../../api/ThreadAPI";
import { PageLayout } from "../../containers/PageLayout/PageLayout";
import { IThreadFull } from "../../models/ThreadFull";
import Moment from 'react-moment';
import { authStore } from "../../stores/AuthStore";
import { userStore } from "../../stores/UserStore";
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