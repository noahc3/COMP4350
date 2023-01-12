import { Route, Routes } from "react-router-dom";
import Overview from "../../pages/Dashboard/Dashboard";
import Login from "../../pages/Login/Login";
import { HistoryRouter } from "./HistoryRouter";
import { ProtectedRoute } from "./ProtectedRoute";
import { navStore } from "../../stores/NavStore";
import SigninCallback from "../../pages/SigninCallback";
import { observer } from "mobx-react";
import { AccountsPage } from "../../pages/Accounts/Accounts";

interface AuthenticatedRoute {
    path: string;
    element: React.ReactNode;
}

const authenticatedRoutes = [
    {
        path: '/overview',
        element: <Overview />
    },
    {
        path: '/accounts',
        element: <AccountsPage />
    },
    {
        path: '/projections',
        element: <Overview />
    },
    {
        path: '/wizard',
        element: <Overview />
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
                <Route path="/" element={<Login />} />
                <Route path="/signin-callback" element={<SigninCallback />} />
                {authenticatedRouteComponents}
            </Routes>
        </HistoryRouter>
    )
})