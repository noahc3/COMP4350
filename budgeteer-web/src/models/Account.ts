export interface IAccount {
    id: string
    name: string
    balance: number
    type: AccountType
    bank: AccountBank
    lastImport: Date
}

export enum AccountBank {
    CIBC_PERSONAL = 'CIBC (Personal)',
    PAYPAL_PERSONAL = 'PayPal (Personal)',
    STACK = 'Stack',
}

export enum AccountType {
    CHEQUING = 'Chequing',
    SAVINGS = 'Savings',
    CREDIT_CARD = 'Credit Card',
}

export function getAccountImage(account: IAccount) {
    switch (account.bank) {
        case AccountBank.CIBC_PERSONAL:
            return '/img/banks/cibc-personal.svg'
        case AccountBank.PAYPAL_PERSONAL:
            return '/img/banks/paypal-personal.svg'
        case AccountBank.STACK:
            return '/img/banks/stack.png'
    }
}