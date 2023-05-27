import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';

import { SpecialityValidationSchema } from '../../../validations/Speciality.validation';

import ModalWindowPreloader from "../../ModalWindowPreloader";

import FacultiesService from "../../../services/Faculties.service.js";
import SpecialitiesService from "../../../services/Specialities.service";
import UpdateSpeciality from "../UpdateSpeciality.jsx";

import { Formik } from 'formik';
import React, { useState } from 'react';
import { useParams } from 'react-router-dom'

export default function ModalWindowEdit({ show, handleClose, specialityId, onLoadSpecialities }) {
    const params = useParams();
    const shortName = params.shortName;

    const [faculty, setFaculty] = useState(null);
    const [specialityFullName, setSpecialityFullName] = useState(null);
    const [updatedSpeciality, setUpdatedSpeciality] = useState(null);

    const handleSubmit = (values) => {
        onEditSpeciality(values);
    }

    const onEditSpeciality = async (values) => {
        if (specialityId !== null) {
            values.faculty = faculty;
            await SpecialitiesService.httpPut(specialityId, values);
            handleClose();
            onLoadSpecialities();
        }
    }
    const getSpetyalityById = async () => {
        var specialityData = await SpecialitiesService.httpGetById(specialityId);
        setUpdatedSpeciality(specialityData);
        setSpecialityFullName(specialityData.fullName);
    }
    const getFacultyByShortName = async () => {
        const faciltyData = await FacultiesService.httpGetByShortName(shortName);
        setFaculty(faciltyData);
    }
    const onClose = () => {
        handleClose();
    }

    React.useEffect(() => {
        getFacultyByShortName();
        if (specialityId && show) {
            getSpetyalityById();
        }
        else {
            setUpdatedSpeciality(null);
        }
    }, [specialityId]);

    if (!updatedSpeciality) {
        return <ModalWindowPreloader show={show} handleClose={handleClose} />;
    }
    else {
        return (
            <Modal size="xl" fullscreen="lg-down" show={show} onHide={onClose} backdrop="static" keyboard={false}>
                <Formik
                    validationSchema={SpecialityValidationSchema}
                    onSubmit={handleSubmit}
                    initialValues={updatedSpeciality}>
                    {({ handleSubmit, handleChange, values, touched, errors }) => (
                        <Form noValidate onSubmit={handleSubmit}>
                            <Modal.Header closeButton>
                                <Modal.Title>Изменить специальность <b className="text-success">"{specialityFullName}"</b></Modal.Title>
                            </Modal.Header>
                            <Modal.Body>
                                <UpdateSpeciality speciality={values} errors={errors} onChangeModel={handleChange} />
                            </Modal.Body>
                            <Modal.Footer>
                                <Button variant="secondary" onClick={onClose}>Закрыть</Button>
                                <Button type="submit" variant="success">Сохранить</Button>
                            </Modal.Footer>
                        </Form >
                    )}
                </Formik>
            </Modal>
        );
    }
}