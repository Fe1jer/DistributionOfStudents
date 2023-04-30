import AdmissionsService from "../../../services/Admissions.service.js";
import StatisticService from "../../../services/Statistic.service.js";

import ModalWindowPreloader from "../../ModalWindowPreloader";

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
        onDeleteAdmission();
        handleClose();
    }
    const onDeleteAdmission = async () => {
        await AdmissionsService.httpDelete(admissionId);
        onLoadGroup();
        onLoadAdmissions();
        onUpdateStatistic();
    }
    const onUpdateStatistic = async () => {
        await StatisticService.httpPutGroupStatisticUrl(facultyShortName, groupId);
    }
    const loadAdmission = async () => {
        var data = await AdmissionsService.httpGetById(admissionId);
        setAdmission(data);
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
        return <ModalWindowPreloader show={show} handleClose={handleClose} />;
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
