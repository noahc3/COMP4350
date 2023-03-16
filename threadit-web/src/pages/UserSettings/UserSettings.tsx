import React from "react";
import { observer } from "mobx-react-lite";
import { PageLayout } from "../../containers/PageLayout/PageLayout";
import { Box, Button, HStack, StackDivider, Tab, TabList, TabPanel, TabPanels, Tabs, useColorMode, VStack, Text, Avatar, Tag, TagLeftIcon, TagLabel, TagCloseButton } from "@chakra-ui/react";

import { userStore } from "../../stores/UserStore";
import { authStore } from "../../stores/AuthStore";
import UserSettingsAPI from "../../api/UserSettingsApi";

export const UserSettings = observer(() => {
    const profile = userStore.userProfile;
    const isAuthenticated = authStore.isAuthenticated;
    const{colorMode, toggleColorMode} = useColorMode();
    const[interests, setInterests] = React.useState<string[]>([]);

    React.useEffect(() => {
        UserSettingsAPI.getUserInterests().then((interests) => {
            setInterests(interests);
        });
    }, [profile, isAuthenticated]);

    const removeInterest = async (interestMod: string) => {
            UserSettingsAPI.removeUserInterest(interestMod).then(
                (interests) => { setInterests(interests) }
            );
    }


    const interested = interests?.map(function (interest) {
        return (
            <HStack spacing={1} justifyContent={'center'} marginTop={50}>
                <Tag size={"lg"} colorScheme={"purple"} variant={"solid"} >
                    <TagLabel>{interest}</TagLabel>
                    <TagCloseButton onClick={() => {removeInterest(interest)}}></TagCloseButton>
                </Tag>
            </HStack>
        );
    });

        return (
            <Box className="profilepanel"  rounded='lg' w='xxl' h='100%'> 
                <PageLayout title="Profile">
                    <Tabs variant={'soft-rounded'}>
                        <TabList justifyContent={'center'}>
                            <Tab>User Settings</Tab>
                            <Tab>Interests</Tab>
                        </TabList>
                        <TabPanels>
                            <TabPanel>
                                <VStack spacing={10} justifyContent={'center'}>
                                    <Text fontSize={'xl'}>Profile Picture:</Text>    
                                    <Avatar size={'xxl'} name='Dan Abrahmov' src='/img/avatar_placeholder.png' />              
                                
                                    <Button size={'md'} colorScheme={'purple'}>
                                    Change Profile Picture
                                    </Button>     
                                </VStack>
                                <HStack spacing={1} justifyContent={'center'} marginTop={50}>
                                    <Text fontSize={'xl'}>Theme:</Text>
                                    <Button onClick={toggleColorMode} size={'md'} colorScheme={'purple'}>
                                        {colorMode === "light" ? "Dark" : "Light"}
                                    </Button>
                                </HStack>
                            </TabPanel>
                            <TabPanel>
                                <VStack spacing ={5} justifyContent={'center'}>
                                <Text fontSize={'xl'}>Interests:</Text>
                                    {interested}
                                </VStack>
                            </TabPanel>
                        </TabPanels>
                    </Tabs>
                </PageLayout>
            </Box>
        );
});