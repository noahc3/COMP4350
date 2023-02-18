import React from "react";
import { Button, Card, CardBody, Flex, FormControl, FormLabel, Input, Stack} from "@chakra-ui/react";
import { PageLayout } from "../../containers/PageLayout/PageLayout";
import { navStore } from "../../stores/NavStore";
import { userStore } from "../../stores/UserStore";
import SpoolAPI from "../../api/SpoolAPI";

export default function PostSpool() {
    const profile = userStore.userProfile;
    const [title, setTitle] = React.useState('');
    const [lockInputs, setLockInputs] = React.useState(false);
    const [interests, setInterests] = React.useState('');



    const postSpool = async () => {
        if (profile) {
            setLockInputs(true);
            try {
                await SpoolAPI.PostSpool(title, profile.id, interests.split(","), []);
                navStore.navigateTo("/s/" + title + "/");
            } finally {
                setLockInputs(false);
            }
        }
        else {
            console.log("No user set.");
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
                                    <Input disabled={lockInputs} size='md' value={interests} onChange={(e) => setInterests(e.target.value)} />
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