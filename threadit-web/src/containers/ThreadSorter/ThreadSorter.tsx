import { observer } from "mobx-react"
import { Box, Tab, Tabs, TabList} from "@chakra-ui/react";
import React from "react";
import { navStore } from "../../stores/NavStore";
import SpoolAPI from "../../api/SpoolAPI";
import ThreadAPI from "../../api/ThreadAPI";
import { IThreadFull } from "../../models/ThreadFull";
import { useParams } from "react-router";
import { useSearchParams } from 'react-router-dom';

export const ThreadSorter = observer(({ onThreadsSorted }: { onThreadsSorted: (threads: IThreadFull[]) => void }) => {
      
    const { id } = useParams();
    const { sortType } = useParams();
    const [searchParams] = useSearchParams();
    const searchString = searchParams.get('q');

    React.useEffect(() => {
        if (id){
            SpoolAPI.getSpoolThreads(id, sortType ? sortType : "", searchString ? searchString : "").then((threads) => {
                onThreadsSorted(threads);
            });
        }
        else{
            ThreadAPI.getAllThreads(sortType ? sortType : "", searchString ? searchString : "").then((threads) => {
                onThreadsSorted(threads);
            });
        }
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [id, sortType, searchString])

    const sortThreads = (sortType: string) => {
        if(id){
            if(searchString){
                navStore.navigateTo(`/s/${id}/${sortType}/?q=${searchString}`);
            }
            else{
                navStore.navigateTo(`/s/${id}/${sortType}`);
            }
        }
        else{
            navStore.navigateTo(`/${sortType}`);
        }

    }

    const getSelectedTabIndex = () => {
        return sortType === "hot" ? 1 : sortType === "top" ? 2 : sortType === "controversial" ? 3 : sortType === "comments" ? 4 : 0;
    }

    return(
        <Box border="1px solid gray" borderRadius="3px" bgColor={"white"} w="100%" h="50%" p="0.5rem">
        <Tabs index={getSelectedTabIndex()} variant='soft-rounded' colorScheme='purple'>
            <TabList>
                <Tab onClick={() => { sortThreads("new") }}>New</Tab>
                <Tab onClick={() => { sortThreads("hot")  }}>Hot</Tab>
                <Tab onClick={() => { sortThreads("top")  }}>Top</Tab>
                <Tab onClick={() => { sortThreads("controversial")  }}>Controversial</Tab>
                <Tab onClick={() => { sortThreads("comments")  }}>Comments</Tab>
            </TabList>
        </Tabs>
        </Box>
    )
})