import { authStore } from "../../stores/AuthStore";
import { observer } from "mobx-react"

interface IAuthGate {
    children?: React.ReactNode;
}

export const AuthGate = observer(({children}: IAuthGate) => {
    const needsAuthCheck = !authStore.isAuthenticated && !authStore.userNeedsAuthentication;

    if (needsAuthCheck) {
        authStore.checkAuthentication();
    }

    return (
        <>
            {children}
        </>
    )
})