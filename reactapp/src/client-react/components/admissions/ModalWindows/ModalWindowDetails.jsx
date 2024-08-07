import Modal from 'react-bootstrap/Modal';
import Button from 'react-bootstrap/Button';
import Col from 'react-bootstrap/Col';
import Row from 'react-bootstrap/Row';

import AdmissionsService from "../../../services/Admissions.service.js";
import RecruitmentPlansService from "../../../services/RecruitmentPlans.service.js";

import ModalWindowPreloader from "../../ModalWindowPreloader";

import React, { useState } from 'react';
import { useParams } from 'react-router-dom'

export default function ModalWindowDetails({ show, handleClose, admissionId }) {
    const params = useParams();
    const groupId = params.groupId;
    const facultyShortName = params.shortName;

    const [isLoaded, setIsLoaded] = useState(false);
    const [student, setStudent] = useState(null);
    const [admission, setAdmission] = useState(null);
    const [studentScores, setStudentScores] = useState(null);
    const [specialitiesPriority, setSpecialitiesPriority] = useState(null);
    const [admissionSpecialitiesPriority, setAdmissionSpecialitiesPriority] = useState(null);
    const [groupPlans, setGroupPlans] = useState(null);

    const setDefaultValues = () => {
        setIsLoaded(false);
        setAdmission(null);
        setStudent(null);
        setStudentScores(null);
        setSpecialitiesPriority(null);
        setAdmissionSpecialitiesPriority(null);
        setGroupPlans(null);
    }
    const loadAdmission = async () => {
        var data = await AdmissionsService.httpGetById(admissionId);
        setAdmission(data);
        setAdmissionSpecialitiesPriority(data.specialityPriorities);
        setStudent(data.student);
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
            if (groupPlans && admissionSpecialitiesPriority && !specialitiesPriority) {
                setSpecialitiesPriority(groupPlans.map(item => {
                    var tempSpecialitiesPriority = admissionSpecialitiesPriority.find(element => element.recruitmentPlanId === item.id);
                    return {
                        planId: item.id, specialityName: item.specialityName,
                        priority: tempSpecialitiesPriority ? tempSpecialitiesPriority.priority : 0
                    }
                }))
            }
        }
        else {
            setDefaultValues();
        }
    }, [groupPlans, admissionSpecialitiesPriority, admissionId]);

    if (!admission || !student || !specialitiesPriority || !studentScores) {
        return <ModalWindowPreloader show={show} handleClose={handleClose} />;
    }
    else {
        return (
            <Modal size="xl" fullscreen="lg-down" show={show} onHide={handleClose}>
                <Modal.Header closeButton>
                    <Modal.Title className="text-success">{student.surname} {student.name} {student.patronymic}</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Row>
                        <Col sm={4}>
                            <h5 className="text-center">Аттестат: {student.gpa}</h5>
                            <hr className="my-1" />
                            <h5 className="text-center">Баллы по ЦТ(ЦЭ)</h5>{
                                studentScores.map((item) =>
                                    <React.Suspense key={item.subject.name}>
                                        <hr className="my-1" />
                                        <p className="mb-0"><b>{item.subject.name}</b>: {item.score}</p>
                                    </React.Suspense>
                                )}
                        </Col>
                        <Col sm={8}>
                            <h5 className="text-center">Приоритет специальностей<sup>*</sup></h5>{
                                admissionSpecialitiesPriority.sort((a, b) => a.priority - b.priority).map((item, index) =>
                                    <React.Suspense key={groupPlans.find(x => x.id === item.recruitmentPlanId).specialityName}>
                                        <hr className="my-1" />
                                        <p className="mb-0">
                                            {index + 1}: <b>{groupPlans.find(x => x.id === item.recruitmentPlanId).specialityName}</b>
                                        </p>
                                    </React.Suspense>
                                )}
                        </Col>
                    </Row>
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={handleClose}>Закрыть</Button>
                </Modal.Footer>
            </Modal>
        );
    }
}
