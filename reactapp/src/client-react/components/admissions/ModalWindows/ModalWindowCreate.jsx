import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';

import StatisticService from "../../../services/Statistic.service.js";
import AdmissionsService from "../../../services/Admissions.service.js";
import SubjectsService from "../../../services/Subjects.service.js";
import RecruitmentPlansService from "../../../services/RecruitmentPlans.service.js";

import { AdmissionValidationSchema } from "../../../validations/Admission.validation";

import UpdateAdmission from "../UpdateAdmission.jsx";

import ModalWindowPreloader from "../../ModalWindowPreloader";

import { Formik } from 'formik';
import React, { useState } from 'react';
import { useParams } from 'react-router-dom'

import { getNow } from "../../../../../src/js/datePicker.js"

export default function CreateModalWindow({ show, handleClose, onLoadAdmissions, onLoadGroup }) {
    const params = useParams();
    const groupId = params.groupId;
    const facultyShortName = params.shortName;

    const defaultStudent = {
        id: "00000000-0000-0000-0000-000000000000", gpa: 0,
        name: "", surname: "", patronymic: "",
    }
    const defaultAdmission = {
        id: "00000000-0000-0000-0000-000000000000",
        dateOfApplication: getNow(),
        isTargeted: false,
        isWithoutEntranceExams: false,
        isOutOfCompetition: false,
        groupOfSpecialtiesId: groupId
    }
    defaultAdmission.student = defaultStudent;

    const [isLoaded, setIsLoaded] = useState(false);
    const [studentScores, setStudentScores] = useState([]);
    const [specialityPriorities, setSpecialityPriorities] = useState([]);
    const [groupSubjects, setGroupSubjects] = useState(null);
    const [groupPlans, setGroupPlans] = useState(null);

    const handleSubmit = (values) => {
        onCreateAdmission(values);
    }

    const onCreateAdmission = async (values) => {
        await AdmissionsService.httpPost(groupId, values);

        handleClose();
        onUpdateStatistic();
        onLoadAdmissions();
        onLoadGroup();
    }
    const onUpdateStatistic = async () => {
        await StatisticService.httpPutGroupStatisticUrl(facultyShortName, groupId);
    }
    const loadGroupPlans = async () => {
        const recruitmentsPlansData = await RecruitmentPlansService.httpGetGroupPlans(facultyShortName, groupId);
        setGroupPlans(recruitmentsPlansData);
    }
    const loadGroupSubjects = async () => {
        const subjectsData = await SubjectsService.httpGetGroupSubjects(groupId);
        setGroupSubjects(subjectsData);
    }

    React.useEffect(() => {
        if (!isLoaded) {
            setIsLoaded(true);
            loadGroupSubjects();
            loadGroupPlans();
            return;
        }
        if (groupPlans && specialityPriorities.length === 0) {
            setSpecialityPriorities(groupPlans.map(item => { return { recruitmentPlanId: item.id, specialityName: item.specialityName, priority: 0 } }))
        }
        if (groupSubjects && studentScores.length === 0) {
            setStudentScores(groupSubjects.map(item => { return { subject: item, subjectId: item.id, score: 0 } }))
        }
        if (show) {
            defaultAdmission.dateOfApplication = getNow();
        }
    }, [groupPlans, groupSubjects, show, defaultAdmission]);

    if (!groupSubjects || !groupPlans) {
        return <ModalWindowPreloader show={show} handleClose={handleClose} />;
    }
    else {
        return (
            <Modal size="xl" fullscreen="lg-down" show={show} onHide={handleClose} backdrop="static" keyboard={false}>
                <Formik
                    validationSchema={AdmissionValidationSchema}
                    onSubmit={handleSubmit}
                    initialValues={{ ...defaultAdmission, studentScores, specialityPriorities }}>
                    {({ handleSubmit, handleChange, values, touched, errors }) => (
                        <Form noValidate onSubmit={handleSubmit}>
                            <Modal.Header closeButton>
                                <Modal.Title>Создать заявку</Modal.Title>
                            </Modal.Header>
                            <Modal.Body>
                                <UpdateAdmission values={values} errors={errors} onChangeModel={handleChange} />
                            </Modal.Body>
                            <Modal.Footer>
                                <Button variant="secondary" onClick={handleClose}>Закрыть</Button>
                                <Button type="submit" variant="success">Сохранить</Button>
                            </Modal.Footer>
                        </Form >
                    )}
                </Formik>
            </Modal>
        );
    }
}
