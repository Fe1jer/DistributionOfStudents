import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';

import StatisticService from "../../../services/Statistic.service.js";
import AdmissionsService from "../../../services/Admissions.service.js";
import RecruitmentPlansService from "../../../services/RecruitmentPlans.service.js";

import ModalWindowPreloader from "../../ModalWindowPreloader";

import { AdmissionValidationSchema } from "../../../validations/Admission.validation";

import UpdateAdmission from "../UpdateAdmission.jsx";

import { Formik } from 'formik';
import React, { useState } from 'react';
import { useParams } from 'react-router-dom'

export default function EditModalWindow({ show, handleClose, onLoadAdmissions, admissionId, onLoadGroup }) {
    const params = useParams();
    const groupId = params.groupId;
    const facultyShortName = params.shortName;

    const [isLoaded, setIsLoaded] = useState(false);
    const [studentFullName, setStudentFullName] = useState("");
    const [admission, setAdmission] = useState(null);
    const [studentScores, setStudentScores] = useState(null);
    const [specialityPriorities, setSpecialityPriorities] = useState(null);
    const [admissionSpecialitiesPriority, setAdmissionSpecialitiesPriority] = useState(null);
    const [groupPlans, setGroupPlans] = useState(null);

    const handleSubmit = (values) => {
        onEditAdmission(values);
    }
    const setDefaultValues = () => {
        setIsLoaded(false);
        setAdmission(null);
        setStudentScores(null);
        setSpecialityPriorities(null);
        setAdmissionSpecialitiesPriority(null);
        setGroupPlans(null);
    }
    const onEditAdmission = async (values) => {
        await AdmissionsService.httpPut(admissionId, values);
        handleClose();
        onLoadAdmissions();
        onLoadGroup();
        setDefaultValues();
        onUpdateStatistic();
    }
    const onUpdateStatistic = async () => {
        await StatisticService.httpPutGroupStatisticUrl(facultyShortName, groupId);
    }
    const loadAdmission = async () => {
        var data = await AdmissionsService.httpGetById(admissionId);
        setAdmission(data);
        setAdmissionSpecialitiesPriority(data.specialityPriorities);
        setStudentFullName(data.student.surname + " " + data.student.name + " " + data.student.patronymic);
        setStudentScores(data.studentScores);
    }
    const loadGroupPlans = async () => {
        const recruitmentsPlansData = await RecruitmentPlansService.httpGetGroupPlans(facultyShortName, groupId);
        setGroupPlans(recruitmentsPlansData);
    }
    React.useEffect(() => {
        if (admissionId) {
            if (!isLoaded) {
                loadGroupPlans();
                loadAdmission();
                setIsLoaded(true);
                return;
            }
            if (groupPlans && admissionSpecialitiesPriority && !specialityPriorities) {
                setSpecialityPriorities(groupPlans.map(item => {
                    var tempSpecialitiesPriority = admissionSpecialitiesPriority.find(element => element.recruitmentPlanId === item.id);
                    return {
                        recruitmentPlanId: item.id, specialityName: item.specialityName,
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
        handleClose();
    }
    if (!admission || !specialityPriorities || !studentScores) {
        return <ModalWindowPreloader show={show} handleClose={handleClose} />;
    }
    else {
        return (
            <Modal size="xl" fullscreen="lg-down" show={show} onHide={onClose} backdrop="static" keyboard={false}>
                <Formik
                    validationSchema={AdmissionValidationSchema}
                    onSubmit={handleSubmit}
                    initialValues={{ ...admission, studentScores, specialityPriorities }}>
                    {({ handleSubmit, handleChange, values, touched, errors }) => (
                        <Form noValidate onSubmit={handleSubmit}>
                            <Modal.Header closeButton>
                                <Modal.Title>Изменить заявку <b className="text-success">"{studentFullName}"</b></Modal.Title>
                            </Modal.Header>
                            <Modal.Body>
                                <UpdateAdmission values={values} errors={errors} onChangeModel={handleChange} />
                            </Modal.Body>
                            <Modal.Footer>
                                <Button variant="secondary" onClick={onClose}>Закрыть</Button>
                                <Button type="submit" variant="success">Сохранить</Button>
                            </Modal.Footer>
                        </Form >
                    )}
                </Formik>
            </Modal>
        );
    }
}
