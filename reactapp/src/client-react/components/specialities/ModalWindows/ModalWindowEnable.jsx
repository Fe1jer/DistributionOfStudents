import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';

import FacultiesService from "../../../services/Faculties.service.js";
import SpecialitiesService from "../../../services/Specialities.service";

import React, { useState } from 'react';
import { useParams } from 'react-router-dom'

export default function ModalWindowEnable({ show, handleClose, specialityId, onLoadSpecialities }) {
    const params = useParams();
    const shortName = params.shortName;

    const [faculty, setFaculty] = useState(null);
    const [speciality, setSpeciality] = useState(null);

    const handleSubmit = (e) => {
        e.preventDefault();
        onEnableSpeciality();
    }

    const onEnableSpeciality = async () => {
        if (specialityId !== null) {
            speciality.faculty = faculty;
            speciality.isDisabled = false;
            await SpecialitiesService.httpPut(specialityId, speciality);
            handleClose();
            onLoadSpecialities();
        }
    }
    const getFacultyByShortName = async () => {
        const faciltyData = await FacultiesService.httpGetByShortName(shortName);
        setFaculty(faciltyData);
    }
    const getSpetyalityById = async () => {
        var specialityData = await SpecialitiesService.httpGetById(specialityId);
        setSpeciality(specialityData);
    }

    React.useEffect(() => {
        getFacultyByShortName();
        if (specialityId) {
            getSpetyalityById();
        }
        else {
            setSpeciality(null);
        }
    }, [specialityId]);

    if (!speciality) {
        return (
            <Modal show={show} onHide={handleClose} backdrop="static" keyboard={false}>
                <Modal.Header closeButton>
                    <Modal.Title>Вернуть специальность</Modal.Title>
                </Modal.Header>
                <Modal.Body className="text-center">
                    Загрузка...
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={handleClose}>Закрыть</Button>
                    <Button variant="primary">Вернуть</Button>
                </Modal.Footer>
            </Modal>
        );
    }
    else {
        return (
            <Modal show={show} onHide={handleClose} backdrop="static" keyboard={false}>
                <Form onSubmit={handleSubmit}>
                    <Modal.Header closeButton>
                        <Modal.Title>Вернуть специальность</Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        Вернуть специальность <b className="text-success">"{speciality.directionName ?? speciality.fullName}"</b>.
                    </Modal.Body>
                    <Modal.Footer>
                        <Button variant="secondary" onClick={handleClose}>Закрыть</Button>
                        <Button type="submit" variant="primary">Вернуть</Button>
                    </Modal.Footer>
                </Form >
            </Modal>
        );
    }
}
