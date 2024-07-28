import UsersList from "./UsersList.jsx";
import TablePreloader from "../TablePreloader.jsx";

import UsersService from "../../services/Users.service.js";

import React from 'react';
import useDocumentTitle from "../useDocumentTitle.jsx";

export default function UsersPage() {
    useDocumentTitle("Пользователи");

    const [users, setUsers] = React.useState([]);
    const [loading, setLoading] = React.useState(true);
    // загрузка данных
    const loadData = async () => {
        try {
            setLoading(true);
            const usersData = await UsersService.httpGet();
            setUsers(usersData);
        } catch (err) {
            console.error(err.message);
        } finally {
            setLoading(false);
        }
    }

    React.useEffect(() => {
        loadData();
    }, [])

    if (loading) {
        return <React.Suspense>
            <h1 className="text-center placeholder-glow"><span className="placeholder w-25"></span></h1>
            <hr />
            <TablePreloader />
        </React.Suspense>
    }
    else {
        return (
            <React.Suspense>
                <h1 className="text-center">Пользователи</h1>
                <hr />
                <div className="ps-lg-4 pe-lg-4 position-relative">
                    <UsersList users={users} onLoadUsers={loadData} />
                </div>
            </React.Suspense>
        );
    }
}
