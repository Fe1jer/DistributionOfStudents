import UsersService from "../../../services/Users.service";

import ModalWindowPreloader from "../../ModalWindowPreloader";

import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';

import React, { useState } from 'react';

export default function ModalWindowDelete({ show, handleClose, userId, onLoadUsers }) {
    const [user, setUser] = useState(null);

    const handleSubmit = (e) => {
        e.preventDefault();
        onDeleteUser();
    }

    const onDeleteUser = async () => {
        if (userId !== null) {
            await UsersService.httpDelete(userId);
            handleClose();
            onLoadUsers();
        }
    }
    const getUserById = async () => {
        var data = await UsersService.httpGetById(userId);
        setUser(data);
    }

    React.useEffect(() => {
        if (userId) {
            getUserById();
        }
        else {
            setUser(null);
        }
    }, [userId]);

    if (!user) {
        return <ModalWindowPreloader show={show} handleClose={handleClose} />;
    }
    else {
        return (
            <Modal show={show} onHide={handleClose} backdrop="static" keyboard={false}>
                <Form onSubmit={handleSubmit}>
                    <Modal.Header closeButton>
                        <Modal.Title as="h5">Удалить пользователя</Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        Вы уверенны, что хотите удалить этого пользователя?
                        <br />
                        Пользователь <b className="text-success" id="deleteName">"{user.surname} {user.name}"</b> будет удалён без возможности восстановления.
                    </Modal.Body>
                    <Modal.Footer>
                        <Button variant="secondary" onClick={handleClose}>Закрыть</Button>
                        <Button type="submit" variant="outline-danger">Удалить</Button>
                    </Modal.Footer>
                </Form >
            </Modal>
        );
    }
}
