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
import { DeleteIcon, CheckIcon } from '@chakra-ui/icons'
import UserSettingsAPI from "../../api/UserSettingsApi";

export const Spool = observer(() => {
    const profile = userStore.userProfile;
    const { id } = useParams();
    const [spool, setSpool] = useState<ISpool>();
    const [threads, setThreads] = useState<IThreadFull[]>([]);
    const [belongs, setBelongs] = useState<boolean | undefined>(undefined);
    const [isLoadingBelongs, setIsLoadingBelongs] = useState<boolean>(true);
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
    }, [id, profile])

    React.useEffect(() => {
        // dont ask.
        (async () => { setBelongs(await UserSettingsAPI.getJoinedSpool(profile!.id, id!)) })()
    }, [id, profile])

    React.useEffect(() => {
        setIsLoadingBelongs(belongs === undefined)
    }, [belongs, setIsLoadingBelongs])

    const removeSpool = () => {
        UserSettingsAPI.removeSpoolUser(profile!.id, id!).then(
            () => { setBelongs(false) }
        );
    }

    const joinSpool = () => {
        UserSettingsAPI.joinSpoolUser(profile!.id, id!).then(
            () => { setBelongs(true) }
        );
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
                                        <NavLink to={"/s/" + spool.name + "/createthread"}><Button leftIcon={<IoCreateOutline />} colorScheme='blue'>Create Post</Button></NavLink>
                                        {/* TODO: this can either be changed to be the owners Username or just remove it.*/}
                                        <Spacer />
                                        <Text as='i'>Owner: {spool!.ownerId}</Text>
                                        {/* TODO: up to here*/}
                                        {spool.ownerId !== profile?.id &&
                                            <>
                                                <Spacer />
                                                {belongs ? <>
                                                    <Button leftIcon={<DeleteIcon />} colorScheme='red' onClick={() => { removeSpool() }}>
                                                        Leave Spool
                                                    </Button>
                                                </> : <>
                                                    <Button isLoading={isLoadingBelongs} leftIcon={<CheckIcon />} colorScheme={isLoadingBelongs ? 'gray' : 'green'} onClick={() => { joinSpool() }}>
                                                        Join Spool
                                                    </Button>
                                                </>}
                                            </>
                                        }
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