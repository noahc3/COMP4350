import { Button, Center, Flex, Icon, Input, InputGroup, InputLeftElement, SimpleGrid, Spacer, Spinner, WrapItem } from "@chakra-ui/react";
import { observer } from "mobx-react"
import { PageLayout } from "../../containers/PageLayout/PageLayout";
import { IAccount } from "../../models/Account";
import { AccountCard } from "./AccountCard";
import { IoMdAddCircleOutline } from "react-icons/io";
import { IoSearch } from "react-icons/io5";
import React from "react";
import { EditAccountModal } from "./EditAccountModal";
import { accountStore } from "../../stores/AccountStore";

export const AccountsPage = observer(() => {
    const accounts = accountStore.accounts
    const [showEditModal, setShowEditModal] = React.useState(false)
    const [accountToEdit, setAccountToEdit] = React.useState<IAccount | undefined>(undefined)
    const setShowEditModalWrapper = (showModal: boolean) => {
        setShowEditModal(showModal)
        if (!showModal) setAccountToEdit(undefined)
    }

    React.useEffect(() => {accountStore.getAccounts()}, [])

    const cards = accounts?.map((account) => {
        return (
            <WrapItem key={account.id}>
                <AccountCard 
                    account={account}
                    editAccount={() => {
                        setAccountToEdit(account)
                        setShowEditModalWrapper(true)
                    }} 
                />
            </WrapItem>
        )
    }) ?? undefined

    return (
        <>
            <EditAccountModal accountToEdit={accountToEdit} isOpen={showEditModal} setIsOpen={(isOpen) => {setShowEditModalWrapper(isOpen)}} />
            <PageLayout title="Accounts">
                {cards ? (
                    <>
                        <Flex flexDirection='row'>
                            <InputGroup mr={2}>
                                <InputLeftElement>
                                    <Icon as={IoSearch}/>
                                </InputLeftElement>
                                <Input placeholder="Search Accounts" />
                            </InputGroup>
                            <Spacer/>
                            <Button leftIcon={<Icon as={IoMdAddCircleOutline}/>} colorScheme={'purple'} onClick={() => {setShowEditModalWrapper(true)}}>Add Account</Button>
                        </Flex>
                        <br/>
                        <SimpleGrid columns={{ base: 1, sm: 1, md: 1, lg: 2, xl: 3, '2xl': 4 }} spacing={5}>
                                {cards}
                        </SimpleGrid>
                    </>
                    ) : (
                        <Center w='100%'>
                            <Spinner thickness="4px" size={'xl'}/>
                        </Center>
                    )
                }
            </PageLayout>
        </>
    )
})