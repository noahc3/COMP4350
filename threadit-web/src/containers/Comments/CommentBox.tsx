import { Spacer, Text, Button, ButtonGroup, Textarea, VStack, Flex } from "@chakra-ui/react";
import { observer } from "mobx-react";
import React from "react";

export const CommentBox = observer(
  ({submitCallback, cancelCallback}: {submitCallback: Function, cancelCallback?: Function | undefined}) => {

    const [content, setContent] = React.useState<string>('');
    const [isSubmitting, setIsSubmitting] = React.useState<boolean>(false);

    const disableInputs = isSubmitting;

    const submit = async () => {
        setIsSubmitting(true);
        await submitCallback(content);
        setIsSubmitting(false);
        if (cancelCallback) {
            cancelCallback();
        } else {
            setContent('');
        }
    }

    return (
      <>
        <VStack w='100%' alignItems={'end'}>
            <Textarea disabled={disableInputs} placeholder="What are your thoughts?" maxH={'30rem'} w='100%' value={content} onChange={(e) => {setContent(e.target.value)}}></Textarea>
            <Flex direction={'row'} w='100%' alignItems={'center'}>
                <Text color={'blackAlpha.600'}>You should familiarize yourself with the spool's rules before commenting.</Text>
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
