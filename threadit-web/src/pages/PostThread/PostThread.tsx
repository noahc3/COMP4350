import React from "react";
import { Button, Card, CardBody, Flex, FormControl, FormLabel, Input, Stack, Textarea } from "@chakra-ui/react";
import { PageLayout } from "../../containers/PageLayout/PageLayout";
import { navStore } from "../../stores/NavStore";
import { useParams } from "react-router";
import SpoolAPI from "../../api/SpoolAPI";
import { ISpool } from "../../models/Spool";
import ThreadAPI from "../../api/ThreadAPI";

export default function PostThread() {
    const { spoolName } = useParams();
    const [spool, setSpool] = React.useState<ISpool>();
    const [lockInputs, setLockInputs] = React.useState(false);
    const [title, setTitle] = React.useState('');
    const [content, setContent] = React.useState('');
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
                const success = await ThreadAPI.postThread(title, content, "topic-placeholder", spool.id);
                if (success) {
                    navStore.navigateTo("/s/" + spool.name + "/");
                }
                else {
                    setCreateError('Invalid spool name');
                    //need to re-render the page here without resetting all the fields
                }
            }
            catch (e) {
                setCreateError('Invalid thread title or content.');
            }
            finally {
                setLockInputs(false);
            }
        }
    }

    return (
        <PageLayout title="Post a thread">
            {spool ? (
                <Flex direction={"column"} className="thread" margin="20px" bgColor="white" border="1px solid grey" borderRadius={"3px"}>
                    <Stack spacing='3'>
                        <Card>
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
                                        <FormLabel>Thread Content</FormLabel>
                                        <Textarea disabled={lockInputs} size='md' height='200px' value={content} onChange={(e) => setContent(e.target.value)} />
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