import { observer } from "mobx-react"
import { Box, Tab, Tabs, TabList} from "@chakra-ui/react";
import React from "react";
import { navStore } from "../../stores/NavStore";
import SpoolAPI from "../../api/SpoolAPI";
import ThreadAPI from "../../api/ThreadAPI";
import { IThreadFull } from "../../models/ThreadFull";
import { useParams } from "react-router";
import { useSearchParams } from 'react-router-dom';
import { useColorMode } from "@chakra-ui/react";
import { mode } from '@chakra-ui/theme-tools'
import { SortTypes } from "../../constants/SortTypes";

export const ThreadSorter = observer(({ sortType, onSortChanged }: { sortType: string, onSortChanged: (sortType: SortTypes) => void }) => {
    const { colorMode } = useColorMode();

    const sortThreads = (sortType: SortTypes) => {
        onSortChanged(sortType)
    }

    const getSelectedTabIndex = () => {
        return sortType === "hot" ? 1 : sortType === "top" ? 2 : sortType === "controversial" ? 3 : 0;
    }

    return(
        <Box border="1px solid gray" borderRadius="3px" bgColor={mode("white", "gray.800")({colorMode})} w="100%" h="50%" p="0.5rem">
        <Tabs index={getSelectedTabIndex()} variant='soft-rounded' colorScheme='purple'>
            <TabList>
                <Tab onClick={() => { sortThreads(SortTypes.SORT_NEW) }}>New</Tab>
                <Tab onClick={() => { sortThreads(SortTypes.SORT_HOT)  }}>Hot</Tab>
                <Tab onClick={() => { sortThreads(SortTypes.SORT_TOP)  }}>Top</Tab>
                <Tab onClick={() => { sortThreads(SortTypes.SORT_CONTROVERSIAL)  }}>Controversial</Tab>
            </TabList>
        </Tabs>
        </Box>
    )
})