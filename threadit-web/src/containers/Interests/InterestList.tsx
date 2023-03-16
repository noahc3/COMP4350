import { VStack } from "@chakra-ui/layout";
import { observer } from "mobx-react";
import { IInterest } from "../../models/Interest";
import { InterestItem } from "./InterestItem";

export const InterestList = observer(({interests}: { interests:  string[]}) => {
    const interestItems = interests.map((interest) => {
        return <InterestItem interest={interest}/>
    });
    return (
        <>
            <VStack w="100%">
                {interestItems}
            </VStack>
        </>
    );
});