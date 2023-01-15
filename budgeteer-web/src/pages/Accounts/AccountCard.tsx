import { Box, Button, ButtonGroup, Divider, Flex, Icon, IconButton, Image, Text } from "@chakra-ui/react";
import { observer } from "mobx-react"
import { AccountType, getAccountImage, IAccount } from "../../models/Account";
import { IoMdCloudUpload, IoMdSettings } from "react-icons/io";
import { MdSavings } from "react-icons/md";
import { HiCreditCard } from "react-icons/hi";
import { FaMoneyBill } from "react-icons/fa";
import './AccountCard.scss'

interface IAccountCard {
    account: IAccount;
    editAccount: () => void;
}

function getAccountTypeIcon(account: IAccount) {
    switch (account.type) {
        case AccountType.CHEQUING:
            return FaMoneyBill
        case AccountType.SAVINGS:
            return MdSavings
        case AccountType.CREDIT_CARD:
            return HiCreditCard
    }
}

export const AccountCard = observer(({account, editAccount}: IAccountCard) => {
    const {name, balance, lastImport} = account
    const imgSrc = getAccountImage(account)
    return (
        <Box borderWidth='1px' borderRadius='lg' p={6} width='100%' className='account-card'>
            <Flex direction={"column"}>
                <Image src={imgSrc} alignSelf='center' width='3xs' objectFit='scale-down'/>
                <Flex direction={'row'} alignItems='center'>
                    <Icon as={getAccountTypeIcon(account)} mr={1}/>
                    <Text fontWeight='semibold' as='h4' lineHeight='tight'>
                        {name}
                    </Text>
                </Flex>
                <Text fontWeight='bold' lineHeight='tight' fontSize={'2xl'}>
                    ${balance / 100.0}
                </Text>
                <Box>
                    <Text color={'gray.500'} fontWeight='semibold' fontSize="xs" lineHeight='tight'>Last Import: {lastImport.toLocaleString()} </Text>
                </Box>
                <Divider />
                <ButtonGroup isAttached size={'sm'}>
                    <Button w={"100%"} leftIcon={<Icon as={IoMdCloudUpload}/>} colorScheme={'purple'}>Import Transactions</Button>
                    <IconButton onClick={() => {editAccount()}} aria-label="edit" icon={<Icon as={IoMdSettings}/>} colorScheme={'pink'}></IconButton>
                </ButtonGroup>
            </Flex>
        </Box>
    )
})