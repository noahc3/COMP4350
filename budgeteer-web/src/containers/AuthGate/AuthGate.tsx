import { authStore } from "../../stores/AuthStore";
import { observer } from "mobx-react"
import Login from "../../pages/Login/Login";

interface IAuthGate {
    children?: React.ReactNode;
}

export const AuthGate = observer(({children}: IAuthGate) => {
    const needsAuthCheck = !authStore.isAuthenticated && !authStore.userNeedsAuthentication;

    if (needsAuthCheck) {
        authStore.checkAuthentication();
    }

    if (needsAuthCheck) {
        return (
            <>
                <p>Checking authentication...</p>
            </>
        )
    } else if (authStore.isAuthenticated) {
        return (
            <>
                {children}
            </>
        )
    } else {
        return (
            <Login/>
        )
    }
})