import {
  Box,
  Button,
  ButtonGroup,
  Heading,
  HStack,
  Spinner,
  Text,
  Textarea,
  VStack,
  useClipboard,
  Tabs,
  TabList,
  Tab,
  TabPanels,
  TabPanel,
} from "@chakra-ui/react";
import { observer } from "mobx-react-lite";
import React, { useState } from "react";
import ThreadAPI from "../../api/ThreadAPI";
import { IThreadFull } from "../../models/ThreadFull";
import { ArrowUpIcon, ArrowDownIcon } from "@chakra-ui/icons";
import Moment from "react-moment";
import { authStore } from "../../stores/AuthStore";
import { userStore } from "../../stores/UserStore";
import { IoCreateOutline, IoArrowRedo } from "react-icons/io5";
import { MdOutlineCancel, MdOutlineDelete } from "react-icons/md";
import { BiLinkExternal, BiSave } from "react-icons/bi";
import { navStore } from "../../stores/NavStore";
import { Link } from "react-router-dom";
import "./ThreadPost.scss";
import { ISpool } from "../../models/Spool";
import { useColorMode } from "@chakra-ui/react";
import { mode } from "@chakra-ui/theme-tools";
import { ThreadTypes } from "../../constants/ThreadTypes";
import { ThreaditMarkdown } from "../../containers/Markdown/ThreaditMarkdown";

export const ThreadPost = observer(
  ({ spool, thread }: { spool: ISpool; thread: IThreadFull }) => {
    const colorMode = useColorMode();
    const [isEditing, setIsEditing] = React.useState(false);
    const [isConfirmingDelete, setIsConfirmingDelete] = React.useState(false);
    const [isDeleting, setIsDeleting] = React.useState(false);
    const [isSaving, setIsSaving] = React.useState(false);
    const [editedText, setEditedText] = React.useState("");
    const threadId = thread.id;
    const isAuthenticated = authStore.isAuthenticated;
    const isThreadOwner =
      isAuthenticated && thread
        ? thread.ownerId === userStore.userProfile?.id
        : false;
    const isSpoolOwner =
      isAuthenticated && thread
        ? spool?.ownerId === userStore.userProfile?.id
        : false;
    const isModerator =
      isAuthenticated && thread
        ? spool?.moderators.includes(userStore.userProfile!.id)
        : false;
    const canEdit =
      thread &&
      thread.threadType === ThreadTypes.TEXT &&
      (isThreadOwner || isSpoolOwner || isModerator);
    const canDelete = thread && (isThreadOwner || isSpoolOwner || isModerator);
    const disableInputs = isSaving || isDeleting;
    const profile = userStore.userProfile;
    const [isStitched, setIsStitched] = useState(
      thread.stitches.includes(profile ? profile.id : "")
    );
    const [isRipped, setIsRipped] = useState(
      thread.rips.includes(profile ? profile.id : "")
    );
    const { onCopy, hasCopied } = useClipboard(window.location.href);

    const dateString = (
      <Moment fromNow>{thread ? thread.dateCreated : ""}</Moment>
    );

    const startEdit = () => {
      setIsEditing(true);
      setEditedText(thread ? thread.content : "");
    };

    const saveEdit = async () => {
      if (thread) {
        setIsSaving(true);
        thread.content = editedText;
        try {
          await ThreadAPI.editThread(thread);
        } finally {
          ThreadAPI.getThreadById(threadId).then((thread) => {
            setIsSaving(false);
            setIsEditing(false);
          });
        }
      }
    };

    const deleteThread = async () => {
      if (thread) {
        setIsDeleting(true);
        try {
          await ThreadAPI.deleteThread(thread.id);
        } finally {
          navStore.navigateTo("/s/" + thread.spoolName);
        }
      }
    };

    const stitchThread = async () => {
      if (thread) {
        const stitchedThread = await ThreadAPI.stitchThread(thread.id);
        if (stitchedThread) {
          updateStitchesAndRips(stitchedThread.stitches, stitchedThread.rips);
        }
      }
    };

    const ripThread = async () => {
      if (thread) {
        const rippedThread = await ThreadAPI.ripThread(thread.id);
        if (rippedThread) {
          updateStitchesAndRips(rippedThread.stitches, rippedThread.rips);
        }
      }
    };

    const updateStitchesAndRips = (
      newStitches: string[],
      newRips: string[]
    ) => {
      if (thread) {
        thread.stitches = newStitches;
        thread.rips = newRips;
        setIsStitched(thread.stitches.includes(profile ? profile.id : ""));
        setIsRipped(thread.rips.includes(profile ? profile.id : ""));
      }
    };

    let threadHeader = <></>;
    let threadContent = <></>;

    if (thread) {
      if (thread.threadType === ThreadTypes.TEXT) {
        threadHeader = (
          <Heading as="h3" size="md">
            {thread.title}
          </Heading>
        );
        threadContent = (
          <Box>
            <ThreaditMarkdown text={thread.content} />
          </Box>
        );
      } else if (thread.threadType === ThreadTypes.IMAGE) {
        threadHeader = (
          <Heading as="h3" size="md">
            {thread.title}
          </Heading>
        );
        threadContent = (
          <img alt={thread.content} loading="lazy" src={thread.content} />
        );
      } else if (thread.threadType === ThreadTypes.LINK) {
        const domain = new URL(thread.content).hostname;
        threadHeader = (
          <a href={thread.content} target="_blank" rel="noreferrer">
            <VStack alignItems={"start"}>
              <Heading as="h3" size="md">
                {thread.title}
              </Heading>
              <Button
                colorScheme={"blue"}
                variant={"link"}
                rightIcon={<BiLinkExternal />}
              >
                {domain}
              </Button>
            </VStack>
          </a>
        );
      }
    }

    return (
      <Box
        border="1px solid gray"
        borderRadius="3px"
        p="2rem"
        bgColor={mode("white", "gray.800")(colorMode)}
        w="100%"
        className="threadPost"
      >
        {thread ? (
          <VStack alignItems="start" spacing={"3"} w="100%">
            <HStack>
              <Link to={"/s/" + thread.spoolName}>
                <Text fontWeight={"bold"}>
                  s/{thread ? thread.spoolName : ""}
                </Text>
              </Link>
              <Text color={mode("blackAlpha.600", "gray.300")(colorMode)}>
                {" "}
                • Posted by u/{thread ? thread.authorName : ""} • {dateString}
              </Text>
            </HStack>
            {threadHeader}
            {thread && threadContent && (
              <>
                {!isEditing ? (
                  <>{threadContent}</>
                ) : (
                  <>
                    <Tabs w="100%">
                      <TabList>
                        <Tab>Edit</Tab>
                        <Tab>Preview</Tab>
                      </TabList>
                      <TabPanels>
                        <TabPanel>
                          <Textarea
                            minHeight={"320px"}
                            disabled={disableInputs}
                            value={editedText}
                            onChange={(e) => {
                              setEditedText(e.target.value);
                            }}
                          />
                        </TabPanel>
                        <TabPanel>
                          <Box
                            border="1px"
                            borderRadius={"5"}
                            borderColor={"chakra-border-color"}
                            padding={"3"}
                          >
                            <ThreaditMarkdown text={editedText} />
                          </Box>
                        </TabPanel>
                      </TabPanels>
                    </Tabs>
                  </>
                )}
              </>
            )}

            <HStack>
              <ButtonGroup size={"sm"} isAttached>
                <Button
                  leftIcon={<ArrowUpIcon />}
                  onClick={() => {
                    stitchThread();
                  }}
                  colorScheme={isStitched ? "blue" : "gray"}
                >
                  {thread.stitches.length}
                </Button>
                <Button
                  leftIcon={<ArrowDownIcon />}
                  onClick={() => {
                    ripThread();
                  }}
                  colorScheme={isRipped ? "red" : "gray"}
                >
                  {thread.rips.length}
                </Button>
              </ButtonGroup>
              <Button size={"sm"} leftIcon={<IoArrowRedo />} onClick={onCopy}>
                {hasCopied ? "Copied Link!" : "Share"}
              </Button>
              {(canEdit || canDelete) && (
                <>
                  {!isConfirmingDelete ? (
                    <ButtonGroup size={"sm"} isAttached>
                      {!isEditing ? (
                        <>
                          {canEdit && (
                            <Button
                              leftIcon={<IoCreateOutline />}
                              onClick={() => {
                                startEdit();
                              }}
                            >
                              Edit
                            </Button>
                          )}
                          {canDelete && (
                            <Button
                              leftIcon={<MdOutlineDelete />}
                              onClick={() => {
                                setIsConfirmingDelete(true);
                              }}
                            >
                              Delete
                            </Button>
                          )}
                        </>
                      ) : (
                        <>
                          <Button
                            isLoading={isSaving}
                            loadingText="Saving..."
                            disabled={disableInputs}
                            leftIcon={<BiSave />}
                            onClick={() => {
                              saveEdit();
                            }}
                          >
                            Save
                          </Button>
                          <Button
                            disabled={disableInputs}
                            leftIcon={<MdOutlineCancel />}
                            onClick={() => {
                              setIsEditing(false);
                            }}
                          >
                            Cancel
                          </Button>
                        </>
                      )}
                    </ButtonGroup>
                  ) : (
                    <>
                      <Text color={mode("red", "red.300")(colorMode)}>
                        Are you sure you want to delete?
                      </Text>
                      <ButtonGroup size={"sm"} isAttached>
                        <Button
                          disabled={disableInputs}
                          isLoading={isDeleting}
                          loadingText="Deleting..."
                          leftIcon={<MdOutlineDelete />}
                          colorScheme="red"
                          onClick={() => {
                            deleteThread();
                          }}
                        >
                          Delete
                        </Button>
                        <Button
                          disabled={disableInputs}
                          leftIcon={<MdOutlineCancel />}
                          onClick={() => {
                            setIsConfirmingDelete(false);
                          }}
                        >
                          Cancel
                        </Button>
                      </ButtonGroup>
                    </>
                  )}
                </>
              )}
            </HStack>
          </VStack>
        ) : (
          <Box textAlign="center" width="100%">
            <Spinner size={"xl"} thickness="4px" emptyColor="gray.200" />
          </Box>
        )}
      </Box>
    );
  }
);
