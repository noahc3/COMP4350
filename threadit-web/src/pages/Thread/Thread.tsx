import React from "react";
import { PageLayout } from "../../containers/PageLayout/PageLayout";
import { Card, CardHeader, CardBody, CardFooter } from '@chakra-ui/react'
import { Text } from '@chakra-ui/react'
import { Box, Button, Divider, Flex, Icon, Image, Spacer, Heading, Stack, StackDivider, Input, Textarea} from "@chakra-ui/react";
import { LockIcon } from '@chakra-ui/icons'

export default class Overview extends React.Component {
    render() {
        return (
            <PageLayout title="Post a thread">
                <Flex direction={"column"} className="thread" margin="20px">
                    <Card>
                        {/* <CardHeader>
                            <Heading size='md'>Thread d</Heading>
                        </CardHeader> */}
                        <CardBody>
                            <Stack spacing='3'>
                            {/* <Stack divider={<StackDivider />} spacing='4'> */}
                                <Input placeholder='Thread Title' size='md'/>
                                {/* <Divider orientation='horizontal' /> */}
                                <Textarea placeholder='Thread Description' size='md' height='200px'/>
                                <Button colorScheme={"purple"} width='120px'>
                                    Post
                                </Button>
                            {/* </Stack> */}
                            </Stack>
                        </CardBody>
                    </Card>

                    
                    {/* <Card>
                    <CardHeader>
                        <Heading size='md'>Thread Title</Heading>
                    </CardHeader>

                    <CardBody>
                        <Stack divider={<StackDivider />} spacing='4'>
                        <Box>
                            <Text pt='2' fontSize='sm'>
                            Thread description text...
                            </Text>
                        </Box>
                        </Stack>
                    </CardBody>
                    <CardFooter
                        justify='space-between'
                        flexWrap='wrap'
                        sx={{
                        '& > button': {
                            minW: '136px',
                        },
                        }}
                    >
                        <Button flex='1' variant='ghost' leftIcon={<LockIcon />}>
                        Like
                        </Button>
                        <Button flex='1' variant='ghost' leftIcon={<LockIcon />}>
                        Comment
                        </Button>
                        <Button flex='1' variant='ghost' leftIcon={<LockIcon />}>
                        Share
                        </Button>
                    </CardFooter>
                    </Card> */}
                </Flex>
            </PageLayout>
        );
    }
}