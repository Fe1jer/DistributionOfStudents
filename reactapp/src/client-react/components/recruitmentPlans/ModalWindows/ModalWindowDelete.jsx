import ModalWindowPreloader from "../../ModalWindowPreloader";

import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';

import React from 'react';

export default function ModalWindowDelete({ show, handleClose, onDeletePlans, facultyShortName, year }) {
    const handleSubmit = (e) => {
        e.preventDefault();
        onDeletePlans(facultyShortName, year);
    }

    if (!facultyShortName || !year) {
        return <ModalWindowPreloader show={show} handleClose={handleClose} />;
    }
    else {
        return (
            <Modal show={show} onHide={handleClose} backdrop="static" keyboard={false}>
                <Form onSubmit={handleSubmit}>
                    <Modal.Header closeButton>
                        <Modal.Title as="h5">Удалить план за <b className="text-success">{year} год</b></Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        <p>
                            Вы уверенны, что хотите удалить план приёма?
                            <br />
                            План приёма за <b className="text-success">{year} год</b> на <b className="text-success">{facultyShortName}</b> будет удалён без возможности восстановления.
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
