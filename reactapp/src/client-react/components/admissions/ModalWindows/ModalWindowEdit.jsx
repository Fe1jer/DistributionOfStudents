import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';

import StatisticService from "../../../services/Statistic.service.js";
import AdmissionsApi from "../../../api/AdmissionsApi.js";
import RecruitmentPlansApi from "../../../api/RecruitmentPlansApi.js";

import UpdateAdmission from "../UpdateAdmission.jsx";

import React, { useState } from 'react';
import { useParams } from 'react-router-dom'

export default function EditModalWindow({ show, handleClose, onLoadAdmissions, admissionId, onLoadGroup }) {
    const params = useParams();
    const groupId = params.groupId;
    const facultyShortName = params.shortName;

    const [isLoaded, setIsLoaded] = useState(false);
    const [studentFullName, setStudentFullName] = useState("");
    const [student, setStudent] = useState(null);
    const [admission, setAdmission] = useState(null);
    const [studentScores, setStudentScores] = useState(null);
    const [specialitiesPriority, setSpecialitiesPriority] = useState(null);
    const [admissionSpecialitiesPriority, setAdmissionSpecialitiesPriority] = useState(null);
    const [groupPlans, setGroupPlans] = useState(null);
    const [validated, setValidated] = useState(false);
    const [errors, setErrors] = useState();

    const onChangeModel = (updateAdmission, updateStudent, updateStudentScores, updateSpecialitiesPriority) => {
        setAdmission(updateAdmission);
        setStudent(updateStudent);
        setStudentScores(updateStudentScores);
        setSpecialitiesPriority(updateSpecialitiesPriority);
    }
    const handleSubmit = (e) => {
        e.preventDefault();
        onEditAdmission();
        setValidated(true);
    }
    const setDefaultValues = () => {
        setValidated(false);
        setIsLoaded(false);
        setAdmission(null);
        setStudent(null);
        setStudentScores(null);
        setSpecialitiesPriority(null);
        setAdmissionSpecialitiesPriority(null);
        setGroupPlans(null);
    }
    const onEditAdmission = () => {
        admission.student = student;
        admission.studentScores = studentScores;
        admission.specialitiesPriority = specialitiesPriority;
        var xhr = new XMLHttpRequest();
        xhr.open("put", AdmissionsApi.getPutUrl(admissionId), true);
        xhr.setRequestHeader("Content-Type", "application/json")
        xhr.onload = function () {
            setErrors(null);
            if (xhr.status === 200) {
                handleClose();
                onLoadAdmissions();
                onLoadGroup();
                setDefaultValues();
                onUpdateStatistic();
            }
            else if (xhr.status === 400) {
                var a = eval('({obj:[' + xhr.response + ']})');
                if (a.obj[0].errors) {
                    setErrors(a.obj[0].errors);
                }
            }
        }.bind(this);
        xhr.send(JSON.stringify(admission));
    }
    const onUpdateStatistic = async () => {
        await StatisticService.httpPutGroupStatisticUrl(facultyShortName, groupId);
    }
    const loadAdmission = () => {
        var xhr = new XMLHttpRequest();
        xhr.open("get", AdmissionsApi.getAdmissionUrl(admissionId), true);
        xhr.onload = function () {
            var data = JSON.parse(xhr.responseText);
            setAdmission(data);
            setAdmissionSpecialitiesPriority(data.specialityPriorities);
            setStudent(data.student);
            setStudentFullName(data.student.surname + " " + data.student.name + " " + data.student.patronymic);
            setStudentScores(data.studentScores);
        }.bind(this);
        xhr.send();
    }
    const loadGroupPlans = () => {
        var xhr = new XMLHttpRequest();
        xhr.open("get", RecruitmentPlansApi.getGroupRecruitmentPlansUrl(facultyShortName, groupId), true);
        xhr.onload = function () {
            var data = JSON.parse(xhr.responseText);
            setGroupPlans(data);
        }.bind(this);
        xhr.send();
    }
    React.useEffect(() => {
        if (admissionId) {
            if (!isLoaded) {
                loadGroupPlans();
                loadAdmission();
                setIsLoaded(true);
                return;
            }
            if (groupPlans && admissionSpecialitiesPriority && !specialitiesPriority) {
                setSpecialitiesPriority(groupPlans.map(item => {
                    var tempSpecialitiesPriority = admissionSpecialitiesPriority.find(element => element.recruitmentPlan.id === item.id);
                    return {
                        planId: item.id, nameSpeciality: item.speciality.directionName ?? item.speciality.fullName,
                        priority: tempSpecialitiesPriority ? tempSpecialitiesPriority.priority : 0
                    }
                }))
            }
        }
        else {
            setDefaultValues();
        }
    }, [groupPlans, admissionSpecialitiesPriority, admissionId]);

    const onClose = () => {
        setValidated(false);
        setErrors(null);
        handleClose();
    }
    if (!admission || !student || !specialitiesPriority || !studentScores) {
        return (
            <Modal show={show} onHide={onClose} backdrop="static" keyboard={false}>
                <Modal.Header closeButton>
                    <Modal.Title>Изменить заявку</Modal.Title>
                </Modal.Header>
                <Modal.Body className="text-center">
                    Загрузка...
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={onClose}>Закрыть</Button>
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
                        <Modal.Title>Изменить заявку <b className="text-success">"{studentFullName}"</b></Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        <UpdateAdmission admission={admission} student={student} specialitiesPriority={specialitiesPriority} studentScores={studentScores} errors={errors} onChangeModel={onChangeModel} />
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
