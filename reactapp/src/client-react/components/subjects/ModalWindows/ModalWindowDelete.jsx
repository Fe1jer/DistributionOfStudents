import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';

import SubjectsApi from "../../../api/SubjectsApi.js";

import React, { useState } from 'react';

export default function ModalWindowDelete({ show, handleClose, subjectId, onLoadSubjects }) {
    const [subject, setSubject] = useState(null);

    const handleSubmit = (e) => {
        e.preventDefault();
        onDeleteSubject();
    }

    const onDeleteSubject = () => {
        if (subjectId !== null) {
            var xhr = new XMLHttpRequest();
            xhr.open("delete", SubjectsApi.getDeleteUrl(subjectId), true);
            xhr.setRequestHeader("Content-Type", "application/json")
            xhr.onload = function () {
                if (xhr.status === 200) {
                   handleClose();
                    onLoadSubjects();
                }
            }.bind(this);
            xhr.send();
        }
    }
    const getSubjectById = () => {
        var xhr = new XMLHttpRequest();
        xhr.open("get", SubjectsApi.getSubjectUrl(subjectId), true);
        xhr.setRequestHeader("Content-Type", "application/json")
        xhr.onload = function () {
            if (xhr.status === 200) {
                setSubject(JSON.parse(xhr.responseText));
            }
        }.bind(this);
        xhr.send();
    }

    React.useEffect(() => {
        if (subjectId) {
            getSubjectById();
        }
        else {
            setSubject(null);
        }
    }, [subjectId]);

    if (!subject) {
        return (
            <Modal show={show} onHide={handleClose} backdrop="static" keyboard={false}>
                <Modal.Header closeButton>
                    <Modal.Title as="h5">Удалить предмет</Modal.Title>
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
                        <Modal.Title as="h5">Удалить предмет</Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        Вы уверенны, что хотите удалить этот предмет?
                        <br />
                        Предмет <b className="text-success" id="deleteName">"{subject.name}"</b> будет удалён без возможности восстановления.
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
