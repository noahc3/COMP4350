import { observer } from "mobx-react"
import { Avatar, Box, Button, Divider, Flex, HStack, Icon, Image, Spacer, Text } from "@chakra-ui/react";
import "./Sidebar.scss";
import { IoStatsChart } from "react-icons/io5";
import { IoMdLogIn } from "react-icons/io";
import { MdOutlineExitToApp } from "react-icons/md";
import { NavLink } from "../Router/NavLink";
import { userStore } from "../../stores/UserStore";
import { authStore } from "../../stores/AuthStore";

export const Sidebar = observer(() => {
    const profile = userStore.userProfile;
    const isAuthenticated = authStore.isAuthenticated;

    const logout = async () => {
        authStore.logout();
    }

    return (
        <Flex direction={"column"} className="sidebar">
            <Image src="/logo.png" alt="Threadit" className="logo" />
            <Divider />
            <NavLink to={"/"}><Button leftIcon={<Icon as={IoStatsChart} />} colorScheme={"purple"}>Home</Button></NavLink>
            <Divider />
            <NavLink to={"/spool/create"}><Button leftIcon={<Icon as={IoStatsChart} />} colorScheme={"purple"}>Create Spool</Button></NavLink>
            <Text mb={"0.5rem"} fontWeight={"bold"}>Spools</Text>
            <NavLink to={"/s/AskThreadit"}><Button colorScheme={"purple"}>AskThreadit</Button></NavLink>
            <NavLink to={"/s/hockey"}><Button colorScheme={"purple"}>hockey</Button></NavLink>
            <Spacer />
            {profile && <>
                <Divider />
                <Box >
                    <HStack marginInlineStart={2}>
                        <Avatar size={'sm'} name='Dan Abrahmov' src='/img/avatar_placeholder.png' />
                        <Text>{profile.username}</Text>
                    </HStack>
                </Box>
            </>}
            <Divider />
            {isAuthenticated && (
                <>
                    <Button leftIcon={<Icon transform={"scaleX(-1)"} as={MdOutlineExitToApp} />} colorScheme={"purple"} onClick={() => { logout() }}>Logout</Button>
                </>
            )}

            {!isAuthenticated && (
                <>
                    <NavLink to={"/login"}><Button leftIcon={<Icon as={IoMdLogIn} />} colorScheme={"purple"}>Login</Button></NavLink>
                </>
            )}
        </Flex>
    )
}) 