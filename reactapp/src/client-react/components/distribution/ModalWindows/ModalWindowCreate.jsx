import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';

import React from 'react';

export default function ModalWindowCreate({ show, handleClose, onCreateDistribution }) {
    const handleSubmit = (e) => {
        e.preventDefault();
        onCreateDistribution();
    }

    return (
        <Modal show={show} onHide={handleClose}>
            <Form onSubmit={handleSubmit}>
                <Modal.Header closeButton>
                    <Modal.Title>Подтвердить списки</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <p>Перед тем как подтвердить списки студентов, проверьте количество выбранных абитуриентов.</p>
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={handleClose}>Закрыть</Button>
                    <Button type="submit" variant="success">Сохранить</Button>
                </Modal.Footer>
            </Form >
        </Modal>
    );

}
