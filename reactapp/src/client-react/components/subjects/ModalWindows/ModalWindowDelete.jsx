import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';

import SubjectsService from "../../../services/Subjects.service.js";

import React, { useState } from 'react';

export default function ModalWindowDelete({ show, handleClose, subjectId, onLoadSubjects }) {
    const [subject, setSubject] = useState(null);

    const handleSubmit = (e) => {
        e.preventDefault();
        onDeleteSubject();
    }

    const onDeleteSubject = async () => {
        if (subjectId !== null) {
            await SubjectsService.httpDelete(subjectId);
            handleClose();
            onLoadSubjects();
        }
    }
    const getSubjectById = async () => {
        var subjectData = await SubjectsService.httpGetById(subjectId);
        setSubject(subjectData);
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
