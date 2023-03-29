import {
  Alert,
  AlertIcon,
  Box,
  Button,
  Container,
  VStack,
  Spacer,
  HStack,
  Text,
  FormControl,
  FormLabel,
  Input,
  Divider,
  Textarea,
  AlertDialog,
  AlertDialogBody,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogContent,
  AlertDialogOverlay,
  useDisclosure,
  InputGroup,
  InputRightElement,
  IconButton,
  Flex,
} from "@chakra-ui/react";
import { observer } from "mobx-react-lite";
import React, { useRef, useState } from "react";
import { useParams } from "react-router";
import SpoolAPI from "../../api/SpoolAPI";
import { PageLayout } from "../../containers/PageLayout/PageLayout";
import { ISpool } from "../../models/Spool";
import { IUserProfile } from "../../models/UserProfile";
import { authStore } from "../../stores/AuthStore";
import { userStore } from "../../stores/UserStore";
import { DeleteIcon, AddIcon, StarIcon } from "@chakra-ui/icons";
import { navStore } from "../../stores/NavStore";
import { NavLink } from "react-router-dom";
import { useColorMode } from "@chakra-ui/react";
import { mode } from "@chakra-ui/theme-tools";

export const ManageSpool = observer(() => {
  const profile = userStore.userProfile;
  const isAuthenticated = authStore.isAuthenticated;
  const colorMode = useColorMode();
  const { name: spoolName } = useParams();
  const [spool, setSpool] = useState<ISpool>();
  const [rules, setRules] = useState(spool?.rules || "");
  const [moderators, setModerators] = useState<IUserProfile[]>([]);
  const [lastUpdate, setLastUpdate] = useState(new Date());
  const { isOpen, onOpen, onClose } = useDisclosure();
  const cancelRef = useRef<HTMLButtonElement>(null);
  const [modToAdd, setModToAdd] = useState<string>("");
  const [ownerToAdd, setOwnerToAdd] = useState<string>("");
  const [addError, setAddError] = React.useState("");
  const [changeError, setChangeError] = React.useState("");

  React.useEffect(() => {
    if (spoolName) {
      SpoolAPI.getSpoolByName(spoolName).then((res) => {
        setSpool(res);
        setRules(res.rules);
      });
    }
  }, [spoolName]);

  React.useEffect(() => {
    if (spool && profile) {
      Promise.all([SpoolAPI.getAllMods(spool.id)]).then((res) => {
        setModerators(res[0]);
      });
    }
  }, [spool, lastUpdate, profile]);

  const moderatorsElements = moderators.map(function (moderator) {
    return (
      <HStack mb={"1rem"} key={moderator.id}>
        <IconButton
          size="sm"
          aria-label="Delete Moderator"
          icon={<DeleteIcon />}
          colorScheme="red"
          onClick={() => {
            removeMod(moderator.id);
          }}
        >
          Delete
        </IconButton>
        <Text mb={"0.5rem"}>{moderator.username}</Text>
      </HStack>
    );
  });

  const removeMod = async (userId: string) => {
    if (spool) {
      await SpoolAPI.removeModerator(spool.id, userId);
      setLastUpdate(new Date());
    }
  };

  const addMod = async () => {
    if (spool) {
      try {
        const successNumber = await SpoolAPI.addModerator(spool.id, modToAdd);
        if (successNumber === true) {
          //was successful
          setLastUpdate(new Date());
          setAddError("");
          setModToAdd("");
        } else {
          //user does not exist
          setAddError(successNumber.toString());
        }
      } finally {
        //do nothing
      }
    }
  };

  const changeOwner = async () => {
    if (spool) {
      try {
        const successNumber = await SpoolAPI.changeOwner(spool.id, ownerToAdd);
        if (successNumber === true) {
          //was successful
          setLastUpdate(new Date());
          setChangeError("");
          navStore.navigateTo("/s/" + spoolName);
        } else {
          //user is already the owner
          setChangeError(successNumber.toString());
        }
      } finally {
        //do nothing
      }
    }
  };

  const deleteSpool = async () => {
    if (spool) {
      await SpoolAPI.deleteSpool(spool.id);
      navStore.navigateTo("/");
    }
  };

  const save = async () => {
    if (spool) {
      await SpoolAPI.saveSpool(spool.id, rules);
    }
  };

  return (
    <PageLayout title={spool ? spool.name + ": Settings" : ""}>
      <Container centerContent={false} maxW={"container.md"}>
        <VStack>
          {spool && isAuthenticated && (
            <Box
              border="1px solid gray"
              borderRadius="3px"
              bgColor={mode("white", "gray.800")(colorMode)}
              w="100%"
              p="0.5rem"
            >
              <FormControl>
                <FormLabel>Rules and Description:</FormLabel>
                <InputGroup size="md">
                  <Textarea
                    size="lg"
                    value={rules || ""}
                    onChange={(e) => setRules(e.target.value)}
                  />
                </InputGroup>
                <Flex direction="row">
                  <Spacer />
                  <Button
                    colorScheme={"purple"}
                    width="120px"
                    onClick={() => {
                      save();
                    }}
                  >
                    Save Rules
                  </Button>
                </Flex>
              </FormControl>
              <Spacer />

              <Divider />
              <Box overflowX="auto" h="50%">
                <FormControl>
                  <FormLabel>Add Moderator:</FormLabel>
                  <InputGroup size="md">
                    <Input
                      size="md"
                      value={modToAdd}
                      onChange={(e) => setModToAdd(e.target.value)}
                    />
                    <InputRightElement width="100px">
                      <Button
                        leftIcon={<AddIcon />}
                        colorScheme={"green"}
                        width="100px"
                        onClick={() => {
                          addMod();
                        }}
                      >
                        Add
                      </Button>
                    </InputRightElement>
                  </InputGroup>
                </FormControl>
                {addError.length > 0 && (
                  <Container p="0.5rem">
                    <Alert status="error">
                      <AlertIcon />
                      {addError}
                    </Alert>
                  </Container>
                )}
              </Box>

              {moderators.length > 0 && (
                <>
                  <Divider />
                  <Box overflowX="auto" h="50%">
                    <Text mb={"0.5rem"} fontWeight={"bold"}>
                      Remove Moderators
                    </Text>
                    {moderatorsElements}
                  </Box>
                </>
              )}

              <Divider />
              <Box overflowX="auto" h="50%">
                <FormControl>
                  <FormLabel>Change Owner:</FormLabel>
                  <InputGroup size="md">
                    <Input
                      size="md"
                      value={ownerToAdd}
                      onChange={(e) => setOwnerToAdd(e.target.value)}
                    />
                    <InputRightElement width="100px">
                      <Button
                        leftIcon={<StarIcon />}
                        colorScheme={"orange"}
                        width="100px"
                        onClick={() => {
                          changeOwner();
                        }}
                      >
                        Change
                      </Button>
                    </InputRightElement>
                  </InputGroup>
                </FormControl>

                {changeError.length > 0 && (
                  <Container p="0.5rem">
                    <Alert status="error">
                      <AlertIcon />
                      {changeError}
                    </Alert>
                  </Container>
                )}
              </Box>

              <Divider />
              <HStack>
                <NavLink to={"/s/" + spoolName}>
                  <Button colorScheme={"purple"} width="120px">
                    Back
                  </Button>
                </NavLink>
                <Spacer />
                <Button colorScheme={"red"} width="120px" onClick={onOpen}>
                  Delete
                </Button>

                <AlertDialog
                  isOpen={isOpen}
                  onClose={onClose}
                  leastDestructiveRef={cancelRef}
                >
                  <AlertDialogOverlay>
                    <AlertDialogContent>
                      <AlertDialogHeader fontSize="lg" fontWeight="bold">
                        Delete Spool
                      </AlertDialogHeader>

                      <AlertDialogBody>
                        Are you sure? You can't undo this action afterwards.
                      </AlertDialogBody>

                      <AlertDialogFooter>
                        <Button ref={cancelRef} onClick={onClose}>
                          Cancel
                        </Button>
                        <Button colorScheme="red" onClick={deleteSpool} ml={3}>
                          Delete
                        </Button>
                      </AlertDialogFooter>
                    </AlertDialogContent>
                  </AlertDialogOverlay>
                </AlertDialog>
              </HStack>
            </Box>
          )}
        </VStack>
      </Container>
    </PageLayout>
  );
});
