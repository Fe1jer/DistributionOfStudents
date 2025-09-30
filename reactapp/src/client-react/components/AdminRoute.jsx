import { Navigate } from 'react-router-dom';
import { useSelector, useDispatch } from 'react-redux';

import { authActions } from '../../_store';
import { history } from '../../_helpers';

export default function AdminRoute({ children }) {
    const { user: authUser, accessToken: token } = useSelector(x => x.auth);
    const dispatch = useDispatch();
    const logout = () => dispatch(authActions.logout());

    if (!authUser || !token || authUser.role !== "admin") {
        // not logged in so redirect to login page with the return url
        logout();
        return <Navigate to="/login" state={{ from: history.location }} />
    }

    // authorized so return child components
    return children;
}