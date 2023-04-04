import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';

import SubjectsApi from "../../../api/SpecialitiesApi.js";

import React, { useState } from 'react';

export default function ModalWindowDelete({ show, handleClose, specialityId, onLoadSpecialities }) {
    const [speciality, setSpeciality] = useState(null);

    const handleSubmit = (e) => {
        e.preventDefault();
        onDeleteSpeciality();
    }

    const onDeleteSpeciality = () => {
        if (specialityId !== null) {
            var xhr = new XMLHttpRequest();
            xhr.open("delete", SubjectsApi.getDeleteUrl(specialityId), true);
            xhr.setRequestHeader("Content-Type", "application/json")
            xhr.onload = function () {
                if (xhr.status === 200) {
                    handleClose();
                    onLoadSpecialities();
                }
            }.bind(this);
            xhr.send();
        }
    }
    const getSpetyalityById = () => {
        var xhr = new XMLHttpRequest();
        xhr.open("get", SubjectsApi.getSpecialityUrl(specialityId), true);
        xhr.setRequestHeader("Content-Type", "application/json")
        xhr.onload = function () {
            if (xhr.status === 200) {
                setSpeciality(JSON.parse(xhr.responseText));
            }
        }.bind(this);
        xhr.send();
    }

    React.useEffect(() => {
        if (specialityId) {
            getSpetyalityById();
        }
        else {
            setSpeciality(null);
        }
    }, [specialityId]);

    if (!speciality) {
        return (
            <Modal show={show} onHide={handleClose} backdrop="static" keyboard={false}>
                <Modal.Header closeButton>
                    <Modal.Title>Удалить специальность</Modal.Title>
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
                        <Modal.Title>Удалить специальность</Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        Вы уверенны, что хотите удалить специальность?
                        <br />
                        Специальность <b className="text-success">"{speciality.fullName}"</b> будет удалена без возможности восстановления.
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
