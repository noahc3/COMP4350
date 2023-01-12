import { Box, Button, ButtonGroup, Flex, FormControl, FormHelperText, FormLabel, Heading, HStack, Input, Modal, ModalBody, ModalContent, ModalFooter, ModalHeader, ModalOverlay, Radio, RadioGroup, Select, Spacer, Stack } from "@chakra-ui/react"
import { reaction } from "mobx";
import { observer } from "mobx-react"
import React from "react";
import { AccountBank, AccountType, IAccount } from "../../models/Account";
import { userStore } from "../../stores/UserStore";

interface IEditAccountModal {
    isOpen: boolean;
    setIsOpen: (isOpen: boolean) => void;
    accountToEdit?: IAccount;
}

export const EditAccountModal = observer(({ isOpen, setIsOpen, accountToEdit }: IEditAccountModal) => {
    const [isSaving, setIsSaving] = React.useState(false)
    const [isDeleting, setIsDeleting] = React.useState(false)
    const disableInputs = isSaving || isDeleting

    const [nicknameValue, setNicknameValue] = React.useState(accountToEdit?.name ?? "")
    const [typeValue, setTypeValue] = React.useState(accountToEdit?.type ?? AccountType.CHEQUING)
    const [bankValue, setBankValue] = React.useState(accountToEdit?.bank ?? undefined)

    const accountTypes = Object.values(AccountType).map((key) => {
        return (
            <Radio disabled={isSaving} key={key} value={key}>{key}</Radio>
        )
    })

    const accountBanks = Object.values(AccountBank).map((key) => {
        return (
            <option key={key} value={key}>{key}</option>
        )
    })

    React.useEffect(() => {
        setNicknameValue(accountToEdit?.name ?? "")
        setTypeValue(accountToEdit?.type ?? AccountType.CHEQUING)
        setBankValue(accountToEdit?.bank ?? undefined)
    }, [isOpen, accountToEdit])

    const saveAccount = async () => {
        if (bankValue) {
            setIsSaving(true)
            const account: IAccount = {
                id: accountToEdit?.id ?? "0",
                name: nicknameValue,
                type: typeValue,
                bank: bankValue,
                balance: accountToEdit?.balance ?? 0,
                lastImport: accountToEdit?.lastImport ?? new Date()
            }
    
            console.log(account)
    
            if (accountToEdit) {
                await userStore.updateAccount(account)
            } else {
                await userStore.addAccount(account)
            }

            setIsSaving(false)
            setIsOpen(false)
        }
    }

    const deleteAccount = async () => {
        if (accountToEdit) {
            setIsDeleting(true)
            await userStore.deleteAccount(accountToEdit)
            setIsDeleting(false)
            setIsOpen(false);
        }
    }

    return (
        <Modal size={'lg'} isOpen={isOpen} onClose={() => setIsOpen(false)} closeOnOverlayClick={false}>
            <ModalOverlay />
            <ModalContent>
                <ModalHeader>{accountToEdit ? "Edit Account" : "Add Account"}</ModalHeader>
                <ModalBody>
                    <Stack spacing={4}>
                        <FormControl>
                            <FormLabel as='legend'>Account Nickname</FormLabel>
                            <Input disabled={disableInputs} value={nicknameValue} onChange={(e) => {setNicknameValue(e.target.value)}} type="text" placeholder="Account Nickname" />
                        </FormControl>
                        <FormControl as='fieldset'>
                            <FormLabel as='legend'>Account Type</FormLabel>
                            <RadioGroup value={typeValue} onChange={(val) => {setTypeValue(val as AccountType)}}>
                                <HStack spacing='24px'>
                                    {accountTypes}
                                </HStack>
                            </RadioGroup>
                        </FormControl>
                        <FormControl>
                            <FormLabel as='legend'>Bank</FormLabel>
                            <Select disabled={disableInputs} placeholder="Select bank" value={bankValue} onChange={(e) => {setBankValue(e.target.value as AccountBank)}}>
                                {accountBanks}
                            </Select>
                        </FormControl>
                    </Stack>
                </ModalBody>
                <ModalFooter>
                    <Flex flexDirection='row' w='100%'>
                        {accountToEdit && 
                        <ButtonGroup>
                            <Button isLoading={isDeleting} loadingText='Deleting' disabled={disableInputs} colorScheme={'red'} variant='outline' onClick={() => {deleteAccount()}}>Delete</Button>
                        </ButtonGroup>}
                        <Spacer/>
                        <ButtonGroup>
                            <Button disabled={disableInputs} onClick={() => { setIsOpen(false) }}>Cancel</Button>
                            <Button isLoading={isSaving} loadingText='Saving' disabled={disableInputs} colorScheme={'purple'} onClick={() => {saveAccount()}}>Save</Button>
                        </ButtonGroup>
                    </Flex>
                </ModalFooter>
            </ModalContent>
        </Modal>
    )
})