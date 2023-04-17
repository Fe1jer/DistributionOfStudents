import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';

import FacultiesApi from "../../../api/FacultiesApi.js";
import SubjectsApi from "../../../api/SpecialitiesApi.js";
import UpdateSpeciality from "../UpdateSpeciality.jsx";

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
    const [updatedSpeciality, setUpdatedSpeciality] = useState(defaultSpeciality);
    const [validated, setValidated] = useState(false);
    const [errors, setErrors] = useState();

    const onChangeModel = (updateSpeciality) => {
        setUpdatedSpeciality(updateSpeciality);
    }
    const handleSubmit = (e) => {
        e.preventDefault();
        onCreateSpeciality();
        setValidated(true);
    }

    const onCreateSpeciality = () => {
        updatedSpeciality.faculty = faculty;
        var xhr = new XMLHttpRequest();
        xhr.open("post", SubjectsApi.getPostUrl(shortName), true);
        xhr.setRequestHeader("Content-Type", "application/json")
        xhr.onload = function () {
            setErrors(null);
            if (xhr.status === 200) {
                handleClose();
                onLoadSpecialities();
                setValidated(false);
                setUpdatedSpeciality(defaultSpeciality);
            }
            else if (xhr.status === 400) {
                var a = eval('({obj:[' + xhr.response + ']})');
                if (a.obj[0].errors) {
                    setErrors(a.obj[0].errors);
                }
            }
        }.bind(this);
        xhr.send(JSON.stringify(updatedSpeciality));
    }
    const getFacultyByShortName = () => {
        var xhr = new XMLHttpRequest();
        xhr.open("get", FacultiesApi.getFacultyUrl(shortName), true);
        xhr.setRequestHeader("Content-Type", "application/json")
        xhr.onload = function () {
            if (xhr.status === 200) {
                setFaculty(JSON.parse(xhr.responseText));
            }
        }.bind(this);
        xhr.send();
    }

    React.useEffect(() => {
        getFacultyByShortName();
    }, []);

    return (
        <Modal size="xl" fullscreen="lg-down" show={show} onHide={handleClose} backdrop="static" keyboard={false}>
            <Form noValidate validated={validated} onSubmit={handleSubmit}>
                <Modal.Header closeButton>
                    <Modal.Title>Добавить специальность</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <UpdateSpeciality speciality={updatedSpeciality} errors={errors} onChangeModel={onChangeModel} />
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={handleClose}>Закрыть</Button>
                    <Button type="submit" variant="primary">Сохранить</Button>
                </Modal.Footer>
            </Form >
        </Modal>
    );
}