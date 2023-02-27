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
    const isAuthenticated = authStore.isAuthenticated;
    const { name: spoolName } = useParams();
    const [spool, setSpool] = useState<ISpool>();
    const [rules, setRules] = useState(spool?.rules || "");
    const [moderators, setModerators] = useState<IUserProfile[]>([]);
    const [lastUpdate, setLastUpdate] = useState(new Date());
    const { isOpen, onOpen, onClose } = useDisclosure()
    const cancelRef = useRef<HTMLButtonElement>(null);
    const [modToAdd, setModToAdd] = useState<string>("");
    const [ownerToAdd, setOwnerToAdd] = useState<string>("");

    React.useEffect(() => {
        if (spoolName) {
            SpoolAPI.getSpoolByName(spoolName).then((res) => {
                setSpool(res);
                setRules(res.rules);
            });
        }
    }, [spoolName])

    React.useEffect(() => {
        if (spool && profile) {
            Promise.all([
                SpoolAPI.getAllMods(spool.id),
                SpoolAPI.getAllNonModerator(spool.id, profile.id),
                SpoolAPI.getAllUsers(spool.id, profile.id)
            ]).then((res) => {
                setModerators(res[0]);
            });
        }
    }, [spool, lastUpdate, profile])

    const moderatorsElements = moderators.map(function (moderator) {
        return (
            <HStack mb={"1rem"} key={moderator.id}>
                <Button size="sm" leftIcon={<DeleteIcon />} colorScheme='red' onClick={() => { removeMod(moderator.id) }}></Button>
                <Text mb={"0.5rem"}>{moderator.username}</Text>
            </HStack>
        );
    });

    const removeMod = async (userId: string) => {
        if (spool) {
            //await SpoolAPI.removeModerator(spool.id, userId);
            console.log("called removeMod");
            setLastUpdate(new Date());
        }
    }

    const addMod = async () => {
        if (spool) {
            await SpoolAPI.addModerator(spool.id, modToAdd);
            setLastUpdate(new Date());
        }
    }

    const changeOwner = async () => {
        if (spool) {
            //await SpoolAPI.changeOwner(spool.id, userId);
            console.log("called changeOwner");
            navStore.navigateTo("/s/" + spoolName);
        }
    }

    const deleteSpool = async () => {
        if (spool) {
            await SpoolAPI.deleteSpool(spool.id);
            navStore.navigateTo("/");
        }
    }

    const save = async () => {
        if (spool) {
            await SpoolAPI.saveSpool(spool.id, rules);
            navStore.navigateTo("/s/" + spoolName);
        }
    }

    return (
        <PageLayout title={spool ? spool.name + ": Settings" : ""}>
            <Container centerContent={false} maxW={"container.md"}>
                    <VStack>
                        {(spool && isAuthenticated) &&
                            <Box border="1px solid gray" borderRadius="3px" bgColor={"white"} w="100%" p="0.5rem">
                                <Text as='b'>Current Rules: </Text>
                                <Text as='i'>{spool?.rules}</Text>
                                <FormControl>
                                    <FormLabel>New Rules:</FormLabel>
                                    <Input
                                        size='md'
                                        value={rules || ""}
                                        onChange={(e) => setRules(e.target.value)}
                                    />
                                </FormControl>
                                <Spacer />

                                <Divider />
                                <Box overflowX="auto" h="50%">
                                    <FormControl>
                                        <FormLabel>Add Moderator:</FormLabel>
                                            <Input
                                                size='md'
                                                value={modToAdd}
                                                onChange={(e) => setModToAdd(e.target.value)}
                                            />
                                    </FormControl>
                                    <Button leftIcon={<DeleteIcon />} colorScheme={"green"} width='120px' onClick={() => { addMod() }}>
                                        Add
                                    </Button>
                                </Box>

                                <Divider />
                                <Box overflowX="auto" h="50%">
                                    <Text mb={"0.5rem"} fontWeight={"bold"}>Remove Moderators</Text>
                                    {moderatorsElements}
                                </Box>

                                <Divider />
                                <Box overflowX="auto" h="50%">
                                    <FormControl>
                                        <FormLabel>Change Owner:</FormLabel>
                                        <Input
                                            size='md'
                                            value={ownerToAdd}
                                            onChange={(e) => setOwnerToAdd(e.target.value)}
                                        />
                                    </FormControl>
                                    <Button leftIcon={<DeleteIcon />} colorScheme={"orange"} width='120px' onClick={() => { changeOwner() }}>
                                        Change
                                    </Button>
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
                                        leastDestructiveRef={cancelRef}
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
        </PageLayout>
    );
});