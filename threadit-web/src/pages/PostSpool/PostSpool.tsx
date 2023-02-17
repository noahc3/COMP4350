import React from "react";
import { Button, Card, CardBody, Flex, FormControl, FormLabel, Input, Stack, Textarea } from "@chakra-ui/react";
import { PageLayout } from "../../containers/PageLayout/PageLayout";
import { navStore } from "../../stores/NavStore";
import { useParams } from "react-router";
import SpoolAPI from "../../api/SpoolAPI";
import { ISpool } from "../../models/Spool";
import ThreadAPI from "../../api/ThreadAPI";

export default function PostThread() {


    const postThread = async () => {
    }

    return (
        <PageLayout title="Post a Spool">
        </PageLayout>
    );
}