import { Box, Button, Container, VStack, Spacer, HStack, Text, FormControl, FormLabel, Input, Divider, AlertDialog, AlertDialogBody, AlertDialogFooter, AlertDialogHeader, AlertDialogContent, AlertDialogOverlay, useDisclosure, } from "@chakra-ui/react";
import { observer } from "mobx-react-lite";
import React, { useRef, useState } from "react";
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
import { DeleteIcon, CheckIcon, AddIcon, SettingsIcon, StarIcon } from '@chakra-ui/icons';
import UserSettingsAPI from "../../api/UserSettingsApi";
import { navStore } from "../../stores/NavStore";

export const ManageSpool = observer(() => {
    const profile = userStore.userProfile;
    const { id } = useParams();
    const [spool, setSpool] = useState<ISpool>();
    const isAuthenticated = authStore.isAuthenticated;
    const [rules, setRules] = React.useState(spool?.rules);
    const { isOpen, onOpen, onClose } = useDisclosure()
    const cancelRef = useRef<HTMLButtonElement>(null);

    const users = spoolUsersStore.users?.map(function (user) {
        return (
            <HStack mb={"1rem"} key={user.id}>
                <Button size="sm" leftIcon={<StarIcon />} colorScheme='orange' onClick={() => { changeOwner(user.id) }}></Button>
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

    const nonModerators = spoolUsersStore.nonModerators?.map(function (nonModerator) {
        return (
            <HStack mb={"1rem"} key={nonModerator.id}>
                <Button size="sm" leftIcon={<AddIcon />} colorScheme='green' onClick={() => { addMod(nonModerator.id) }}></Button>
                <Text mb={"0.5rem"}>{nonModerator.username}</Text>
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
        spoolUsersStore.refreshAllNonModerator(spool!.id, profile!.id);
        spoolUsersStore.refreshAllUsers(spool!.id, profile!.id);
        spoolUsersStore.refreshAllModerators(spool!.id);

    }

    const addMod = (userId: string) => {
        SpoolAPI.addModerator(spool!.id, userId);
        spoolUsersStore.refreshAllNonModerator(spool!.id, profile!.id);
        spoolUsersStore.refreshAllUsers(spool!.id, profile!.id);
        spoolUsersStore.refreshAllModerators(spool!.id);
    }

    const changeOwner = (userId: string) => {
        SpoolAPI.changeOwner(spool!.id, userId);
    }

    const deleteSpool = () => {
        SpoolAPI.deleteSpool(spool!.id);
        navStore.navigateTo("/");
    }

    const save = () => {
        SpoolAPI.saveSpool(spool!.id, spool!.rules);
        navStore.navigateTo("/");
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
                                    {nonModerators}
                                </Box>

                                <Divider />
                                <Box overflowX="auto" h="50%">
                                    <Text mb={"0.5rem"} fontWeight={"bold"}>Remove Moderators</Text>
                                    {moderators}
                                </Box>

                                <Divider />
                                <Box overflowX="auto" h="50%">
                                    <Text mb={"0.5rem"} fontWeight={"bold"}>Change Ownership</Text>
                                    {users}
                                </Box>

                                <Divider />
                                <HStack>
                                    <Button colorScheme={"purple"} width='120px' onClick={() => { save() }}>
                                        Save
                                    </Button>
                                    <Spacer />
                                    <Button colorScheme={"red"} width='120px' onClick={onOpen}>
                                        Delete
                                    </Button>

                                    <AlertDialog
                                        isOpen={isOpen}
                                        onClose={onClose}
                                        leastDestructiveRef = {cancelRef}
                                    >
                                        <AlertDialogOverlay>
                                            <AlertDialogContent>
                                                <AlertDialogHeader fontSize='lg' fontWeight='bold'>
                                                    Delete Spool
                                                </AlertDialogHeader>

                                                <AlertDialogBody>
                                                    Are you sure? You can't undo this action afterwards.
                                                </AlertDialogBody>

                                                <AlertDialogFooter>
                                                    <Button ref={cancelRef} onClick={onClose}>
                                                        Cancel
                                                    </Button>
                                                    <Button colorScheme='red' onClick={deleteSpool} ml={3}>
                                                        Delete
                                                    </Button>
                                                </AlertDialogFooter>
                                            </AlertDialogContent>
                                        </AlertDialogOverlay>
                                    </AlertDialog>
                                </HStack>
                            </Box>

                            }
                        </VStack>
                    </Container>
                )}
        </PageLayout>
    );
});