import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';

import React from 'react';

export default function ModalWindowConfirm({ show, handleClose, onConfirmDistribution }) {
    const handleSubmit = (e) => {
        e.preventDefault();
        onConfirmDistribution();
    }

    return (
        <Modal show={show} onHide={handleClose}>
            <Form onSubmit={handleSubmit}>
                <Modal.Header closeButton>
                    <Modal.Title>Подтвердить зачисление</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <p>Это окончательный этап для подтверждения студентов к зачислению.</p>
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={handleClose}>Закрыть</Button>
                    <Button type="submit" variant="primary">Сохранить</Button>
                </Modal.Footer>
            </Form >
        </Modal>
    );
}
