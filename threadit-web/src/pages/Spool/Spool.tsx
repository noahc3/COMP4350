import { Box, Button, Container, VStack } from "@chakra-ui/react";
import { observer } from "mobx-react-lite";
import React, { useState } from "react";
import { useParams } from "react-router";
import SpoolAPI from "../../api/SpoolAPI";
import { PageLayout } from "../../containers/PageLayout/PageLayout";
import { PostFeed } from "../../containers/Posts/PostFeed";
import { ISpool } from "../../models/Spool";
import { IThreadFull } from "../../models/ThreadFull";
import { IoCreateOutline } from "react-icons/io5";
import { NavLink } from "react-router-dom";
import { authStore } from "../../stores/AuthStore";

export const Spool = observer(() => {
    const { id } = useParams();
    const [spool, setSpool] = useState<ISpool>();
    const [threads, setThreads] = useState<IThreadFull[]>([]);
    const isAuthenticated = authStore.isAuthenticated;

    React.useEffect(() => {
        if (id) {
            SpoolAPI.getSpoolById(id).then((spool) => {
                setSpool(spool);
            });

            SpoolAPI.getSpoolThreads(id).then((threads) => {
                setThreads(threads);
            });
        }
    }, [id]);

    return (
        <PageLayout title={spool ? spool.name + ": New Posts" : ""}>
            {spool &&
                (
                    <Container centerContent={false} maxW={"container.md"}>
                        <VStack>
                            {isAuthenticated &&
                                <Box border="1px solid gray" borderRadius="3px" bgColor={"white"} w="100%" p="0.5rem">
                                    <NavLink to={"/s/" + spool.name + "/createthread"}><Button leftIcon={<IoCreateOutline />}>Create Post</Button></NavLink>
                                </Box>
                            }
                            <PostFeed threads={threads} />
                        </VStack>
                    </Container>
                )}
        </PageLayout>
    );
});