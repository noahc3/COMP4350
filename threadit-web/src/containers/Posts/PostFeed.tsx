import { VStack } from "@chakra-ui/layout";
import { Button } from "@chakra-ui/react";
import { observer } from "mobx-react";
import React from "react";
import ThreadAPI from "../../api/ThreadAPI";
import { SortTypes } from "../../constants/SortTypes";
import { ThreadSorter } from "../ThreadSorter/ThreadSorter";
import { FeedPostItem } from "./FeedPostItem";
import { ThreadOrderedSet } from "./ThreadOrderedSet";

export const PostFeed = observer(
  ({
    spoolFilter,
    searchQuery,
  }: {
    spoolFilter?: string | undefined;
    searchQuery?: string | undefined;
  }) => {
    const [currentSort, setCurrentSort] = React.useState(SortTypes.SORT_NEW);
    const [threadSet, setThreadSet] = React.useState<ThreadOrderedSet>(
      new ThreadOrderedSet()
    );
    const [canLoadMore, setCanLoadMore] = React.useState<boolean>(true);
    const [isLoadingThreads, setIsLoadingThreads] =
      React.useState<boolean>(false);

    const sortThreads = async (sortType: SortTypes) => {
      if (sortType !== currentSort) {
        setCurrentSort(sortType);
      }
    };

    const loadMoreThreads = async () => {
      if (threadSet && canLoadMore) {
        setIsLoadingThreads(true);
        const threads = await ThreadAPI.getThreads(
          currentSort,
          searchQuery,
          threadSet.list.length,
          spoolFilter
        );
        if (threads.length > 0) {
          threadSet.addThreads(threads);
          setThreadSet(threadSet);
        } else {
          setCanLoadMore(false);
        }
        setIsLoadingThreads(false);
      }
    };

    React.useEffect(() => {
      (async () => {
        const newThreadSet = new ThreadOrderedSet();
        const threads = await ThreadAPI.getThreads(
          currentSort,
          searchQuery,
          undefined,
          spoolFilter
        );
        newThreadSet.addThreads(threads);
        setThreadSet(newThreadSet);
        setCanLoadMore(true);
      })();
    }, [spoolFilter, searchQuery, currentSort]);

    const threadItems = threadSet.list.map((thread) => {
      return <FeedPostItem thread={thread} key={thread.id} />;
    });

    return (
      <>
        <VStack w="100%">
          <ThreadSorter
            onSortChanged={sortThreads}
            sortType={currentSort}
          ></ThreadSorter>
          {threadItems}
          {canLoadMore ? (
            <Button
              isLoading={isLoadingThreads}
              loadingText="Loading threads..."
              marginTop="5px"
              variant="link"
              onClick={() => {
                loadMoreThreads();
              }}
            >
              Load more threads
            </Button>
          ) : (
            <Button disabled={true} marginTop="5px" variant="link">
              No more threads to view
            </Button>
          )}
        </VStack>
      </>
    );
  }
);
