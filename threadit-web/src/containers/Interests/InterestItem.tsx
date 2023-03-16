import { observer } from "mobx-react";
import { IUserSettings } from "../../models/UserSettings";

export const InterestItem = observer(({userSettings}: {userSettings: IUserSettings}) => {
    return (
        <>
            <div>InterestItem</div>
        </>
    );
})
