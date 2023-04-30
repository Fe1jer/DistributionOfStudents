import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';

import { SubjectValidationSchema } from '../../../validations/Subject.validation';
import SubjectsService from "../../../services/Subjects.service.js";
import UpdateSubject from "../UpdateSubject.jsx";

import * as formik from 'formik';

import React, { useState } from 'react';

export default function ModalWindowEdit({ show, handleClose, subjectId, onLoadSubjects }) {
    const { Formik } = formik;
    const [subjectName, setSubjectName] = useState(null);
    const [validated, setValidated] = useState(false);
    const [updatedSubject, setUpdatedSubject] = useState(null);

    const handleSubmit = (values) => {
        onEditSubject(values);
        setValidated(true);
    }

    const onEditSubject = async (updatedSubject) => {
        if (subjectId !== null) {
            await SubjectsService.httpPut(subjectId, updatedSubject);
            handleClose();
            onLoadSubjects();
            setValidated(false);
        }
    }
    const getSubjectById = async () => {
        var subjectData = await SubjectsService.httpGetById(subjectId);
        setUpdatedSubject(subjectData);
        setSubjectName(subjectData.name);
    }
    const onClose = () => {
        setValidated(false);
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
                <Formik
                    validationSchema={SubjectValidationSchema}
                    onSubmit={handleSubmit}
                    initialValues={updatedSubject}>
                    {({ handleSubmit, handleChange, values, touched, errors }) => (
                        <Form noValidate validated={validated} onSubmit={handleSubmit}>
                            <Modal.Header closeButton>
                                <Modal.Title as="h5">Изменить <b className="text-success">"{subjectName}"</b></Modal.Title>
                            </Modal.Header>
                            <Modal.Body>
                                <UpdateSubject subject={values} errors={errors} onChangeModel={handleChange} />
                            </Modal.Body>
                            <Modal.Footer>
                                <Button variant="secondary" onClick={onClose}>Закрыть</Button>
                                <Button type="submit" variant="primary">Сохранить</Button>
                            </Modal.Footer>
                        </Form >
                    )}
                </Formik>
            </Modal>
        );
    }
}