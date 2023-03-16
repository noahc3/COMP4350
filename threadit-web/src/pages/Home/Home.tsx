import { ChatIcon, QuestionIcon, SearchIcon } from "@chakra-ui/icons";
import { Box, Button, Container, HStack, IconButton, Modal, ModalBody, ModalCloseButton, ModalContent, ModalHeader, ModalOverlay, position, useDisclosure, VStack } from "@chakra-ui/react";
import { observer } from "mobx-react-lite";
import React, { useState } from "react";
import { BiChat } from "react-icons/bi";
import ThreadAPI from "../../api/ThreadAPI";
import UserSettingsAPI from "../../api/UserSettingsApi";
import { PageLayout } from "../../containers/PageLayout/PageLayout";
import { PostFeed } from "../../containers/Posts/PostFeed";
import { IThreadFull } from "../../models/ThreadFull";

export const Home = observer(() => {

    const [threads, setThreads] = useState<IThreadFull[]>([]);
    const [userInterests, setUserInterests] = useState<string[]>([]);
    const {isOpen, onOpen, onClose} = useDisclosure();
    
    React.useEffect(() => {
        ThreadAPI.getAllThreads().then((threads) => {
            setThreads(threads);
        });
    }, []);

    React.useEffect(() => {
        UserSettingsAPI.getUserInterests().then((userInterests) => {
            setUserInterests(userInterests);
        });
    }, []);


    const interestButtons = userInterests?.map(function (interest) {
        return (
            <Button colorScheme={"purple"}>{interest}</Button>
        );
    });
    
    return (
        <PageLayout title="New Posts">
            <Box position="fixed" bottom="20px" right="16px" p="4">
                <IconButton onClick={onOpen}  aria-label={"Open interest quiz"} isRound = {true} icon={<ChatIcon/>}>Open Modal</IconButton>
            </Box>
            <Container centerContent={false} maxW={"container.md"}>
                <HStack>
                    <VStack w="100%">
                        <PostFeed threads={threads}/>
                    </VStack>
                </HStack>
            </Container>
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
        </PageLayout>   
    );
});