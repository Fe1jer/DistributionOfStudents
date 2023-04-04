import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';

import UpdateFaculty from "../UpdateFaculty.jsx";

import FacultiesApi from "../../../api/FacultiesApi.js";

import React, { useState } from 'react';

export default function ModalWindowEdit({ show, handleClose, onLoadFaculties, shortName }) {
    const [updatedFaculty, setUpdatedFaculty] = useState(null);
    const [updatedImg, setUpdatedImg] = useState(null);
    const [validated, setValidated] = useState(false);
    const [errors, setErrors] = useState();
    const [modelErrors, setModelErrors] = useState();

    const handleSubmit = (e) => {
        e.preventDefault();
        onEditFaculty();
        setValidated(true);
    }
    const onChangeModel = (faculty, img) => {
        setUpdatedFaculty(faculty);
        setUpdatedImg(img);
    }

    const onEditFaculty = () => {
        const form = new FormData();
        form.append("Faculty.FullName", updatedFaculty.fullName);
        form.append("Faculty.ShortName", updatedFaculty.shortName);
        form.append("Faculty.Img", updatedFaculty.img);
        form.append("Img", updatedImg);
        var xhr = new XMLHttpRequest();
        xhr.open("put", FacultiesApi.getPutUrl(shortName), true);
        xhr.onload = function () {
            setModelErrors(null);
            setErrors(null);
            if (xhr.status === 200) {
                onLoadFaculties();
                handleClose();
                setValidated(false);
            }
            else if (xhr.status === 400) {
                var a = eval('({obj:[' + xhr.response + ']})');
                if (a.obj[0].errors) {
                    setErrors(a.obj[0].errors);
                }
                if (a.obj[0].modelErrors) {
                    setModelErrors(a.obj[0].modelErrors);
                }
            }
        }.bind(form);
        xhr.send(form);
    }
    const getFacultyByShortName = () => {
        var xhr = new XMLHttpRequest();
        xhr.open("get", FacultiesApi.getFacultyUrl(shortName), true);
        xhr.setRequestHeader("Content-Type", "application/json")
        xhr.onload = function () {
            if (xhr.status === 200) {
                setUpdatedFaculty(JSON.parse(xhr.responseText));
            }
        }.bind(this);
        xhr.send();
    }
    const onClose = () => {
        setValidated(false);
        setModelErrors(null);
        setErrors(null);
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
        return (
            <Modal show={show} onHide={handleClose} backdrop="static" keyboard={false}>
                <Modal.Header closeButton>
                    <Modal.Title>Изменить факультет</Modal.Title>
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
                <Form noValidate validated={validated} onSubmit={handleSubmit}>
                    <Modal.Header closeButton>
                        <Modal.Title>Изменить факультет <b className="text-success">"{shortName}"</b></Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        <UpdateFaculty faculty={updatedFaculty} errors={errors} onChangeModel={onChangeModel} modelErrors={modelErrors} />
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
