import { AddIcon, StarIcon} from "@chakra-ui/icons";
import { Box, Button, Container, HStack, IconButton, Modal, ModalBody, ModalCloseButton, ModalContent, ModalHeader, ModalOverlay, useDisclosure, VStack, Tag, TagLabel, TagRightIcon, Alert, AlertIcon } from "@chakra-ui/react";
import { observer } from "mobx-react-lite";
import React, { useState } from "react";
import ThreadAPI from "../../api/ThreadAPI";
import UserSettingsAPI from "../../api/UserSettingsApi";
import InterestAPI from "../../api/InterestAPI";
import { PageLayout } from "../../containers/PageLayout/PageLayout";
import { PostFeed } from "../../containers/Posts/PostFeed";
import { IThreadFull } from "../../models/ThreadFull";
import { IInterest } from "../../models/Interest";
import { authStore } from "../../stores/AuthStore";
import { useParams } from "react-router";
import { ThreadSorter } from "../../containers/ThreadSorter/ThreadSorter";
import { spoolStore } from "../../stores/SpoolStore";
import { navStore } from "../../stores/NavStore";
import {Select, SingleValue, ActionMeta } from "chakra-react-select"

export const Home = observer(() => {
    const isAuthenticated = authStore.isAuthenticated;
    const [threads, setThreads] = useState<IThreadFull[]>([]);
    const [interests, setInterests] = useState<IInterest[]>([]);
    const {isOpen, onOpen, onClose} = useDisclosure();


    const allSpools = spoolStore.allSpools;

    const options = allSpools ? allSpools.map((spool) => ({
        value: spool.id,
        label: spool.name
      })) : [];

    const onSpoolSelected = (selectedOption: SingleValue<{ value: string; label: string; }>, actionMeta: ActionMeta<{ value: string; label: string; }>) => {
        if(selectedOption){
            navStore.navigateTo(`/s/${selectedOption.label}`)
        }
    }

    const setSortedThreads = (threads: IThreadFull[]) => {
        setThreads(threads);
      };

    React.useEffect(() => {
        ThreadAPI.getAllThreads("", "").then((threads) => {
            setThreads(threads);
        });
    }, []);

    React.useEffect(() => {
        InterestAPI.getAllInterests().then((interests) => {
            setInterests(interests);
        });
    }, []);

    const addInterest = async (interestMod: IInterest) => {
        await UserSettingsAPI.addUserInterest(interestMod.name);
    }

    const interestButtons = interests?.map(function (interest) {
        return (
            <HStack spacing={1} justifyContent={'center'}>
                <Button rightIcon={<AddIcon/>} size={"lg"} colorScheme={"purple"} variant={"outline"} onClick={() => {addInterest(interest)}}>
                    <label>{interest.name}</label>
                </Button>
            </HStack>
        );
    });
    
    return (
        <PageLayout title="New Posts">
            {isAuthenticated &&
            <Box position="fixed" bottom="20px" right="16px" p="4">
                <IconButton onClick={onOpen}  aria-label={"Open interest quiz"} isRound = {true} icon={<StarIcon/>}>Open Modal</IconButton>
            </Box>
            }
            <Container centerContent={false} maxW={"container.md"}>
                <HStack>
                    <VStack w="100%">
                        <Box border="1px solid gray" borderRadius="3px" bgColor={"white"} w="100%" h="50%" p="0.5rem">
                            <Select
                                options={options}
                                onChange={onSpoolSelected}
                                placeholder="Search"
                            />
                        </Box>
                        <ThreadSorter onThreadsSorted={setSortedThreads}></ThreadSorter>
                        <PostFeed threads={threads}/>
                    </VStack>
                </HStack>
            </Container>
            { isAuthenticated &&
            <Modal blockScrollOnMount={false} isOpen={isOpen} onClose={onClose}>
            <ModalOverlay />
            <ModalContent>
                <ModalHeader>Select Your Interests</ModalHeader>
                <ModalCloseButton />
                <ModalBody>
                    <HStack>
                        <VStack w="100%">
                            {interestButtons}
                        </VStack>
                    </HStack>
                </ModalBody> 
            </ModalContent>
            </Modal>
            }
        </PageLayout>   
    );
});