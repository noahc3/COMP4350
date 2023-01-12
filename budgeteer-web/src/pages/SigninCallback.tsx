import { observer } from "mobx-react";
import { authStore } from "../stores/AuthStore";

const SigninCallback: React.FC = observer(() => {
    authStore.signinCallback();
    
    return (
        <div>
            <p>Signing in...</p>
        </div>
    )
})

export default SigninCallback;