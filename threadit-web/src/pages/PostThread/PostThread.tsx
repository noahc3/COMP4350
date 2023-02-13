import React from "react";
import Select from 'react-select'
import { Button, Card, CardBody, Flex, Input, Stack, Textarea} from "@chakra-ui/react";
import { PageLayout } from "../../containers/PageLayout/PageLayout";
import { navStore } from "../../stores/NavStore";
import { threadStore } from "../../stores/ThreadStore";

export default function PostThread() {
    const [lockInputs, setLockInputs] = React.useState(false);

    const [title, setTitle] = React.useState('');
    const [content, setContent] = React.useState('');
    // const [topic, setTopic] = React.useState('topic-placeholder');
    // const [spoolId, setSpoolId] = React.useState('spoolid-placeholder');

    // const [threadError, setThreadError] = React.useState('');

    const options = [
        { value: 'spool1', label: 'spool1' },
        { value: 'spool2', label: 'spool2' },
        { value: 'spool3', label: 'spool3' }
      ]

    const postThread = async () => {
        setLockInputs(true);
        try {
            const success = await threadStore.postThread(title, content, "topic-placeholder", "spoolid_placeholder");
            if (!success) {
                // setThreadError('Invalid thread parameters.');
            } else {
                navStore.navigateTo("/");
            }
        } finally {
            setLockInputs(false);
        }
    }

    return (
        <PageLayout title="Post a thread">
        <Flex direction={"column"} className="thread" margin="20px">
            <Stack spacing='3'>
                <Select options={options} placeholder='Select a spool...'/>
                <Card>
                    <CardBody>
                        <Stack spacing='3'>
                            <Input disabled={lockInputs} placeholder='Thread Title' size='md' value={title} onChange={(e) => setTitle(e.target.value)}/>
                            <Textarea disabled={lockInputs} placeholder='Thread Content' size='md' height='200px' value={content} onChange={(e) => setContent(e.target.value)}/>
                            <Button colorScheme={"purple"} width='120px' onClick={() => { postThread() }}>
                                Post
                            </Button>
                        </Stack>
                    </CardBody>
                </Card>
            </Stack>
        </Flex>
    </PageLayout>
    );
}