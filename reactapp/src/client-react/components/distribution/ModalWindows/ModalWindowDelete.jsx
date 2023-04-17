import GroupsOfSpecialitiesApi from "../../../api/GroupsOfSpecialitiesApi.js";
import DistributionApi from "../../../api/DistributionApi.js";

import Button from 'react-bootstrap/Button';
import Form from 'react-bootstrap/Form';
import Modal from 'react-bootstrap/Modal';

import React, { useState } from 'react';

export default function ModalWindowDelete({ show, handleClose, onLoadGroup, groupId, facultyShortName }) {
    const [group, setGroup] = useState(null);

    const handleSubmit = (e) => {
        e.preventDefault();
        onDeleteDistribution();
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

    const onDeleteDistribution = () => {
        var xhr = new XMLHttpRequest();
        xhr.open("delete", DistributionApi.getDeleteUrl(facultyShortName, groupId), true);
        xhr.setRequestHeader("Content-Type", "application/json")
        xhr.onload = function () {
            if (xhr.status === 200) {
                onLoadGroup();
                handleClose();
            }
        }.bind(this);
        xhr.send();
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
                    <Modal.Title>Расформировать специальности</Modal.Title>
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
                        <Modal.Title>Расформировать специальности</Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        <p>
                            Вы уверенны, что хотите расформировать специальности?
                            <br />
                            Специальности в группе <b className="text-success">"{group.name}"</b> будут расформированы без возможности восстановления.
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
