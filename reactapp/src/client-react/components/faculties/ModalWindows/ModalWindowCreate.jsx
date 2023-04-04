import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';

import UpdateFaculty from "../UpdateFaculty.jsx";

import FacultiesApi from "../../../api/FacultiesApi.js";

import React, { useState } from 'react';

export default function ModalWindowCreate({ show, handleClose, onLoadFaculties }) {
    const defaultFaculty = {
        id: 0,
        fullName: "",
        shortName: "",
        img: "\\img\\Faculties\\Default.jpg"
    }
    const [updatedFaculty, setUpdatedFaculty] = useState(defaultFaculty);
    const [updatedImg, setUpdatedImg] = useState(null);
    const [validated, setValidated] = useState(false);
    const [errors, setErrors] = useState(null);
    const [modelErrors, setModelErrors] = useState(null);

    const handleSubmit = (e) => {
        e.preventDefault();
        onCreateFaculty();
        setValidated(true);
    }
    const onChangeModel = (faculty, img) => {
        setUpdatedFaculty(faculty);
        setUpdatedImg(img);
    }

    const onCreateFaculty = () => {
        const form = new FormData();
        form.append("Faculty.FullName", updatedFaculty.fullName);
        form.append("Faculty.ShortName", updatedFaculty.shortName);
        form.append("Faculty.Img", updatedFaculty.img);
        form.append("Img", updatedImg);
        var xhr = new XMLHttpRequest();
        xhr.open("post", FacultiesApi.getPostUrl(), true);
        xhr.onload = function () {
            setModelErrors(null);
            setErrors(null);
            if (xhr.status === 200) {
                setUpdatedFaculty(defaultFaculty);
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
        }.bind(this);
        xhr.send(form);
    }

    return (
        <Modal show={show} onHide={handleClose} backdrop="static" keyboard={false}>
            <Form noValidate validated={validated} onSubmit={handleSubmit}>
                <Modal.Header closeButton>
                    <Modal.Title>Создать факультет</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <UpdateFaculty faculty={updatedFaculty} errors={errors} onChangeModel={onChangeModel} modelErrors={modelErrors} />
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={handleClose}>Закрыть</Button>
                    <Button type="submit" variant="primary">Сохранить</Button>
                </Modal.Footer>
            </Form >
        </Modal>
    );
}
