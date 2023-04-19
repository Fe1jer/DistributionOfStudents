import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';

import StatisticService from "../../../services/Statistic.service.js";
import AdmissionsApi from "../../../api/AdmissionsApi.js";
import SubjectsService from "../../../services/Subjects.service.js";
import RecruitmentPlansService from "../../../services/RecruitmentPlans.service.js";

import UpdateAdmission from "../UpdateAdmission.jsx";

import React, { useState } from 'react';
import { useParams } from 'react-router-dom'

import { getNow } from "../../../../../src/js/datePicker.js"

export default function CreateModalWindow({ show, handleClose, onLoadAdmissions, onLoadGroup }) {
    const params = useParams();
    const groupId = params.groupId;
    const facultyShortName = params.shortName;

    const defaultStudent = {
        id: 0, gps: 0,
        name: "", surname: "", patronymic: "",
    }
    const defaultAdmission = {
        id: 0,
        dateOfApplication: getNow()
    }

    const [isLoaded, setIsLoaded] = useState(false);
    const [student, setStudent] = useState(defaultStudent);
    const [admission, setAdmission] = useState(defaultAdmission);
    const [studentScores, setStudentScores] = useState([]);
    const [specialitiesPriority, setSpecialitiesPriority] = useState([]);
    const [groupSubjects, setGroupSubjects] = useState(null);
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
        onCreateAdmission();
        setValidated(true);
    }

    const setDefaultValues = () => {
        setValidated(false);
        setIsLoaded(false);
        setAdmission(defaultAdmission);
        setStudent(defaultStudent);
        setStudentScores([]);
        setSpecialitiesPriority([]);
        setGroupSubjects(null);
        setGroupPlans(null);
    }
    const onCreateAdmission = () => {
        admission.student = student;
        admission.studentScores = studentScores;
        admission.specialitiesPriority = specialitiesPriority;
        var xhr = new XMLHttpRequest();
        xhr.open("post", AdmissionsApi.getPostUrl(groupId), true);
        xhr.setRequestHeader("Content-Type", "application/json")
        xhr.onload = function () {
            setErrors(null);
            if (xhr.status === 200) {
                handleClose();
                onUpdateStatistic();
                onLoadAdmissions();
                onLoadGroup();
                setDefaultValues();
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
    const loadGroupPlans = async () => {
        const recruitmentsPlansData = await RecruitmentPlansService.httpGetGroupRecruitmentPlans(facultyShortName, groupId);
        setGroupPlans(recruitmentsPlansData);
    }
    const loadGroupSubjects = async () => {
        const subjectsData = await SubjectsService.httpGetGroupSubjects(groupId);
        setGroupSubjects(subjectsData);
    }

    React.useEffect(() => {
        if (!isLoaded) {
            loadGroupSubjects();
            loadGroupPlans();
            setIsLoaded(true);
            return;
        }
        if (groupPlans && specialitiesPriority.length == 0) {
            setSpecialitiesPriority(groupPlans.map(item => { return { planId: item.id, nameSpeciality: item.speciality.directionName ?? item.speciality.fullName, priority: 0 } }))
        }
        if (groupSubjects && studentScores.length == 0) {
            setStudentScores(groupSubjects.map(item => { return { subject: item, score: 0 } }))
        }
        if (show) {
            admission.dateOfApplication = getNow();
            setAdmission(admission);
        }
    }, [groupPlans, groupSubjects, show, admission]);

    if (!groupSubjects || !groupPlans) {
        return (
            <Modal show={show} onHide={handleClose} backdrop="static" keyboard={false}>
                <Modal.Header closeButton>
                    <Modal.Title>Создать заявку</Modal.Title>
                </Modal.Header>
                <Modal.Body className="text-center">
                    Загрузка...
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={handleClose}>Закрыть</Button>
                    <Button type="submit" variant="primary">Сохранить</Button>
                </Modal.Footer>
            </Modal>
        )
    }
    else {
        return (
            <Modal size="xl" fullscreen="lg-down" show={show} onHide={handleClose} backdrop="static" keyboard={false}>
                <Form noValidate validated={validated} onSubmit={handleSubmit}>
                    <Modal.Header closeButton>
                        <Modal.Title>Создать заявку</Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        <UpdateAdmission admission={admission} student={student} specialitiesPriority={specialitiesPriority} studentScores={studentScores} errors={errors} onChangeModel={onChangeModel} />
                    </Modal.Body>
                    <Modal.Footer>
                        <Button variant="secondary" onClick={handleClose}>Закрыть</Button>
                        <Button type="submit" variant="primary">Сохранить</Button>
                    </Modal.Footer>
                </Form >
            </Modal>
        );
    }
}
