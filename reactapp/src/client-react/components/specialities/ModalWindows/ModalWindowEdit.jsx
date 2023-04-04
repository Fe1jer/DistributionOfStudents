import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';

import FacultiesApi from "../../../api/FacultiesApi.js";
import SubjectsApi from "../../../api/SpecialitiesApi.js";
import UpdateSpeciality from "../UpdateSpeciality.jsx";

import React, { useState } from 'react';
import { useParams } from 'react-router-dom'

export default function ModalWindowEdit({ show, handleClose, specialityId, onLoadSpecialities }) {
    const params = useParams();
    const shortName = params.shortName;

    const [faculty, setFaculty] = useState(null);
    const [specialityFullName, setSpecialityFullName] = useState(null);
    const [validated, setValidated] = useState(false);
    const [errors, setErrors] = useState();
    const [updatedSpeciality, setUpdatedSpeciality] = useState(null);

    const onChangeModel = (updateSpeciality) => {
        setUpdatedSpeciality(updateSpeciality);
    }
    const handleSubmit = (e) => {
        e.preventDefault();
        onEditSpeciality();
        setValidated(true);
    }

    const onEditSpeciality = () => {
        if (specialityId !== null) {
            updatedSpeciality.faculty = faculty;
            var xhr = new XMLHttpRequest();
            xhr.open("put", SubjectsApi.getPutUrl(specialityId), true);
            xhr.setRequestHeader("Content-Type", "application/json")
            xhr.onload = function () {
                setErrors(null);
                if (xhr.status === 200) {
                    handleClose();
                    onLoadSpecialities();
                    setValidated(false);
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
    }
    const getSpetyalityById = () => {
        var xhr = new XMLHttpRequest();
        xhr.open("get", SubjectsApi.getSpecialityUrl(specialityId), true);
        xhr.setRequestHeader("Content-Type", "application/json")
        xhr.onload = function () {
            if (xhr.status === 200) {
                setUpdatedSpeciality(JSON.parse(xhr.responseText));
                setSpecialityFullName(JSON.parse(xhr.responseText).fullName);
            }
        }.bind(this);
        xhr.send();
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
    const onClose = () => {
        setValidated(false);
        setErrors(null);
        handleClose();
    }

    React.useEffect(() => {
        getFacultyByShortName();
        if (specialityId) {
            getSpetyalityById();
        }
        else {
            setUpdatedSpeciality(null);
        }
    }, [specialityId]);

    if (!updatedSpeciality) {
        return (
            <Modal show={show} onHide={handleClose} backdrop="static" keyboard={false}>
                <Modal.Header closeButton>
                    <Modal.Title>Изменить специальность</Modal.Title>
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
            <Modal size="xl" fullscreen="lg-down" show={show} onHide={onClose} backdrop="static" keyboard={false}>
                <Form noValidate validated={validated} onSubmit={handleSubmit}>
                    <Modal.Header closeButton>
                        <Modal.Title>Изменить специальность <b className="text-success">"{specialityFullName}"</b></Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        <UpdateSpeciality speciality={updatedSpeciality} errors={errors} onChangeModel={onChangeModel} />
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