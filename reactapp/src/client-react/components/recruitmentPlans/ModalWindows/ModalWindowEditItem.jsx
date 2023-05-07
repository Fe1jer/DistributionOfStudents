import ModalWindowPreloader from "../../ModalWindowPreloader";

import RecruitmentPlansServise from "../../../services/RecruitmentPlans.service";

import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';

import { RecruitmentPlanValidationSchema } from '../../../validations/RecruitmentPlans.validation';
import UpdatePlanItem from "../UpdatePlanItem.jsx";

import { Formik } from 'formik';
import React, { useState } from 'react';

export default function EditModalWindowItem({ show, handleClose, planId, onLoadPlans }) {
    const [plan, setPlan] = useState(null);

    const handleSubmit = (values) => {
        onEditPlan(values);
    }
    const onEditPlan = async (values) => {
        if (planId !== null) {
            await RecruitmentPlansServise.httpPutById(planId, values);
            handleClose();
            onLoadPlans();
        }
    }
    const getPlanById = async () => {
        var data = await RecruitmentPlansServise.httpGetById(planId);
        setPlan(data);
    }

    React.useEffect(() => {
        if (planId && show) {
            getPlanById();
        }
        else {
            setPlan(null);
        }
    }, [planId]);

    if (!plan) {
        return <ModalWindowPreloader show={show} handleClose={handleClose} />;
    }
    else {
        console.log(plan);
        return (
            <Modal show={show} onHide={handleClose} backdrop="static" keyboard={false}>
                <Formik
                    validationSchema={RecruitmentPlanValidationSchema}
                    onSubmit={handleSubmit}
                    initialValues={{ count: plan.count, target: plan.target, planId: plan.id }}>
                    {({ handleSubmit, handleChange, values, touched, errors }) => (
                        <Form noValidate onSubmit={handleSubmit}>
                            <Modal.Header closeButton>
                                <Modal.Title as="h5">Изменить план <b className="text-success">"{plan.speciality.directionName ?? plan.speciality.fullName}"</b></Modal.Title>
                            </Modal.Header>
                            <Modal.Body>
                                <UpdatePlanItem values={values} handleChange={handleChange} errors={errors} />
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
