import DistributionService from '../../services/Distribution.service.js';
import GroupsOfSpecialitiesService from '../../services/GroupsOfSpecialities.service.js';
import { DistributionValidationSchema } from "../../validations/Distribution.validation";

import ModalWindowConfirm from "./ModalWindows/ModalWindowConfirm.jsx";
import ModalWindowCreate from "./ModalWindows/ModalWindowCreate.jsx";

import DistributedPlan from "../distribution/DistributedPlan.jsx";

import CreateDistributionPlanList from './CreateDistributionPlanList.jsx';

import Button from 'react-bootstrap/Button';
import Form from 'react-bootstrap/Form';
import Placeholder from 'react-bootstrap/Placeholder';

import TablePreloader from "../TablePreloader.jsx";

import { Formik } from 'formik';
import React, { useState } from 'react';
import { Link, useNavigate, useParams } from 'react-router-dom';
import useDocumentTitle from '../useDocumentTitle.jsx';

export default function CreateDistributionPage() {
    useDocumentTitle("Распределение");

    const params = useParams();
    const facultyShortName = params.shortName;
    const groupId = params.groupId;
    const navigate = useNavigate();

    const [group, setGroup] = useState(null);
    const [plans, setPlans] = useState(null);
    const [isDistributed, setIsDistributed] = useState(false);
    const [createDistributionShow, setCreateDistributionShow] = useState(false);
    const [confirmDistributionShow, setConfirmDistributionShow] = useState(false);
    const [distributedPlans, setDistribitedPlans] = useState(null);
    const [modelErrors, setModelErrors] = useState(null);
    const [isInitSelectedStudents, setIsInitSelectedStudents] = useState(false);

    const setDefaultValues = () => {
        setCreateDistributionShow(false);
        setConfirmDistributionShow(false);
        setIsInitSelectedStudents(false);
        setPlans(null);
        setDistribitedPlans(null);
        setModelErrors(null);
    }
    const loadGroup = async () => {
        const groupData = await GroupsOfSpecialitiesService.httpGetById(groupId);
        setGroup(groupData);
    }
    const loadPlans = async () => {
        const data = await DistributionService.httpGet(facultyShortName, groupId);
        setIsDistributed(!data.areControversialStudents);
        setPlans(data.plans);
    }

    const onCreateDistribution = async () => {
        setModelErrors(null);
        try {
            const data = await DistributionService.httpPost(facultyShortName, groupId, distributedPlans);
            setDefaultValues();
            setIsDistributed(!data.areControversialStudents);
            setPlans(data.plans);
            initializationSelectedStudents(data.plans);
        }
        catch (err) {
            setModelErrors(JSON.parse(`{${err.message.replace('Error', '"Error"')}}`).Error);
        }
        handleCreateClose();
    }
    const onConfirmDistribution = async (notify) => {
        setModelErrors(null);
        try {
            await DistributionService.httpPostConfirm(facultyShortName, groupId, distributedPlans, notify);
            navigate("/Faculties/" + facultyShortName + "/" + groupId);
        }
        catch (err) {
            setModelErrors(JSON.parse(`{${err.message.replace('Error', '"Error"')}}`).Error);
        }
        handleConfirmClose();
    }
    const isDistributedStudent = (plan, enrolledStudent) => {
        return plan.count < plan.enrolledStudents.length && enrolledStudent.score === plan.passingScore;
    }
    const initializationSelectedStudentsInPlan = (plan) => {
        return plan.enrolledStudents.map((item) => {
            return { ...item, studentId: item.student.id, isDistributed: !isDistributedStudent(plan, item) }
        });
    }
    const initializationSelectedStudents = (plans) => {
        if (!isInitSelectedStudents) {
            setIsInitSelectedStudents(true);
            var initializatedDistributedPlans = plans.map((item) => {
                return { id: item.id, count: item.count, passingScore: item.passingScore, distributedStudents: initializationSelectedStudentsInPlan(item) }
            });
            setDistribitedPlans(initializatedDistributedPlans);
        }
    };

    const handleCreateSubmit = (values) => {
        setDistribitedPlans(values.distributedPlans);
        setCreateDistributionShow(true);
    }
    const handleConfirmSubmit = () => {
        setConfirmDistributionShow(true);
    }
    const handleCreateClose = () => {
        setCreateDistributionShow(false);
    };
    const handleConfirmClose = () => {
        setConfirmDistributionShow(false);
    };

    const _formGroupErrors = (errors) => {
        if (errors) {
            return errors.map((error) => <React.Suspense key={error}><span>{error}</span><br></br></React.Suspense>)
        }
    }

    const _showConfirmDistrinution = () => {
        return (
            <React.Suspense>
                <ModalWindowConfirm show={confirmDistributionShow} handleClose={handleConfirmClose} onConfirmDistribution={onConfirmDistribution} />{
                    plans.map((plan, index) =>
                        <React.Suspense key={plan.speciality.directionName ?? plan.speciality.fullName}>
                            <DistributedPlan plan={plan} />
                            {index !== plans.length - 1 ? <hr /> : null}
                        </React.Suspense>
                    )}
                <div className="text-center pt-4">
                    <Button size="lg" variant="outline-success" onClick={() => handleConfirmSubmit()}>Подтвердить</Button>
                    <Link type="button" className="btn btn-outline-danger btn-lg ms-2" to={"/Faculties/" + facultyShortName + "/" + groupId}>Отмена</Link>
                </div>
            </React.Suspense>
        )
    }
    const _showCreateDistrinution = (handleChange, values, touched, errors) => {
        return (
            <React.Suspense>
                <ModalWindowCreate show={createDistributionShow} handleClose={handleCreateClose} onCreateDistribution={onCreateDistribution} />{
                    values.distributedPlans.map((plan, index) =>
                        <React.Suspense key={plan.id}>
                            <h4>{plans[index].speciality.directionName ?? plans[index].speciality.fullName} (Набор {plans[index].count} человек, проходной балл {plans[index].passingScore})</h4>
                            <CreateDistributionPlanList planIndex={index} plan={plans[index]} distributedPlan={plan} errors={errors.distributedPlans ? errors.distributedPlans[index] : {}} handleChange={handleChange} />
                            {index !== (distributedPlans.length - 1) ? <hr /> : null}
                        </React.Suspense>)}
                <div className="text-center pt-4">
                    <Button type="submit" size="lg" variant="outline-success">Подтвердить</Button>
                    <Link type="button" className="btn btn-outline-danger btn-lg ms-2" to={"/Faculties/" + facultyShortName + "/" + groupId}>Отмена</Link>
                </div>
            </React.Suspense>
        );
    }

    React.useEffect(() => {
        if (!plans && !group) {
            loadGroup();
            loadPlans();
        }
        if (plans) {
            initializationSelectedStudents(plans);
        }
    }, [plans])

    if (!group || !plans || !distributedPlans) {
        return <React.Suspense>
            <Placeholder as="h1" animation="glow" className="text-center"><Placeholder xs={12} /></Placeholder>
            <hr />
            <div className="ps-lg-4 pe-lg-4 position-relative">{
                [1, 2, 3].map((item, index) =>
                    <React.Suspense key={item}>
                        <Placeholder as="h4" animation="glow"><Placeholder xs={8} /></Placeholder>
                        <TablePreloader />
                        {index !== 2 ? <hr /> : null}
                    </React.Suspense>)}
                <hr />
            </div>
        </React.Suspense>
    }
    else {
        return (
            <React.Suspense>
                <h1 className="text-center">{group.name}</h1>
                <hr />
                <div className="ps-lg-4 pe-lg-4 position-relative">
                    <Formik
                        validationSchema={DistributionValidationSchema}
                        onSubmit={handleCreateSubmit}
                        initialValues={{ distributedPlans }}>
                        {({ handleSubmit, handleChange, values, touched, errors }) => (
                            <Form noValidate onSubmit={handleSubmit}>
                                <Form.Group style={{ textAlign: "-webkit-center" }}>
                                    <Form.Control className="p-0 d-none" plaintext readOnly isInvalid={modelErrors ? !!modelErrors : false} />
                                    <Form.Control.Feedback type="invalid">{modelErrors ? _formGroupErrors(modelErrors) : ""}</Form.Control.Feedback>
                                </Form.Group>
                                {!isDistributed ? _showCreateDistrinution(handleChange, values, touched, errors) : _showConfirmDistrinution()}
                            </Form >
                        )}
                    </Formik>
                </div>
            </React.Suspense>
        );
    }
}