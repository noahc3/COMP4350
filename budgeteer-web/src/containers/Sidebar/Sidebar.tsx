import { observer } from "mobx-react"
import { Box, Button, Divider, Flex, Icon, Image, Spacer, Text } from "@chakra-ui/react";
import "./Sidebar.scss";
import { IoStatsChart, IoWallet, IoFilter, IoTelescope } from "react-icons/io5";
import { BiCategory } from "react-icons/bi";
import { FaWrench } from "react-icons/fa";
import { MdOutlineExitToApp } from "react-icons/md";
import { NavLink } from "../Router/NavLink";
import { userStore } from "../../stores/UserStore";

export const Sidebar = observer(() => {
    const profile = userStore.userProfile;
    return (
        <Flex direction={"column"} className="sidebar">
            <Image src="/logo.png" alt="Power User's Budgeteer" className="logo" />
            <Divider />
            <NavLink to={"/overview"}><Button leftIcon={<Icon as={IoStatsChart} />} colorScheme={"purple"}>Overview</Button></NavLink>
            <NavLink to={"/accounts"}><Button leftIcon={<Icon as={IoWallet} />} colorScheme={"purple"}>Accounts</Button></NavLink>
            <NavLink to={"/projections"}><Button leftIcon={<Icon as={IoTelescope} />} colorScheme={"purple"}>Projections</Button></NavLink>
            <Divider />
            <NavLink to={"/categories"}><Button leftIcon={<Icon as={BiCategory} />} colorScheme={"purple"}>Categories</Button></NavLink>
            <NavLink to={"/identify"}><Button leftIcon={<Icon as={IoFilter} />} colorScheme={"purple"}>Transaction Rules</Button></NavLink>
            <Spacer />
            {profile && <>
                <Divider />
                <Box paddingInlineStart={4} paddingInlineEnd={4} textColor={'gray.300'} fontSize={'xs'}>
                    {profile.email}<br/>{profile.id}
                </Box>
            </>}
            <Divider />
            <NavLink to={"/categories"}><Button leftIcon={<Icon as={FaWrench} />} colorScheme={"purple"}>Settings</Button></NavLink>
            <NavLink to={"/categories"}><Button leftIcon={<Icon transform={"scaleX(-1)"} as={MdOutlineExitToApp} />} colorScheme={"purple"}>Logout</Button></NavLink>
        </Flex>
    )
}) 