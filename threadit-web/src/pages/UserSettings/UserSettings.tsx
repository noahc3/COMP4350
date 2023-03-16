import React from "react";
import { observer } from "mobx-react-lite";
import { PageLayout } from "../../containers/PageLayout/PageLayout";
import { Box, Button, HStack, StackDivider, Tab, TabList, TabPanel, TabPanels, Tabs, useColorMode, VStack, Text, Avatar } from "@chakra-ui/react";

export const UserSettings = observer(() => {
    const{colorMode, toggleColorMode} = useColorMode();

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
                                <Text fontSize={'xl'}>Interests:</Text>
                            </TabPanel>
                        </TabPanels>
                    </Tabs>
                </PageLayout>
            </Box>
        );
});