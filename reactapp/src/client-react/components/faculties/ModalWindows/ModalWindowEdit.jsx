import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';

import UpdateFaculty from "../UpdateFaculty.jsx";

import FacultiesService from "../../../services/Faculties.service.js";
import { FacultyValidationSchema } from "../../../validations/Faculty.validation";

import ModalWindowPreloader from "../../ModalWindowPreloader";

import { Formik } from 'formik';

import React, { useState } from 'react';

export default function ModalWindowEdit({ show, handleClose, onLoadFaculties, shortName }) {
    const [updatedFaculty, setUpdatedFaculty] = useState(null);
    const [modelErrors, setModelErrors] = useState();

    const handleSubmit = (values) => {
        onEditFaculty(values);
    }

    const onEditFaculty = async (values) => {
        try {
            const form = new FormData();
            form.append("Faculty.FullName", values.fullName);
            form.append("Faculty.ShortName", values.shortName);
            form.append("Faculty.Img", values.img);
            form.append("Img", values.fileImg);
            setModelErrors(null);

            await FacultiesService.httpPut(shortName, form);
            onLoadFaculties();
            handleClose();
        }
        catch (err) {
            setModelErrors(JSON.parse(`{${err.message.replace('Error', '"Error"')}}`).Error);
        }
    }
    const getFacultyByShortName = async () => {
        const faciltyData = await FacultiesService.httpGetByShortName(shortName);
        setUpdatedFaculty(faciltyData);
    }
    const onClose = () => {
        setModelErrors(null);
        handleClose();
    }

    React.useEffect(() => {
        if (shortName) {
            getFacultyByShortName();
        }
        else {
            setUpdatedFaculty(null);
        }
    }, [shortName]);

    if (!updatedFaculty) {
        return <ModalWindowPreloader show={show} handleClose={handleClose} />;
    }
    else {
        return (
            <Modal show={show} onHide={onClose} backdrop="static" keyboard={false}>
                <Formik
                    validationSchema={FacultyValidationSchema}
                    onSubmit={handleSubmit}
                    initialValues={{ ...updatedFaculty, fileImg: null }}>
                    {({ handleSubmit, handleChange, values, touched, errors }) => (
                        <Form noValidate onSubmit={handleSubmit}>
                            <Modal.Header closeButton>
                                <Modal.Title>Изменить факультет <b className="text-success">"{shortName}"</b></Modal.Title>
                            </Modal.Header>
                            <Modal.Body>
                                <UpdateFaculty form={values} errors={errors} onChangeModel={handleChange} modelErrors={modelErrors} />
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
