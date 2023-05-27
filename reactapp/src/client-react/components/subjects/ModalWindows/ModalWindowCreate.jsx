import { SubjectValidationSchema } from '../../../validations/Subject.validation';
import SubjectsService from "../../../services/Subjects.service.js";
import UpdateSubject from "../UpdateSubject.jsx";

import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';

import * as formik from 'formik';

import React from 'react';

export default function ModalWindowCreate({ show, handleClose, onLoadSubjects }) {
    const { Formik } = formik;
    const defaultSubject = {
        id: 0,
        name: null
    }

    const handleSubmit = (values) => {
        onCreateSubject(values);
    }

    const onCreateSubject = async (updatedSubject) => {
        await SubjectsService.httpPost(updatedSubject);
        handleClose();
        onLoadSubjects();
    }

    return (
        <Modal show={show} onHide={handleClose} backdrop="static" keyboard={false}>
            <Formik
                validationSchema={SubjectValidationSchema}
                onSubmit={handleSubmit}
                initialValues={defaultSubject}>
                {({ handleSubmit, handleChange, values, touched, errors }) => (
                    <Form noValidate onSubmit={handleSubmit}>
                        <Modal.Header closeButton>
                            <Modal.Title as="h5">Добавить предмет</Modal.Title>
                        </Modal.Header>
                        <Modal.Body>
                            <UpdateSubject subject={values} errors={errors} onChangeModel={handleChange} />
                        </Modal.Body>
                        <Modal.Footer>
                            <Button variant="secondary" onClick={handleClose}>Закрыть</Button>
                            <Button type="submit" variant="success">Сохранить</Button>
                        </Modal.Footer>
                    </Form >
                )}
            </Formik>
        </Modal>
    );
}