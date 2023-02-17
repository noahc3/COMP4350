import React from "react";
import { Button, Card, CardBody, Flex, FormControl, FormLabel, Input, Stack, Textarea } from "@chakra-ui/react";
import { PageLayout } from "../../containers/PageLayout/PageLayout";
import { navStore } from "../../stores/NavStore";
import { userStore } from "../../stores/UserStore";
import { useParams } from "react-router";
import SpoolAPI from "../../api/SpoolAPI";
import { ISpool } from "../../models/Spool";
import ThreadAPI from "../../api/ThreadAPI";

export default function PostThread() {
    const { spoolName } = useParams();
    const [spool, setSpool] = React.useState<ISpool>();
    const [title, setTitle] = React.useState('');
    const [lockInputs, setLockInputs] = React.useState(false);
    const [interests, setInterests] = React.useState('');



    const postSpool = async () => {
        setLockInputs(true);
        try {
            await SpoolAPI.PostSpool(title, id,,);
            navStore.navigateTo("/s/" + spool.name + "/");
        } finally {
            setLockInputs(false);
        }
    }

    return (
        <PageLayout title="Post a Spool">
            <Flex direction={"column"} className="thread" margin="20px" bgColor="white" border="1px solid grey" borderRadius={"3px"}>
                <Stack spacing='3'>
                    <Card>
                        <CardBody>
                            <Stack spacing='3'>
                                <FormControl>
                                    <FormLabel>Spool Title</FormLabel>
                                    <Input disabled={lockInputs} size='md' value={title} onChange={(e) => setTitle(e.target.value)} />
                                </FormControl>
                                <FormControl>
                                    <FormLabel>Interests</FormLabel>
                                    <Input disabled={lockInputs} size='md' value={interests} onChange={(e) => setTitle(e.target.value)} />
                                </FormControl>
                                <Button colorScheme={"purple"} width='120px' onClick={() => { postSpool() }}>
                                    Create
                                </Button>
                            </Stack>
                        </CardBody>
                    </Card>
                </Stack>
            </Flex>
        </PageLayout>
    );
}