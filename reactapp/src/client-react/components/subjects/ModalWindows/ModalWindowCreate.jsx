import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';

import SubjectsApi from "../../../api/SubjectsApi.js";
import UpdateSubject from "../UpdateSubject.jsx";

import React, { useState } from 'react';

export default function ModalWindowCreate({ show, handleClose, onLoadSubjects }) {
    const defaultSubject = {
        id: 0,
        name: null
    }

    const [updatedSubject, setUpdatedSubject] = useState(defaultSubject);
    const [validated, setValidated] = useState(false);
    const [errors, setErrors] = useState();

    const onChangeModel = (updateSpeciality) => {
        setUpdatedSubject(updateSpeciality);
    }
    const handleSubmit = (e) => {
        e.preventDefault();
        onCreateSubject();
        setValidated(true);
    }

    const onCreateSubject = () => {
        var xhr = new XMLHttpRequest();
        xhr.open("post", SubjectsApi.getPostUrl(), true);
        xhr.setRequestHeader("Content-Type", "application/json")
        xhr.onload = function () {
            if (xhr.status === 200) {
                handleClose();
                onLoadSubjects();
                setValidated(false);
                setUpdatedSubject(defaultSubject);
            }
            else if (xhr.status === 400) {
                console.log(xhr.response);
                var a = eval('({obj:[' + xhr.response + ']})');
                if (a.obj[0].errors) {
                    setErrors(a.obj[0].errors);
                }
            }
        }.bind(this);
        xhr.send(JSON.stringify(updatedSubject));
    }

    return (
        <Modal show={show} onHide={handleClose} backdrop="static" keyboard={false}>
            <Form noValidate validated={validated} onSubmit={handleSubmit}>
                <Modal.Header closeButton>
                    <Modal.Title as="h5">Добавить предмет</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <UpdateSubject subject={updatedSubject} errors={errors} onChangeModel={onChangeModel} />
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={handleClose}>Закрыть</Button>
                    <Button type="submit" variant="primary">Сохранить</Button>
                </Modal.Footer>
            </Form >
        </Modal>
    );
}