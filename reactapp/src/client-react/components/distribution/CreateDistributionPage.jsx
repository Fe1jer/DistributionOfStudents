import DistributionApi from '../../api/DistributionApi.js';
import GroupsOfSpecialitiesService from '../../services/GroupsOfSpecialities.service.js';
import ModalWindowConfirm from "./ModalWindows/ModalWindowConfirm.jsx";
import ModalWindowCreate from "./ModalWindows/ModalWindowCreate.jsx";

import ConfirmDistributionPlan from './ConfirmDistributionPlan.jsx';
import CreateDistributionPlanList from './CreateDistributionPlanList.jsx';

import Button from 'react-bootstrap/Button';
import Form from 'react-bootstrap/Form';
import Placeholder from 'react-bootstrap/Placeholder';

import TablePreloader from "../TablePreloader.jsx";

import React, { useState } from 'react';
import { Link, useNavigate, useParams } from 'react-router-dom';

export default function CreateDistributionPage() {
    const params = useParams();
    const facultyShortName = params.shortName;
    const groupId = params.groupId;
    const navigate = useNavigate();

    const [group, setGroup] = useState(null);
    const [plans, setPlans] = useState(null);
    const [isDistributed, setIsDistributed] = useState(false);
    const [createDistributionShow, setCreateDistributionShow] = useState(false);
    const [confirmDistributionShow, setConfirmDistributionShow] = useState(false);
    const [selectedStudents, setSelectedStudents] = useState(null);
    const [errors, setErrors] = useState({});
    const [modelErrors, setModelErrors] = useState(null);
    const [validated, setValidated] = useState(false);
    const [isInitSelectedStudents, setIsInitSelectedStudents] = useState(false);

    const handleCreateSubmit = (e) => {
        e.preventDefault();
        setCreateDistributionShow(true);
        setValidated(true);
    }

    const setDefaultValues = () => {
        setValidated(false);
        setCreateDistributionShow(false);
        setConfirmDistributionShow(false);
        setIsInitSelectedStudents(false);
        setPlans(null);
        setSelectedStudents(null);
        setErrors(null);
        setModelErrors(null);
    }
    const loadGroup = async () => {
        const groupData = await GroupsOfSpecialitiesService.httpGetById(groupId);
        setGroup(groupData);
    }
    const loadPlans = () => {
        var xhr = new XMLHttpRequest();
        xhr.open("get", DistributionApi.getDistributionUrl(facultyShortName, groupId), true);
        xhr.onload = function () {
            var data = JSON.parse(xhr.responseText);
            setIsDistributed(!data.areControversialStudents);
            setPlans(data.plans);
        }.bind(this);
        xhr.send();
    }

    const onCreateDistribution = () => {
        setCreateDistributionShow(false);
        var xhr = new XMLHttpRequest();
        xhr.open("post", DistributionApi.getPostCreateUrl(facultyShortName, groupId), true);
        xhr.setRequestHeader("Content-Type", "application/json")
        xhr.onload = function () {
            setErrors(null);
            setModelErrors(null);
            if (xhr.status === 200) {
                setDefaultValues();
                var data = JSON.parse(xhr.responseText);
                setIsDistributed(!data.areControversialStudents);
                setPlans(data.plans);
                initializationSelectedStudents(data.plans);
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
        xhr.send(JSON.stringify(selectedStudents));
    }

    const onConfirmDistribution = () => {
        setConfirmDistributionShow(false);
        var xhr = new XMLHttpRequest();
        xhr.open("post", DistributionApi.getPostConfirmUrl(facultyShortName, groupId), true);
        xhr.setRequestHeader("Content-Type", "application/json")
        xhr.onload = function () {
            if (xhr.status === 200) {
                navigate("/Faculties/" + facultyShortName + "/" + groupId);
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
        xhr.send(JSON.stringify(selectedStudents));
    }

    const onChangeSelectedStudents = (index, updatedSelectedStudents) => {
        var updateSelectedStudentsTemp = selectedStudents;
        updateSelectedStudentsTemp[index].distributedStudents = updatedSelectedStudents;
        setSelectedStudents(updateSelectedStudentsTemp);
    }

    const initializationSelectedStudents = (plans) => {
        if (!isInitSelectedStudents) {
            setIsInitSelectedStudents(true);
            var initializatedSelectedStudents = plans.map((item) => {
                return { planId: item.id, planCount: item.count, passingScore: item.passingScore, distributedStudents: null }
            });
            setSelectedStudents(initializatedSelectedStudents);
        }
    };


    const handleConfirmSubmit = () => {
        setConfirmDistributionShow(true);
    }
    const handleCreateClose = () => {
        setCreateDistributionShow(false);
    };
    const handleConfirmClose = () => {
        setConfirmDistributionShow(false);
    };

    const _showContent = () => {
        if (!isDistributed) {
            return _showCreateDistrinution();
        }
        else {
            return _showConfirmDistrinution();
        }
    }

    const _formGroupErrors = (errors) => {
        if (errors) {
            return (<React.Suspense>{
                errors.map((error) => <React.Suspense key={error}><span>{error}</span><br></br></React.Suspense>)}
            </React.Suspense>);
        }
    }

    const _showConfirmDistrinution = () => {
        return (
            <React.Suspense>
                <Form noValidate validated={validated} onSubmit={handleCreateSubmit}>
                    <Form.Group style={{ textAlign: "-webkit-center" }}>
                        <Form.Control className="p-0 d-none" plaintext readOnly isInvalid={modelErrors ? !!modelErrors : false} />
                        <Form.Control.Feedback type="invalid">{modelErrors ? _formGroupErrors(modelErrors) : ""}</Form.Control.Feedback>
                    </Form.Group>
                    <ModalWindowConfirm show={confirmDistributionShow} handleClose={handleConfirmClose} onConfirmDistribution={onConfirmDistribution} />{
                        plans.map((plan, index) =>
                            <React.Suspense key={plan.speciality.directionName ?? plan.speciality.fullName}>
                                <ConfirmDistributionPlan index={index} plan={plan} onChangeSelectedStudents={onChangeSelectedStudents} />
                                {index !== plans.length - 1 ? <hr /> : null}
                            </React.Suspense>
                        )}
                    <div className="text-center pt-4">
                        <Button type="submit" size="lg" variant="outline-success" onClick={() => handleConfirmSubmit()}>Подтвердить</Button>
                        <Link type="button" className="btn btn-outline-danger btn-lg ms-2" to={"/Faculties/" + facultyShortName + "/" + groupId}>Отмена</Link>
                    </div>
                </Form >
            </React.Suspense>
        )
    }

    const _showCreateDistrinution = () => {
        return (
            <Form noValidate validated={validated} onSubmit={handleCreateSubmit}>
                <Form.Group style={{ textAlign: "-webkit-center" }}>
                    <Form.Control className="p-0 d-none" plaintext readOnly isInvalid={modelErrors ? !!modelErrors : false} />
                    <Form.Control.Feedback type="invalid">{modelErrors ? _formGroupErrors(modelErrors) : ""}</Form.Control.Feedback>
                </Form.Group>
                <ModalWindowCreate show={createDistributionShow} handleClose={handleCreateClose} onCreateDistribution={onCreateDistribution} />{
                    plans.map((plan, index) =>
                        <React.Suspense key={plan.speciality.directionName ?? plan.speciality.fullName}>
                            <h4>{plan.speciality.directionName ?? plan.speciality.fullName} (Набор {plan.count} человек, проходной балл {plan.passingScore})</h4>
                            <CreateDistributionPlanList index={index} plan={plan} error={errors ? errors['[' + index + ']'] : null} onChangeSelectedStudents={onChangeSelectedStudents} />
                            {index != (plans.length - 1) ? <hr /> : null}
                        </React.Suspense>)}
                <div className="text-center pt-4">
                    <Button type="submit" size="lg" variant="outline-success">Подтвердить</Button>
                    <Link type="button" className="btn btn-outline-danger btn-lg ms-2" to={"/Faculties/" + facultyShortName + "/" + groupId}>Отмена</Link>
                </div>
            </Form >
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

    if (!group || !plans || !selectedStudents) {
        return <React.Suspense>
            <Placeholder as="h1" animation="glow" className="text-center"><Placeholder xs={12} /></Placeholder>
            <hr />
            <div className="ps-lg-4 pe-lg-4 position-relative">{
                [1, 2, 3].map((item, index) =>
                    <React.Suspense key={item}>
                        <Placeholder as="h4" animation="glow"><Placeholder xs={8} /></Placeholder>
                        <TablePreloader />
                        {index != 2 ? <hr /> : null}
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
                    {_showContent()}
                </div>
            </React.Suspense>
        );
    }
}