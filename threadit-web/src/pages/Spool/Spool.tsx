import { Box, Button, Container, VStack, Spacer, HStack, Text } from "@chakra-ui/react";
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
import { userStore } from "../../stores/UserStore";
import { DeleteIcon } from '@chakra-ui/icons'
import UserSettingsAPI from "../../api/UserSettingsApi";

export const Spool = observer(() => {
    const profile = userStore.userProfile;
    const { id } = useParams();
    const [spool, setSpool] = useState<ISpool>();
    const [threads, setThreads] = useState<IThreadFull[]>([]);
    const [belongs, setBelongs] = useState<boolean>();
    const isAuthenticated = authStore.isAuthenticated;

    React.useEffect(() => {
        if (id) {
            SpoolAPI.getSpoolById(id).then((spool) => {
                setSpool(spool);
            });

            SpoolAPI.getSpoolThreads(id).then((threads) => {
                setThreads(threads);
            });

            if (profile && spool) {
                UserSettingsAPI.getJoinedSpool(profile!.id, id!).then((belongs) => {
                    setBelongs(belongs);
                });
            }
        }
    }, [id]);

    const removeSpool = () => {
        UserSettingsAPI.removeSpoolUser(profile!.id, spool!.id);
    }

    return (
        <PageLayout title={spool ? spool.name + ": New Posts" : ""}>
            {spool &&
                (
                    <Container centerContent={false} maxW={"container.md"}>
                        <VStack>
                            {isAuthenticated &&
                            <Box border="1px solid gray" borderRadius="3px" bgColor={"white"} w="100%" p="0.5rem">
                                <HStack>
                                    <NavLink to={"/s/" + spool.name + "/createthread"}><Button leftIcon={<IoCreateOutline />} colorScheme='green'>Create Post</Button></NavLink>
                                    {/* TODO: this can either be changed to be the owners Username or just remove it.*/}
                                    <Spacer />
                                    <Text as='i'>Owner: {spool!.ownerId}</Text>
                                    {/* TODO: up to here*/}
                                    {spool.ownerId !== profile?.id && belongs &&<>
                                            <Spacer />
                                            <NavLink to={""}><Button leftIcon={<DeleteIcon />} colorScheme='red' onClick={() => { removeSpool() }}>Leave Spool</Button></NavLink>
                                    </>}
                                </HStack>
                                </Box>
                            }
                            <PostFeed threads={threads} />
                        </VStack>
                    </Container>
                )}
        </PageLayout>
    );
});