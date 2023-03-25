import { Box, Divider, Heading } from "@chakra-ui/react";
import { observer } from "mobx-react"
import './PageLayout.scss'

interface IPageLayout {
    title: string;
    children?: React.ReactNode;
}

export const PageLayout = observer(({title, children}: IPageLayout) => {
    return (
        <Box className="pagelayout" bgColor={"blackAlpha.200"} h="100%" overflowY="scroll" scrollBehavior={"smooth"} paddingBottom={'10rem'}>
            <Heading>{title}</Heading>
            <Divider/>
            {children}
        </Box>
    )
})