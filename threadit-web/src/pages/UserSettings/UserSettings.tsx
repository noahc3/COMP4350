import React from "react";
import { observer } from "mobx-react-lite";
import { AddIcon, MinusIcon } from "@chakra-ui/icons";
import { PageLayout } from "../../containers/PageLayout/PageLayout";
import {
  Box,
  Button,
  HStack,
  Tab,
  TabList,
  TabPanel,
  TabPanels,
  Tabs,
  useColorMode,
  VStack,
  Text,
  Avatar,
} from "@chakra-ui/react";

import { userStore } from "../../stores/UserStore";
import { authStore } from "../../stores/AuthStore";
import { interestStore } from "../../stores/InterestStore";
import UserSettingsAPI from "../../api/UserSettingsApi";
import { spoolStore } from "../../stores/SpoolStore";

export const UserSettings = observer(() => {
  const profile = userStore.userProfile;
  const isAuthenticated = authStore.isAuthenticated;
  const { colorMode, toggleColorMode } = useColorMode();
  const userInterests = interestStore.joinedInterests;
  const otherInterests = interestStore.otherInterests;

  React.useEffect(() => {
    interestStore.refreshJoinedInterests();
    interestStore.refreshOtherInterests();
  }, [profile, isAuthenticated, userInterests]);

  const removeInterest = async (interestMod: string) => {
    await UserSettingsAPI.removeUserInterest(interestMod);
    await spoolStore.refreshSuggestedSpools();
  };

  const addInterest = async (interestMod: string) => {
    await UserSettingsAPI.addUserInterest(interestMod);
    await spoolStore.refreshSuggestedSpools();
  };

  const interested = userInterests?.map(function (interest) {
    return (
      <HStack spacing={1} justifyContent={"left"} marginTop={50}>
        <Button rightIcon={<MinusIcon/>} colorScheme={"purple"} variant={"solid"} onClick={() => {removeInterest(interest);}}>
          <label>{interest}</label>
        </Button>
      </HStack>
    );
  });

  const notInterested = otherInterests?.map(function (interest) {
    return (
      <HStack spacing={1} justifyContent={"left"} marginTop={50}>
        <Button rightIcon={<AddIcon/>} colorScheme={"purple"} variant={"outline"} onClick={() => {addInterest(interest);}}>
          <label>{interest}</label>
        </Button>
      </HStack>
    );
  });

  return (
    <Box className="profilepanel" rounded="lg" w="xxl" h="100%">
      <PageLayout title="Profile">
        <Tabs variant={"soft-rounded"}>
          <TabList justifyContent={"center"}>
            <Tab>User Settings</Tab>
            <Tab>Interests</Tab>
          </TabList>
          <TabPanels>
            <TabPanel>
              <VStack spacing={10} justifyContent={"center"}>
                <Text fontSize={"xl"}>Profile Picture:</Text>
                <Avatar
                  size={"xxl"}
                  name="Dan Abrahmov"
                  src="/img/avatar_placeholder.png"
                />

                <Button size={"md"} colorScheme={"purple"}>
                  <label>Change Profile Picture</label>
                </Button>
              </VStack>
              <HStack spacing={1} justifyContent={"center"} marginTop={50}>
                <Text fontSize={"xl"}>Theme:</Text>
                <Button
                  onClick={toggleColorMode}
                  size={"md"}
                  colorScheme={"purple"}
                >
                  {colorMode === "light" ? "Dark" : "Light"}
                </Button>
              </HStack>
            </TabPanel>
            <TabPanel>
              <VStack spacing={5}>
                <Text fontSize={"xl"}>User Interests:</Text>
                {interested}
              </VStack>
              <VStack spacing={5} marginTop={15}>
                <Text fontSize={"xl"}>Other Interests:</Text>
                {notInterested}
              </VStack>
            </TabPanel>
          </TabPanels>
        </Tabs>
      </PageLayout>
    </Box>
  );
});
