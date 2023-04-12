import AdmissionsApi from "../../../api/AdmissionsApi.js";
import StatisticApi from "../../../api//StatisticApi.js";

import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';

import React, { useState } from 'react';
import { useParams } from 'react-router-dom'

export default function ModalWindowDelete({ show, handleClose, onLoadAdmissions, admissionId, onLoadGroup }) {
    const params = useParams();
    const groupId = params.groupId;
    const facultyShortName = params.shortName;

    const [admission, setAdmission] = useState(null);

    const handleSubmit = (e) => {
        e.preventDefault();
        onDeleteAdmission(admissionId);
        handleClose();
    }
    const onDeleteAdmission = (id) => {
        var xhr = new XMLHttpRequest();
        xhr.open("delete", AdmissionsApi.getDeleteUrl(admissionId), true);
        xhr.onload = function () {
            if (xhr.status === 200) {
                onLoadGroup();
                onLoadAdmissions();
                onUpdateStatistic();
            }
        }.bind(this);
        xhr.send();
    }
    const onUpdateStatistic = () => {
        var xhr = new XMLHttpRequest();
        xhr.open("put", StatisticApi.getPutGroupStatisticUrl(facultyShortName, groupId), true);
        xhr.setRequestHeader("Content-Type", "application/json")
        xhr.onload = function () {
        }.bind(this);
        xhr.send();
    }
    const loadAdmission = () => {
        var xhr = new XMLHttpRequest();
        xhr.open("get", AdmissionsApi.getAdmissionUrl(admissionId), true);
        xhr.onload = function () {
            var data = JSON.parse(xhr.responseText);
            setAdmission(data);
        }.bind(this);
        xhr.send();
    }

    React.useEffect(() => {
        if (admissionId) {
            loadAdmission();
        }
        else {
            setAdmission(null);
        }
    }, [admissionId])

    if (!admissionId || admission == null) {
        return (
            <Modal show={show} onHide={handleClose} backdrop="static" keyboard={false}>
                <Modal.Header closeButton>
                    <Modal.Title>Удалить заявку</Modal.Title>
                </Modal.Header>
                <Modal.Body className="text-center">
                    Загрузка...
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={handleClose}>Закрыть</Button>
                    <Button variant="outline-danger">Удалить</Button>
                </Modal.Footer>
            </Modal>
        );
    }
    else {
        return (
            <Modal show={show} onHide={handleClose} backdrop="static" keyboard={false}>
                <Form onSubmit={handleSubmit}>
                    <Modal.Header closeButton>
                        <Modal.Title>Удалить заявку</Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        <p>
                            Вы уверенны, что хотите удалить эту заявку?
                            <br />
                            Заявка абитуриента <b className="text-success">{admission.student.surname} {admission.student.name} {admission.student.patronymic}</b> будет удалёна без возможности восстановления.
                        </p>
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
