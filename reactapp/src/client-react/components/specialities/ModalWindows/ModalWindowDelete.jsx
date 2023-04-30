import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';

import ModalWindowPreloader from "../../ModalWindowPreloader";

import SpecialitiesService from "../../../services/Specialities.service";

import React, { useState } from 'react';

export default function ModalWindowDelete({ show, handleClose, specialityId, onLoadSpecialities }) {
    const [speciality, setSpeciality] = useState(null);

    const handleSubmit = (e) => {
        e.preventDefault();
        onDeleteSpeciality();
    }

    const onDeleteSpeciality = async () => {
        if (specialityId !== null) {
            await SpecialitiesService.httpDelete(specialityId);
            handleClose();
            onLoadSpecialities();
        }
    }
    const getSpetyalityById = async () => {
        console.log("d");
        var specialityData = await SpecialitiesService.httpGetById(specialityId);
        setSpeciality(specialityData);
    }

    React.useEffect(() => {
        if (specialityId && show) {
            getSpetyalityById();
        }
        else {
            setSpeciality(null);
        }
    }, [specialityId]);

    if (!speciality) {
        return (
            <ModalWindowPreloader show={show} handleClose={handleClose} />
        );
    }
    else {
        return (
            <Modal show={show} onHide={handleClose} backdrop="static" keyboard={false}>
                <Form onSubmit={handleSubmit}>
                    <Modal.Header closeButton>
                        <Modal.Title>Удалить специальность</Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        Вы уверенны, что хотите удалить специальность?
                        <br />
                        Специальность <b className="text-success">"{speciality.directionName ?? speciality.fullName}"</b> будет удалена без возможности восстановления.
                    </Modal.Body>
                    <Modal.Footer>
                        <Button variant="secondary" onClick={handleClose}>Закрыть</Button>
                        <Button type="submit" variant="outline-danger">Удалить</Button>
                    </Modal.Footer>
                </Form >
            </Modal>
        );
    }
}
