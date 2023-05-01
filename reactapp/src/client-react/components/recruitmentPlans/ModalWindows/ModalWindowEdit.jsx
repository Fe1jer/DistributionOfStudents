import ModalWindowPreloader from "../../ModalWindowPreloader";

import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';

import React from 'react';

export default function EditModalWindow({ show, handleClose, onEditPlans, facultyShortName, year }) {
    const handleSubmit = (e) => {
        e.preventDefault();
        onEditPlans();
    }

    if (!facultyShortName || !year) {
        return <ModalWindowPreloader show={show} handleClose={handleClose} />;
    }
    else {
        return (
            <Modal show={show} onHide={handleClose} backdrop="static" keyboard={false}>
                <Form onSubmit={handleSubmit}>
                    <Modal.Header closeButton>
                        <Modal.Title>Сохранить изменения</Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        <p>
                            Перед тем как изменить план приёма на <b className="text-success">{year} год</b>, проверьте заполнение всех ячеек плана приёма.
                        </p>
                    </Modal.Body>
                    <Modal.Footer>
                        <Button variant="secondary" onClick={handleClose}>Закрыть</Button>
                        <Button type="submit" variant="success">Сохранить</Button>
                    </Modal.Footer>
                </Form >
            </Modal>
        );
    }
}
