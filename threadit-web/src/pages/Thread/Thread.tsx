import React from "react";
import { PageLayout } from "../../containers/PageLayout/PageLayout";
import { Card, CardHeader, CardBody, CardFooter } from '@chakra-ui/react'
import { Text } from '@chakra-ui/react'
import { Box, Button, Divider, Flex, Icon, Image, Spacer } from "@chakra-ui/react";


export default class Overview extends React.Component {
    render() {
        return (
            // <PageLayout title="Thread"></PageLayout>
            <Flex direction={"column"} className="thread">
                <Card>
                    <CardHeader>
                        <Text>Thread Title</Text>
                    </CardHeader>
                    <CardBody>
                        <Text>View a summary of all your customers over the last month.</Text>
                    </CardBody>
                </Card>
            </Flex>
        );
    }
}