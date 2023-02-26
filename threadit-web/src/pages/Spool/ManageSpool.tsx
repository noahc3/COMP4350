import { Box, Button, Container, VStack, Spacer, HStack, Text, FormControl, FormLabel, Input, Divider } from "@chakra-ui/react";
import { observer } from "mobx-react-lite";
import React, { useState } from "react";
import { useParams } from "react-router";
import SpoolAPI from "../../api/SpoolAPI";
import { PageLayout } from "../../containers/PageLayout/PageLayout";
import { PostFeed } from "../../containers/Posts/PostFeed";
import { ISpool } from "../../models/Spool";
import { IUserProfile } from "../../models/UserProfile";
import { IThreadFull } from "../../models/ThreadFull";
import { IoCreateOutline } from "react-icons/io5";
import { NavLink } from "react-router-dom";
import { authStore } from "../../stores/AuthStore";
import { userStore } from "../../stores/UserStore";
import { spoolUsersStore } from "../../stores/SpoolUsersStore";
import { DeleteIcon, CheckIcon, AddIcon, SettingsIcon } from '@chakra-ui/icons';
import UserSettingsAPI from "../../api/UserSettingsApi";

export const ManageSpool = observer(() => {
    const profile = userStore.userProfile;
    const { id } = useParams();
    const [spool, setSpool] = useState<ISpool>();
    const isAuthenticated = authStore.isAuthenticated;
    const [rules, setRules] = React.useState(spool?.rules);

    const users = spoolUsersStore.users?.map(function (user) {
        return (
            <HStack mb={"1rem"} key={user.id}>
                <Button size="sm" leftIcon={<AddIcon />} colorScheme='green' onClick={() => { addMod(user.id) }}></Button>
                <Text mb={"0.5rem"}>{user.username}</Text>
            </HStack>
        );
    });

    const moderators = spoolUsersStore.moderators?.map(function (moderator) {
        return (
            <HStack mb={"1rem"} key={moderator.id}>
                <Button size="sm" leftIcon={<DeleteIcon />} colorScheme='red' onClick={() => { removeMod(moderator.id) }}></Button>
                <Text mb={"0.5rem"}>{moderator.username}</Text>
            </HStack>
        );
    });

    React.useEffect(() => {
        if (id) {
            SpoolAPI.getSpoolById(id).then((spool) => {
                setSpool(spool);
                //if these are both uncommented, profile.id is apparently null, but it fixes the non mod users list. line 50 does not fix mods though
                //spoolUsersStore.refreshAllUsers(spool.id, profile!.id);
                //spoolUsersStore.refreshAllModerators(spool.id);
            });
        }
    }, [id, profile])

    const removeMod = (userId: string) => {
        SpoolAPI.removeModerator(spool!.id, userId);
        spoolUsersStore.refreshAllUsers(spool!.id, profile!.id);
        spoolUsersStore.refreshAllModerators(spool!.id);
    }

    const addMod = (userId: string) => {
        SpoolAPI.addModerator(spool!.id, userId);
        spoolUsersStore.refreshAllUsers(spool!.id, profile!.id);
        spoolUsersStore.refreshAllModerators(spool!.id);
    }

    const saveSettings = () => {

    }

    const deleteSpool = () => {

    }

    return (
        <PageLayout title={spool ? spool.name + ": Settings" : ""}>
            {spool && spool.ownerId === profile?.id && (
                    <Container centerContent={false} maxW={"container.md"}>
                        <VStack>
                            {isAuthenticated &&
                            <Box border="1px solid gray" borderRadius="3px" bgColor={"white"} w="100%" p="0.5rem">
                                <FormControl>
                                    <FormLabel>Thread Rules</FormLabel>
                                    <Input size='md' value={rules} onChange={(e) => setRules(e.target.value)} />
                                </FormControl>
                                <Spacer />

                                <Divider />
                                <Box overflowX="auto" h="50%">
                                    <Text mb={"0.5rem"} fontWeight={"bold"}>Add Moderators</Text>
                                    {users}
                                </Box>

                                <Divider />
                                <Box overflowX="auto" h="50%">
                                    <Text mb={"0.5rem"} fontWeight={"bold"}>Remove Moderators</Text>
                                    {moderators}
                                </Box>

                                <Divider />
                                <HStack>
                                    <Button colorScheme={"purple"} width='120px' onClick={() => { saveSettings() }}>
                                        Save
                                    </Button>
                                    <Spacer />
                                    <Button colorScheme={"red"} width='120px' onClick={() => { deleteSpool() }}>
                                        Delete
                                    </Button>
                                </HStack>
                            </Box>

                            }
                        </VStack>
                    </Container>
                )}
        </PageLayout>
    );
});