import { Route, Routes } from "react-router-dom";
import { HistoryRouter } from "./HistoryRouter";
import { navStore } from "../../stores/NavStore";
import { observer } from "mobx-react";
import { Home } from "../../pages/Home/Home";
import Login from "../../pages/Login/Login";
import PostThread from "../../pages/PostThread/PostThread";
import PostSpool from "../../pages/PostSpool/PostSpool";
import { Thread } from "../../pages/Thread/Thread";
import { Spool } from "../../pages/Spool/Spool";
import { ManageSpool } from "../../pages/Spool/ManageSpool";

export const Router: React.FC = observer(() => {
    const history = navStore.history;

    return (
        <HistoryRouter history={history}>
            <Routes>
                <Route path="/" element={<Home />} />
                <Route path="/login" element={<Login />} />
                <Route path="/s/:spoolName/createthread" element={<PostThread />} />
                <Route path="/createSpool" element={<PostSpool />} />
                <Route path="/s/:spoolName/post/:threadId" element={<Thread />} />
                <Route path="/s/:id" element={<Spool />} />
                <Route path="/s/:name/manage" element={<ManageSpool />} />
            </Routes>
        </HistoryRouter>
    )
})