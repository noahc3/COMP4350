import { Route, Routes } from "react-router-dom";
import { HistoryRouter } from "./HistoryRouter";
import { ProtectedRoute } from "./ProtectedRoute";
import { navStore } from "../../stores/NavStore";
import { observer } from "mobx-react";
import UserSettings from "../../pages/UserSettings/UserSettings";
import Home from "../../pages/Home/Home";
import Login from "../../pages/Login/Login";
import Thread from "../../pages/CreateThread/CreateThread";


interface AuthenticatedRoute {
    path: string;
    element: React.ReactNode;
}

const authenticatedRoutes = [
    {
        path: '/settings',
        element: <UserSettings />
    }
]

export const Router: React.FC = observer(() => {
    const history = navStore.history;
    const authenticatedRouteComponents = authenticatedRoutes.map((route: AuthenticatedRoute) => {
        return (
            <Route key={route.path} path={route.path} element={<ProtectedRoute element={route.element} />} />
        )
    })

    return (
        <HistoryRouter history={history}>
            <Routes>
                <Route path="/" element={<Home />} />
                <Route path="/login" element={<Login />} />
                <Route path="/thread" element={<Thread />} />
                {authenticatedRouteComponents}
            </Routes>
        </HistoryRouter>
    )
})