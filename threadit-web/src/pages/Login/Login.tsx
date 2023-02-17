import { LockIcon } from "@chakra-ui/icons";
import { Alert, AlertIcon, Box, Button, FormControl, FormLabel, Heading, Input, Tab, TabList, TabPanel, TabPanels, Tabs, VStack } from "@chakra-ui/react";
import React from "react";
import { authStore } from "../../stores/AuthStore";
import { navStore } from "../../stores/NavStore";
import { userStore } from "../../stores/UserStore";
import './Login.css'

export default function Login() {
    const [lockInputs, setLockInputs] = React.useState(false);

    const [loginUsername, setLoginUsername] = React.useState('');
    const [loginPassword, setLoginPassword] = React.useState('');
    const [registerEmail, setRegisterEmail] = React.useState('');
    const [registerUsername, setRegisterUsername] = React.useState('');
    const [registerPassword, setRegisterPassword] = React.useState('');

    const [loginError, setLoginError] = React.useState('');
    const [registerError, setRegisterError] = React.useState('');
    const [registerSuccess, setRegisterSuccess] = React.useState('');

    const login = async () => {
        setLockInputs(true);
        try {
            const success = await authStore.login(loginUsername, loginPassword);
            if (!success) {
                setLoginError('Invalid username or password');
            } else {
                navStore.navigateTo("/");
            }
        } finally {
            setLockInputs(false);
        }
    }

    const register = async () => {
        setRegisterError('');
        setRegisterSuccess('');
        setLockInputs(true);
        try {
            const result = await authStore.register(registerEmail, registerUsername, registerPassword);
            if (result !== true && typeof result === 'string') {
                setRegisterError(result);
            }
            else{
                setRegisterSuccess("Successfully registered!");
            }
        } finally {
            setLockInputs(false);
        }
    }

    return (
        <Box className="loginPanel" boxShadow='2xl' p={10} rounded='lg' border="1px solid black" w='xl' h='xl'>
            <Heading as="h1">threadit</Heading>
            <br></br>
            <Tabs>
                <TabList>
                    <Tab>Login</Tab>
                    <Tab>Register</Tab>
                </TabList>

                <TabPanels>
                    <TabPanel>
                        <VStack spacing={4}>
                            {loginError.length > 0 && (
                                <Alert status='error'>
                                    <AlertIcon />
                                    {loginError}
                                </Alert>
                            )}
                            <FormControl isRequired>
                                <FormLabel>Username or Email</FormLabel>
                                <Input disabled={lockInputs} type='email' value={loginUsername} onChange={(e) => setLoginUsername(e.target.value)} />
                            </FormControl>
                            <FormControl isRequired>
                                <FormLabel>Password</FormLabel>
                                <Input onKeyDown={(e) => { if (e.key === 'Enter') login()}} disabled={lockInputs} type='password' value={loginPassword} onChange={(e) => setLoginPassword(e.target.value)} />
                            </FormControl>
                            <Button width={'100%'} colorScheme="purple" leftIcon={<LockIcon />} onClick={() => { login() }}>Login</Button>
                        </VStack>
                    </TabPanel>
                    <TabPanel>
                        <VStack spacing={4}>
                            {registerError.length > 0 && (
                                <Alert status='error'>
                                    <AlertIcon />
                                    {registerError}
                                </Alert>
                            )}
                            {registerSuccess.length > 0 && (
                                    <Alert status='success'>
                                    <AlertIcon />
                                    {registerSuccess}
                                </Alert>
                            )}
                            <FormControl isRequired>
                                <FormLabel>Email</FormLabel>
                                <Input disabled={lockInputs} type='email' value={registerEmail} onChange={(e) => setRegisterEmail(e.target.value)} />
                            </FormControl>
                            <FormControl isRequired>
                                <FormLabel>Username</FormLabel>
                                <Input disabled={lockInputs} type='text' value={registerUsername} onChange={(e) => setRegisterUsername(e.target.value)} />
                            </FormControl>
                            <FormControl isRequired>
                                <FormLabel>Password</FormLabel>
                                <Input onKeyDown={(e) => { if (e.key === 'Enter') register()}} disabled={lockInputs} type='password' value={registerPassword} onChange={(e) => setRegisterPassword(e.target.value)} />
                            </FormControl>
                            <Button w='100%' colorScheme="purple" leftIcon={<LockIcon />} onClick={() => { register() }}>Register</Button>
                        </VStack>
                    </TabPanel>
                </TabPanels>
            </Tabs>
        </Box>
    );
}