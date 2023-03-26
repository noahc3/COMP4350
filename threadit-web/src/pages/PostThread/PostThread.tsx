import React from "react";
import { Alert, AlertIcon, Box, Button, Card, CardBody, Flex, FormControl, FormLabel, HStack, Input, Radio, RadioGroup, Stack, Tab, TabList, TabPanel, TabPanels, Tabs, Textarea } from "@chakra-ui/react";
import { PageLayout } from "../../containers/PageLayout/PageLayout";
import { navStore } from "../../stores/NavStore";
import { useParams } from "react-router";
import SpoolAPI from "../../api/SpoolAPI";
import { ISpool } from "../../models/Spool";
import ThreadAPI from "../../api/ThreadAPI";
import { ThreadTypes } from "../../constants/ThreadTypes";
import { useColorMode } from "@chakra-ui/react";
import { mode } from '@chakra-ui/theme-tools'
import { ThreaditMarkdown } from "../../containers/Markdown/ThreaditMarkdown";

export default function PostThread() {
    const colorMode = useColorMode();
    const { spoolName } = useParams();
    const [spool, setSpool] = React.useState<ISpool>();
    const [lockInputs, setLockInputs] = React.useState(false);
    const [title, setTitle] = React.useState('');
    const [threadType, setThreadType] = React.useState(ThreadTypes.TEXT);
    const [contentText, setContentText] = React.useState('');
    const [contentUrl, setContentUrl] = React.useState('');
    const [createError, setCreateError] = React.useState('');

    React.useEffect(() => {
        if (spoolName) {
            SpoolAPI.getSpoolByName(spoolName).then((spool) => {
                setSpool(spool);
            });
        }
    }, [spoolName]);

    const postThread = async () => {
        if (spool) {
            setLockInputs(true);
            try {
                const success = await ThreadAPI.postThread(title, threadType === ThreadTypes.TEXT ? contentText : contentUrl, "topic-placeholder", spool.id, threadType);
                if (success) {
                    navStore.navigateTo("/s/" + spool.name + "/");
                }
                else {
                    setCreateError('Invalid spool name');
                }
            }
            catch (e) {
                if (e instanceof Error) {
                    setCreateError(e.message);
                }
            }
            finally {
                setLockInputs(false);
            }
        }
    }

    const contentLabel = (() => {
        switch (threadType) {
            case ThreadTypes.TEXT: {
                return "Thread Content"
            }
            case ThreadTypes.LINK: {
                return "URL"
            }
            case ThreadTypes.IMAGE: {
                return "Image URL"
            }
        }
    })()

    return (
        <PageLayout title="Post a thread">
            {spool ? (
                <Flex direction={"column"} className="thread" margin="20px" border="1px solid grey" borderRadius={"3px"}>
                    <Stack spacing='3'>
                        <Card bgColor={mode("white", "gray.800")(colorMode)}>
                            <CardBody>
                                {createError.length > 0 && (
                                    <Alert status='error'>
                                        <AlertIcon />
                                        {createError}
                                    </Alert>
                                )}
                                <Stack spacing='3'>
                                    <FormControl>
                                        <FormLabel>Spool</FormLabel>
                                        <Input readOnly={true} size='md' value={spool.name} />
                                    </FormControl>
                                    <FormControl>
                                        <FormLabel>Thread Title</FormLabel>
                                        <Input disabled={lockInputs} size='md' value={title} onChange={(e) => setTitle(e.target.value)} />
                                    </FormControl>
                                    <FormControl>
                                        <FormLabel>Thread Type</FormLabel>
                                        <RadioGroup onChange={(t) => {setThreadType(t as ThreadTypes)}} value={threadType}>
                                            <HStack spacing={'8'}>
                                                <Radio value={ThreadTypes.TEXT}>Text</Radio>
                                                <Radio value={ThreadTypes.LINK}>Link</Radio>
                                                <Radio value={ThreadTypes.IMAGE}>Image</Radio>
                                            </HStack>
                                        </RadioGroup>
                                    </FormControl>
                                    <FormControl>
                                        <FormLabel>{contentLabel}</FormLabel>
                                        {threadType === ThreadTypes.TEXT ? (
                                            <>
                                                <Tabs>
                                                    <TabList>
                                                        <Tab>Edit</Tab>
                                                        <Tab isDisabled={contentText.length === 0}>Preview</Tab>
                                                    </TabList>
                                                    <TabPanels>
                                                        <TabPanel>
                                                            <Textarea disabled={lockInputs} size='md' height='200px' value={contentText} onChange={(e) => setContentText(e.target.value)} />
                                                        </TabPanel>
                                                        <TabPanel>
                                                            <Box border='1px' borderRadius={'5'} borderColor={'chakra-border-color'} padding={'3'}>
                                                                <ThreaditMarkdown text={contentText}/>
                                                            </Box>
                                                        </TabPanel>
                                                    </TabPanels>
                                                </Tabs>
                                            </>
                                        ) : (
                                            <Input disabled={lockInputs} size='md' value={contentUrl} onChange={(e) => setContentUrl(e.target.value)} />
                                        )}
                                    </FormControl>
                                    <Button colorScheme={"purple"} width='120px' onClick={() => { postThread() }}>
                                        Post
                                    </Button>
                                </Stack>
                            </CardBody>
                        </Card>
                    </Stack>
                </Flex>
            ) : (
                <div>Loading...</div>
            )}
        </PageLayout>
    );
}