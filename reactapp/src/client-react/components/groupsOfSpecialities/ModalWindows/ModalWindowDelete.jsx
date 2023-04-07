import GroupsOfSpecialitiesApi from "../../../api/GroupsOfSpecialitiesApi.js";

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
    const loadGroupOfSpecialities = () => {
        var xhr = new XMLHttpRequest();
        xhr.open("get", GroupsOfSpecialitiesApi.getGroupUrl(groupId), true);
        xhr.onload = function () {
            var data = JSON.parse(xhr.responseText);
            setGroup(data);
        }.bind(this);
        xhr.send();
    }

    const onDeleteGroup = () => {
        if (year !== "0") {
            var xhr = new XMLHttpRequest();
            xhr.open("delete", GroupsOfSpecialitiesApi.getDeleteUrl(shortName, groupId), true);
            xhr.setRequestHeader("Content-Type", "application/json")
            xhr.onload = function () {
                if (xhr.status === 200) {
                    onLoadGroups(year);
                    handleClose();
                }
            }.bind(this);
            xhr.send();
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
