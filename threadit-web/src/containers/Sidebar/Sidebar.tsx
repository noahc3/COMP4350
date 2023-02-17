import { observer } from "mobx-react"
import { Box, Button, Divider, Flex, Icon, Image, Spacer, Text } from "@chakra-ui/react";
import "./Sidebar.scss";
import { IoStatsChart } from "react-icons/io5";
import { FaWrench } from "react-icons/fa";
import { IoMdLogIn } from "react-icons/io";
import { MdOutlineExitToApp } from "react-icons/md";
import { NavLink } from "../Router/NavLink";
import { userStore } from "../../stores/UserStore";
import { authStore } from "../../stores/AuthStore";

export const Sidebar = observer(() => {
    const profile = userStore.userProfile;
    const isAuthenticated = authStore.isAuthenticated;
    return (
        <Flex direction={"column"} className="sidebar">
            <Image src="/logo.png" alt="Threadit" className="logo" />
            <Divider />
            <NavLink to={"/"}><Button leftIcon={<Icon as={IoStatsChart} />} colorScheme={"purple"}>Home</Button></NavLink>
            <Divider />
            <Text mb={"0.5rem"} fontWeight={"bold"}>Spools</Text>
            <NavLink to={"/s/AskThreadit"}><Button colorScheme={"purple"}>AskThreadit</Button></NavLink>
            <NavLink to={"/s/hockey"}><Button colorScheme={"purple"}>hockey</Button></NavLink>
            <Spacer />
            {profile && <>
                <Divider />
                <Box paddingInlineStart={4} paddingInlineEnd={4} textColor={'gray.300'} fontSize={'xs'}>
                    {profile.email}<br />{profile.id}
                </Box>
            </>}
            <Divider />
            {isAuthenticated && (
                <>
                    <NavLink to={"/settings"}><Button leftIcon={<Icon as={FaWrench} />} colorScheme={"purple"}>Settings</Button></NavLink>
                    <Button leftIcon={<Icon transform={"scaleX(-1)"} as={MdOutlineExitToApp} />} colorScheme={"purple"}>Logout</Button>
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