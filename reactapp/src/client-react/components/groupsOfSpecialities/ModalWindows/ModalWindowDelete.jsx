import GroupsOfSpecialitiesService from '../../../services/GroupsOfSpecialities.service.js';

import Button from 'react-bootstrap/Button';
import Form from 'react-bootstrap/Form';
import Modal from 'react-bootstrap/Modal';

import React, { useState } from 'react';
import { useParams } from 'react-router-dom';

export default function ModalWindowDelete({ show, handleClose, onLoadGroups, groupId, year }) {
    const params = useParams();
    const shortName = params.shortName;
    const [group, setGroup] = useState(null);

    const handleSubmit = (e) => {
        e.preventDefault();
        onDeleteGroup();
    }
    const loadGroupOfSpecialities = async () => {
        const groupData = await GroupsOfSpecialitiesService.httpGetById(groupId);
        setGroup(groupData);
    }

    const onDeleteGroup = async () => {
        if (year !== "0") {
            await GroupsOfSpecialitiesService.httpDelete(shortName, groupId);
            onLoadGroups(year);
            handleClose();
        }
    }

    React.useEffect(() => {
        if (groupId) {
            loadGroupOfSpecialities();
        }
        else {
            setGroup(null);
        }
    }, [groupId])

    if (!groupId || group == null) {
        return (
            <Modal show={show} onHide={handleClose} backdrop="static" keyboard={false}>
                <Modal.Header closeButton>
                    <Modal.Title>Удалить группу</Modal.Title>
                </Modal.Header>
                <Modal.Body className="text-center">
                    Загрузка...
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={handleClose}>Закрыть</Button>
                    <Button variant="outline-danger">Удалить</Button>
                </Modal.Footer>
            </Modal>
        );
    }
    else {
        return (
            <Modal show={show} onHide={handleClose} backdrop="static" keyboard={false}>
                <Form onSubmit={handleSubmit}>
                    <Modal.Header closeButton>
                        <Modal.Title>Удалить группу</Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        <p>
                            Вы уверенны, что хотите удалить эту группу?
                            <br />
                            Группа <b className="text-success">{group.name}</b> будет удалёна без возможности восстановления.
                        </p>
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
