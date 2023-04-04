import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';

import SubjectsApi from "../../../api/SubjectsApi.js";
import UpdateSubject from "../UpdateSubject.jsx";

import React, { useState } from 'react';

export default function ModalWindowEdit({ show, handleClose, subjectId, onLoadSubjects }) {
    const [subjectName, setSubjectName] = useState(null);
    const [validated, setValidated] = useState(false);
    const [errors, setErrors] = useState();
    const [updatedSubject, setUpdatedSubject] = useState(null);

    const onChangeModel = (updateSubject) => {
        setUpdatedSubject(updateSubject);
    }
    const handleSubmit = (e) => {
        e.preventDefault();
        onEditSubject();
        setValidated(true);
    }

    const onEditSubject = () => {
        if (subjectId !== null) {
            var xhr = new XMLHttpRequest();
            xhr.open("put", SubjectsApi.getPutUrl(subjectId), true);
            xhr.setRequestHeader("Content-Type", "application/json")
            xhr.onload = function () {
                setErrors(null);
                if (xhr.status === 200) {
                    handleClose();
                    onLoadSubjects();
                    setValidated(false);
                }
                else if (xhr.status === 400) {
                    var a = eval('({obj:[' + xhr.response + ']})');
                    if (a.obj[0].errors) {
                        setErrors(a.obj[0].errors);
                    }
                }
            }.bind(this);
            xhr.send(JSON.stringify(updatedSubject));
        }
    }
    const getSubjectById = () => {
        var xhr = new XMLHttpRequest();
        xhr.open("get", SubjectsApi.getSubjectUrl(subjectId), true);
        xhr.setRequestHeader("Content-Type", "application/json")
        xhr.onload = function () {
            if (xhr.status === 200) {
                setUpdatedSubject(JSON.parse(xhr.responseText));
                setSubjectName(JSON.parse(xhr.responseText).name);
            }
        }.bind(this);
        xhr.send();
    }
    const onClose = () => {
        setValidated(false);
        setErrors(null);
        handleClose();
    }

    React.useEffect(() => {
        if (subjectId) {
            getSubjectById();
        }
        else {
            setUpdatedSubject(null);
        }
    }, [subjectId]);

    if (!updatedSubject) {
        return (
            <Modal show={show} onHide={handleClose} backdrop="static" keyboard={false}>
                <Modal.Header closeButton>
                    <Modal.Title as="h5">Изменить предмет</Modal.Title>
                </Modal.Header>
                <Modal.Body className="text-center">
                    Загрузка...
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={handleClose}>Закрыть</Button>
                    <Button variant="primary">Сохранить</Button>
                </Modal.Footer>
            </Modal>
        );
    }
    else {
        return (
            <Modal show={show} onHide={onClose} backdrop="static" keyboard={false}>
                <Form noValidate validated={validated} onSubmit={handleSubmit}>
                    <Modal.Header closeButton>
                        <Modal.Title as="h5">Изменить <b className="text-success">"{subjectName}"</b></Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        <UpdateSubject subject={updatedSubject} errors={errors} onChangeModel={onChangeModel} />
                    </Modal.Body>
                    <Modal.Footer>
                        <Button variant="secondary" onClick={onClose}>Закрыть</Button>
                        <Button type="submit" variant="primary">Сохранить</Button>
                    </Modal.Footer>
                </Form >
            </Modal>
        );
    }
}