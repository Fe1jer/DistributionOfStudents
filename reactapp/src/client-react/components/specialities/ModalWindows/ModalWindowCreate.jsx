import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';

import { SpecialityValidationSchema } from '../../../validations/Speciality.validation';

import FacultiesService from "../../../services/Faculties.service";
import SpecialitiesService from "../../../services/Specialities.service";
import UpdateSpeciality from "../UpdateSpeciality";

import { Formik } from 'formik';
import React, { useState } from 'react';
import { useParams } from 'react-router-dom'

export default function ModalWindowCreate({ show, handleClose, onLoadSpecialities }) {
    const params = useParams();
    const shortName = params.shortName;
    const defaultSpeciality = {
        id: 0,
        fullName: null,
        shortName: null,
        code: null,
        shortCode: null,
        directionName: null,
        directionCode: null,
        specializationName: null,
        specializationCode: null,
        description: null
    }

    const [faculty, setFaculty] = useState(null);

    const handleSubmit = (values) => {
        onCreateSpeciality(values);
    }
    const onCreateSpeciality = async (values) => {
        values.faculty = faculty;
        await SpecialitiesService.httpPost(shortName, values);
        handleClose();
        onLoadSpecialities();
    }
    const getFacultyByShortName = async () => {
        const faciltyData = await FacultiesService.httpGetByShortName(shortName);
        setFaculty(faciltyData);
    }

    React.useEffect(() => {
        getFacultyByShortName();
    }, []);

    return (
        <Modal size="xl" fullscreen="lg-down" show={show} onHide={handleClose} backdrop="static" keyboard={false}>
            <Formik
                validationSchema={SpecialityValidationSchema}
                onSubmit={handleSubmit}
                initialValues={defaultSpeciality}>
                {({ handleSubmit, handleChange, values, touched, errors }) => (
                    <Form noValidate onSubmit={handleSubmit}>
                        <Modal.Header closeButton>
                            <Modal.Title>Добавить специальность</Modal.Title>
                        </Modal.Header>
                        <Modal.Body>
                            <UpdateSpeciality speciality={values} errors={errors} onChangeModel={handleChange} />
                        </Modal.Body>
                        <Modal.Footer>
                            <Button variant="secondary" onClick={handleClose}>Закрыть</Button>
                            <Button type="submit" variant="primary">Сохранить</Button>
                        </Modal.Footer>
                    </Form >
                )}
            </Formik>
        </Modal>
    );
}