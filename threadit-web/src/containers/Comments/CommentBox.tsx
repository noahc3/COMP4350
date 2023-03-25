import { Spacer, Text, Button, ButtonGroup, Textarea, VStack, Flex, Tabs, TabList, Tab, TabPanels, TabPanel, Box } from "@chakra-ui/react";
import { observer } from "mobx-react";
import React from "react";
import { useColorMode } from "@chakra-ui/react";
import { mode } from '@chakra-ui/theme-tools'
import ReactMarkdown from "react-markdown";
import ChakraUIRenderer from "chakra-ui-markdown-renderer";

export const CommentBox = observer(
  ({submitCallback, cancelCallback}: {submitCallback: Function, cancelCallback?: Function | undefined}) => {

    const colorMode = useColorMode();
    const [content, setContent] = React.useState<string>('');
    const [isSubmitting, setIsSubmitting] = React.useState<boolean>(false);
    const tabsRef = React.useRef<any>(null);

    const disableInputs = isSubmitting;

    const submit = async () => {
        setIsSubmitting(true);
        await submitCallback(content);
        setIsSubmitting(false);
        if (cancelCallback) {
            cancelCallback();
        } else {
            setContent('');
            if (tabsRef) {
                tabsRef.current.index = 0
            }
        }
    }

    return (
      <>
        <VStack w='100%' alignItems={'end'}>
            <Tabs w='100%' >
                <TabList>
                    <Tab>Edit</Tab>
                    <Tab isDisabled={content.length === 0}>Preview</Tab>
                </TabList>
                <TabPanels>
                    <TabPanel>
                    <Textarea disabled={disableInputs} placeholder="What are your thoughts?" maxH={'30rem'} w='100%' value={content} onChange={(e) => {setContent(e.target.value)}}></Textarea>
                    </TabPanel>
                    <TabPanel>
                        <Box border='1px' borderRadius={'5'} borderColor={'chakra-border-color'} padding={'3'}>
                            <ReactMarkdown components={ChakraUIRenderer()} disallowedElements={['h1', 'h2', 'h3', 'img']} children={content} skipHtml/>
                        </Box>
                    </TabPanel>
                </TabPanels>
            </Tabs>
            <Flex direction={'row'} w='100%' alignItems={'center'}>
                <Text color={mode('blackAlpha.600', 'gray.300')(colorMode)}>You should familiarize yourself with the spool's rules before commenting.</Text>
                <Spacer/>
                <ButtonGroup size={'sm'}>
                    {cancelCallback && <Button onClick={() => {cancelCallback()}} disabled={disableInputs}>Cancel</Button>}
                    <Button isLoading={isSubmitting} disabled={disableInputs || content.length === 0} colorScheme={'purple'} onClick={() => {submit()}}>Comment</Button>
                </ButtonGroup>
            </Flex>
        </VStack>
      </>
    );
  }
);
