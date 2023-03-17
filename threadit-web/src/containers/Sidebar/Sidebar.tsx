import { observer } from "mobx-react"
import { Avatar, Box, Button, Divider, Flex, HStack, Icon, Image, Spacer, Text } from "@chakra-ui/react";
import "./Sidebar.scss";
import { IoStatsChart } from "react-icons/io5";
import { IoMdLogIn } from "react-icons/io";
import { MdOutlineExitToApp } from "react-icons/md";
import { NavLink } from "../Router/NavLink";
import { userStore } from "../../stores/UserStore";
import { authStore } from "../../stores/AuthStore";
import { spoolStore } from "../../stores/SpoolStore";
import { AddIcon } from "@chakra-ui/icons";
import React from "react";
import { BiCog } from "react-icons/bi";
import { useColorMode } from "@chakra-ui/react";
import { mode } from "@chakra-ui/theme-tools";

export const Sidebar = observer(() => {
    const colorMode = useColorMode();
    const profile = userStore.userProfile;
    const isAuthenticated = authStore.isAuthenticated;
    const allSpools = spoolStore.allSpools;
    const joinedSpools = spoolStore.joinedSpools;
    const suggestedSpools = spoolStore.suggestedSpools;
    const allButtons = allSpools?.map(function (spool) {
        return (
            <NavLink to={"/s/" + spool.name} key={spool.id}>
                <Button textColor={"white"} colorScheme={mode("purple.500", "purple.900")(colorMode)}>{spool.name}</Button>
            </NavLink>
        );
    });

    const suggestedButtons = suggestedSpools?.map(function (spool) {
        return (
            <NavLink to={"/s/" + spool.name} key={spool.id}>
                <Button textColor={"white"} colorScheme={mode("purple.500", "purple.900")(colorMode)}>{spool.name}</Button>
            </NavLink>
        );
    });

    const joinedButtons = joinedSpools?.map(function (spool) {
        return (
            <NavLink to={"/s/" + spool.name} key={spool.id}>
                <Button textColor={"white"} colorScheme={mode("purple.500", "purple.900")(colorMode)}>{spool.name}</Button>
            </NavLink>
        );
    });

    React.useEffect(() => { 
        spoolStore.refreshJoinedSpools();
    }, [profile, isAuthenticated])

    React.useEffect(() => {
        spoolStore.refreshSuggestedSpools();
    }, [profile, joinedSpools])

    const logout = async () => {
        authStore.logout();
    }

    return (
        <Flex direction={"column"} className="sidebar" bgColor={mode("purple.500", "purple.900")(colorMode)}>
            <Image src="/logo.png" alt="Threadit" className="logo" />
            <Divider />
            <NavLink to={"/"}><Button leftIcon={<Icon as={IoStatsChart} />} textColor="white" colorScheme={mode("purple.500", "purple.900")(colorMode)}>Home (DEMO)</Button></NavLink>
            {profile && <>
                <NavLink to={"/createSpool"}><Button leftIcon={<Icon as={AddIcon} />} textColor="white" colorScheme={mode("purple.500", "purple.900")(colorMode)}>Create Spool</Button></NavLink>
            </>}
            <Divider />
            {!profile && <>
                <Box overflowX="auto" h="50%">
                    <Text mb={"0.5rem"} fontWeight={"bold"}>All Spools</Text>
                    <>{allButtons}</>
                </Box>
            </>}

            <Box overflowX="auto" h="50%">
                {profile && <>
                    <Spacer />
                    <Text mb={"0.5rem"} fontWeight={"bold"}>Joined Spools</Text>
                    <>{joinedButtons}</>
                </>}
                <Spacer />
            </Box>
        
            <Box overflowX="auto" h="50%">
                {profile && <>
                    <Spacer />
                    <Text mb={"0.5rem"} fontWeight={"bold"}>Suggested Spools</Text>
                    <>{suggestedButtons}</>
                </>}
                <Spacer />
            </Box>

            {profile && <>
                <Divider />
                <Box >
                    <HStack marginLeft={"8.3px"} marginBottom={2}>
                        <Avatar size={'sm'} name='Dan Abrahmov' src='/img/avatar_placeholder.png' />
                        <Text>{profile.username}</Text>
                    </HStack>
                        <NavLink to={"/profile"}><Button textColor={"white"} colorScheme={mode("purple.500", "purple.900")(colorMode)} leftIcon={<Icon as={BiCog} />}>Profile</Button></NavLink>
                </Box>
            </>}
            <Divider />
            {isAuthenticated && (
                <>
                    <Button leftIcon={<Icon transform={"scaleX(-1)"} as={MdOutlineExitToApp} />} textColor="white" colorScheme={mode("purple.500", "purple.900")(colorMode)} onClick={() => { logout() }}>Logout</Button>
                </>
            )}

            {!isAuthenticated && (
                <>
                    <NavLink to={"/login"}><Button leftIcon={<Icon as={IoMdLogIn} />} textColor="white" colorScheme={mode("purple.500", "purple.900")(colorMode)}>Login</Button></NavLink>
                </>
            )}
        </Flex>
    )
}) 