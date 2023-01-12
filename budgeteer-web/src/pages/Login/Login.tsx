import { LockIcon } from "@chakra-ui/icons";
import { Button, Heading } from "@chakra-ui/react";
import React from "react";
import { authStore } from "../../stores/AuthStore";
import './Login.css'

export default class Login extends React.Component {
    login() {
        authStore.login();
    }

    render() {
        return (
            <div className="loginPanel">
                <Heading as="h1">Power User's Budgeteer</Heading>
                <br></br>
                <Button colorScheme="purple" leftIcon={<LockIcon/>} onClick={() => {this.login()}}>Login with Authentik</Button>
            </div>
        );
    }
}