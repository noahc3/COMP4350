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

export const Home = observer(() => {

    const isAuthenticated = authStore.isAuthenticated;
    const { id } = useParams();
    const [threads, setThreads] = useState<IThreadFull[]>([]);
    const [interests, setInterests] = useState<IInterest[]>([]);
    const [userInterests, setUserInterests] = useState<string[]>([]);
    const {isOpen, onOpen, onClose} = useDisclosure();
    const [belongs, setBelongs] = useState<boolean | undefined>(undefined);
    const [isLoadingBelongs, setIsLoadingBelongs] = useState<boolean>(true);
    
    React.useEffect(() => {
        ThreadAPI.getAllThreads().then((threads) => {
            setThreads(threads);
        });
    }, []);

    React.useEffect(() => {
        InterestAPI.getAllInterests().then((interests) => {
            setInterests(interests);
        });
    }, []);

    React.useEffect(() => {
        UserSettingsAPI.getUserInterests().then((interests) => {
            setUserInterests(interests);
        });
    }, [])

    React.useEffect(() => {
        setIsLoadingBelongs(belongs === undefined)
    }, [belongs, setIsLoadingBelongs])

    const addInterest = async (interestMod: IInterest) => {
        await UserSettingsAPI.addUserInterest(interestMod.name);
    }

    /*React.useEffect(() => {
        // dont ask.
        if (id && profile) {
            (async () => { setBelongs(await UserSettingsAPI.belongUserInterest({interests.name})) })()
        }
    }, [id, profile])*/

    /*
        {belongs ? <>
            <Button leftIcon={<DeleteIcon />} colorScheme='red' onClick={() => { removeSpool() }}>
                Leave Spool
            </Button>
        </> : <>
            <Button isLoading={isLoadingBelongs} leftIcon={<CheckIcon />} colorScheme={isLoadingBelongs ? 'gray' : 'green'} onClick={() => { joinSpool() }}>
                Join Spool
            </Button>
        </>}
    */

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