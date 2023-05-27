import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';

import React, { useState } from 'react';

export default function ModalWindowConfirm({ show, handleClose, onConfirmDistribution }) {
    const [notify, setNotify] = useState(true);
    const notifyChange = (e) => {
        const { target } = e;
        const value = target.checked;
        console.log(value);
        setNotify(value);
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        onConfirmDistribution(notify);
    }

    return (
        <Modal show={show} onHide={handleClose}>
            <Form onSubmit={handleSubmit}>
                <Modal.Header closeButton>
                    <Modal.Title>Подтвердить зачисление</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <p>Это окончательный этап для подтверждения студентов к зачислению.</p>
                    <Form.Group>
                        <Form.Check name="notify"
                            type="checkbox" checked={notify}
                            onChange={notifyChange}
                            label="Оповестить абитуриентов" />
                    </Form.Group>
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={handleClose}>Закрыть</Button>
                    <Button type="submit" variant="success">Сохранить</Button>
                </Modal.Footer>
            </Form >
        </Modal>
    );
}
