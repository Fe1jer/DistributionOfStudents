import User from './User.jsx';

import ModalWindowCreate from './ModalWindows/ModalWindowCreate.jsx';
import ModalWindowEdit from './ModalWindows/ModalWindowEdit.jsx';
import ModalWindowDelete from './ModalWindows/ModalWindowDelete.jsx';

import Table from 'react-bootstrap/Table';
import Button from 'react-bootstrap/Button';

import React, { useState } from 'react';

export default function UsersList({ users, onLoadUsers }) {
    const [selectedUserId, setSelectedUserId] = useState(null);
    const [editShow, setEditShow] = useState(false);
    const [createShow, setCreateShow] = useState(false);
    const [deleteShow, setDeleteShow] = useState(false);

    const handleEditClose = () => {
        setEditShow(false);
        setSelectedUserId(null);
    };
    const handleCreateClose = () => {
        setCreateShow(false);
    };
    const handleDeleteClose = () => {
        setDeleteShow(false);
        setSelectedUserId(null);
    };

    const onClickEditUser = (id) => {
        setSelectedUserId(id);
        setEditShow(true);
    }
    const onClickCreateUser = () => {
        setCreateShow(true);
    }
    const onClickDeleteUser = (id) => {
        setDeleteShow(true);
        setSelectedUserId(id);
    }

    return (
        <div className="card shadow" >
            <ModalWindowCreate show={createShow} handleClose={handleCreateClose} onLoadUsers={onLoadUsers} />
            <ModalWindowEdit show={editShow} handleClose={handleEditClose} userId={selectedUserId} onLoadUsers={onLoadUsers} />
            <ModalWindowDelete show={deleteShow} handleClose={handleDeleteClose} userId={selectedUserId} onLoadUsers={onLoadUsers} />
            <Table responsive className="table mb-0">
                <thead>
                    <tr>
                        <th width="20">
                            №
                        </th>
                        <th>
                            Имя пользователя
                        </th>
                        <th>
                            ФИО
                        </th>
                        <th className="text-center" width="100">
                            <Button variant="empty" className="p-0 text-success" onClick={onClickCreateUser} >
                                <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" className="bi bi-plus-circle-fill suc" viewBox="0 0 16 16">
                                    <path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zM8.5 4.5a.5.5 0 0 0-1 0v3h-3a.5.5 0 0 0 0 1h3v3a.5.5 0 0 0 1 0v-3h3a.5.5 0 0 0 0-1h-3v-3z" />
                                </svg>
                            </Button >
                        </th>
                    </tr>
                </thead>
                <tbody>{
                    users.map((item, index) =>
                        <User num={index + 1} key={item.id + item.name} user={item} onClickEdit={onClickEditUser} onClickDelete={onClickDeleteUser} />
                    )}
                </tbody>
            </Table>
        </div >
    );
}