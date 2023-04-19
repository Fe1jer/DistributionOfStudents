import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';

import FacultiesService from "../../../services/Faculties.service.js";

import React from 'react';

export default function ModalWindowDelete({ show, handleClose, shortName, fullName, onLoadFaculties }) {
    const handleSubmit = (e) => {
        e.preventDefault();
        onDeleteFaculty(shortName);
    }

    const onDeleteFaculty = async (facultyShortName) => {
        await FacultiesService.httpDelete(facultyShortName);
        handleClose();
        onLoadFaculties();
    }

    if (!shortName || !fullName) {
        return (
            <Modal show={show} onHide={handleClose} backdrop="static" keyboard={false}>
                <Modal.Header closeButton>
                    <Modal.Title>Удалить факультет</Modal.Title>
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
                        <Modal.Title>Удалить <b className="text-success">{shortName}</b></Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        <p>
                            Вы уверенны, что хотите удалить факультет?
                            <br />
                            <b className="text-success">{fullName}</b> будет удалён без возможности восстановления.
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
