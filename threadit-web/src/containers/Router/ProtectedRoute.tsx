import { authStore } from "../../stores/AuthStore";
import { observer } from "mobx-react"

interface IProtectedRoute {
    element?: React.ReactNode;
}

export const ProtectedRoute = observer(({element}: IProtectedRoute) => {

    return authStore.isAuthenticated ? (
        <>
            {element}
        </>
    ) : (
        <>
            <p>You are not logged in</p>
        </>
    )
    
})