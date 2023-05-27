import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';

import UpdateFaculty from "../UpdateFaculty.jsx";

import FacultiesService from "../../../services/Faculties.service";
import { FacultyValidationSchema } from "../../../validations/Faculty.validation";

import { Formik } from 'formik';

import React, { useState } from 'react';

export default function ModalWindowCreate({ show, handleClose, onLoadFaculties }) {
    const defaultFaculty = {
        id: 0,
        fullName: "",
        shortName: "",
        img: "\\img\\Faculties\\Default.jpg"
    }
    const [modelErrors, setModelErrors] = useState(null);

    const handleSubmit = (values) => {
        onCreateFaculty(values);
    }

    const onCreateFaculty = async (values) => {
        try {
            const form = new FormData();
            form.append("Faculty.FullName", values.fullName);
            form.append("Faculty.ShortName", values.shortName);
            form.append("Faculty.Img", values.img);
            form.append("Img", values.fileImg);
            setModelErrors(null);

            await FacultiesService.httpPost(form);
            onLoadFaculties();
            handleClose();
        }
        catch (err) {
            setModelErrors(JSON.parse(`{${err.message.replace('Error', '"Error"')}}`).Error);
        }
    }

    return (
        <Modal show={show} onHide={handleClose} backdrop="static" keyboard={false}>
            <Formik
                validationSchema={FacultyValidationSchema}
                onSubmit={handleSubmit}
                initialValues={{ ...defaultFaculty, fileImg: null }}>
                {({ handleSubmit, handleChange, values, touched, errors }) => (
                    <Form noValidate onSubmit={handleSubmit}>
                        <Modal.Header closeButton>
                            <Modal.Title>Создать факультет</Modal.Title>
                        </Modal.Header>
                        <Modal.Body>
                            <UpdateFaculty form={values} errors={errors} onChangeModel={handleChange} modelErrors={modelErrors} />
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
