import { Box, Button, Container, VStack, Spacer, HStack, Text, Center} from "@chakra-ui/react";
import { observer } from "mobx-react-lite";
import React, { useState } from "react";
import { useParams } from "react-router";
import SpoolAPI from "../../api/SpoolAPI";
import { PageLayout } from "../../containers/PageLayout/PageLayout";
import { PostFeed } from "../../containers/Posts/PostFeed";
import { ISpool } from "../../models/Spool";
import { IoCreateOutline } from "react-icons/io5";
import { NavLink } from "react-router-dom";
import { authStore } from "../../stores/AuthStore";
import { spoolUsersStore } from "../../stores/SpoolUsersStore";
import { userStore } from "../../stores/UserStore";
import { spoolStore } from "../../stores/SpoolStore";
import { navStore } from "../../stores/NavStore";
import { DeleteIcon, CheckIcon, SettingsIcon } from '@chakra-ui/icons';
import { OptionBase, Select, SingleValue, ActionMeta } from "chakra-react-select";
import UserSettingsAPI from "../../api/UserSettingsApi";
import { useSearchParams } from 'react-router-dom';
import { useColorMode } from "@chakra-ui/react";
import { mode } from '@chakra-ui/theme-tools'


export const Spool = observer(() => {
    interface Option extends OptionBase {
        label: string;
        value: string;
      }

    const profile = userStore.userProfile;
    const { id } = useParams();
    const colorMode = useColorMode();
    const [spool, setSpool] = useState<ISpool>();
    const [belongs, setBelongs] = useState<boolean | undefined>(undefined);
    const [isLoadingBelongs, setIsLoadingBelongs] = useState<boolean>(true);
    const { sortType } = useParams();
    const isAuthenticated = authStore.isAuthenticated;
    const [selectOption, setSelectOption] = useState<Option[]>([]);
    const [searchParams] = useSearchParams();
    const searchWord = searchParams.get('q') ?? undefined


    const handleInputChange = (newValue : string) => {
        setSelectOption([ {value: newValue, label: `Search for "${newValue}" in s/${spool ? spool.name : ''}`}]);
    }

    const handleSearch = (selectedOption: SingleValue<{ value: string; label: string; }>, actionMeta: ActionMeta<{ value: string; label: string; }>) => {
        if (spool && selectedOption){
            navStore.navigateTo(`/s/${spool.name}${sortType ? '/' + sortType : ''}/?q=${selectedOption.value}`)
        }
    }

    React.useEffect(() => {
        if (id) {
            SpoolAPI.getSpoolByName(id).then((spool) => {
                setSpool(spool);
                spoolUsersStore.refreshAllModerators(spool.id);
                spoolStore.refreshSpool(spool);
            });
        }
    }, [id, profile])

    React.useEffect(() => {
        // dont ask.
        if (id && profile) {
            (async () => { setBelongs(await UserSettingsAPI.getJoinedSpool(id!)) })()
        }
    }, [id, profile])

    React.useEffect(() => {
        setIsLoadingBelongs(belongs === undefined)
    }, [belongs, setIsLoadingBelongs])

    const removeSpool = () => {
        UserSettingsAPI.removeSpoolUser(id!).then(
            () => { setBelongs(false) }
        );
    }

    const joinSpool = () => {
        UserSettingsAPI.joinSpoolUser(id!).then(
            () => { setBelongs(true) }
        );
    }

    return (
        <PageLayout title={spool ? "s/" + spool.name : ""}>
            {spool &&
                (
                    <Container centerContent={false} maxW={"container.md"}>
                        <VStack>
                        {isAuthenticated &&
                            <VStack w='100%'>
                                <Box border="1px solid gray" borderRadius="3px" bgColor={mode("white", "gray.800")(colorMode)} w="100%" p="0.5rem">
                                    <HStack>
                                        <NavLink to={"/s/" + spool.name + "/createthread"}><Button leftIcon={<IoCreateOutline />} colorScheme='blue' >Create Post</Button></NavLink>
                                        {spool.ownerId !== profile?.id && <>
                                                <Spacer />
                                                {belongs ? <>
                                                    <Button leftIcon={<DeleteIcon />} colorScheme='red' onClick={() => { removeSpool() }}>
                                                        Leave Spool
                                                    </Button>
                                                </> : <>
                                                    <Button isLoading={isLoadingBelongs} leftIcon={<CheckIcon />} colorScheme={isLoadingBelongs ? 'gray' : 'green'} onClick={() => { joinSpool() }}>
                                                        Join Spool
                                                    </Button>
                                                </>}
                                        </>}
                                        {spool.ownerId === profile?.id && <>
                                            <Spacer />
                                            <NavLink to={"/s/" + spool.name + "/manage"}><Button leftIcon={<SettingsIcon />} colorScheme='green'>Manage Spool</Button></NavLink>
                                        </>}
                                    </HStack>
                                </Box>
                            </VStack>
                        }
                            <Box border="1px solid gray" borderRadius="3px" bgColor={mode("white", "gray.800")(colorMode)} w="100%" h="50%" p="0.5rem">
                                <Center>
                                    <Text as='b'><Text as='u' fontSize='lg' align='center'>RULES AND DESCRIPTION</Text></Text>
                                </Center>
                                <Text align="center" whiteSpace="pre-wrap">{spool.rules || ""}</Text>
                            </Box>

                            <Box border="1px solid gray" borderRadius="3px" bgColor={mode("white", "gray.800")(colorMode)} w="100%" h="50%" p="0.5rem">
                                <Select
                                    options={selectOption}
                                    value={[ {value: searchWord ? searchWord : "", label: searchWord ? searchWord : ""} ]}
                                    onChange={handleSearch}
                                    onInputChange={handleInputChange}
                                    noOptionsMessage={() => null}
                                    placeholder="Search"
                                />
                            </Box>
                            <PostFeed searchQuery={searchWord} spoolFilter={spool.id} />
                        </VStack>
                    </Container>
                )}
        </PageLayout>
    );
});