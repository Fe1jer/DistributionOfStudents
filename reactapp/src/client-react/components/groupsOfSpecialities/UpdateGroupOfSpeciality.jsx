import SelectedGroupSubjects from "./SelectedGroupSubjects.jsx";
import SelectedGroupSpecialities from "./SelectedGroupSpecialities.jsx";

import React, { useEffect } from 'react';
import Form from 'react-bootstrap/Form';
import Col from 'react-bootstrap/Col';
import Row from 'react-bootstrap/Row';
import ToggleButton from 'react-bootstrap/ToggleButton';

import { useFormikContext } from 'formik';

import "../../../css/slider-radio.css";
import { getNameByProps } from "../../../js/groupNames.js";

export default function UpdateGroupOfSpeciality({ onChangeModel, values, errors }) {
    const formikProps = useFormikContext();

    useEffect(() => {
        const _setGroupName = () => {
            const name = getNameByProps(values.group.formOfEducation.isDailyForm, values.group.formOfEducation.isBudget, values.group.formOfEducation.isFullTime);
            formikProps.setFieldValue("group.name", name);
        }

        _setGroupName();
    }, [values.group.formOfEducation])

    return (
        <React.Suspense>
            <Form.Group className="px-2">
                <Form.Label className="mb-0">Название</Form.Label><sup>*</sup>
                <Form.Control type="text"
                    required name="group.name" value={values.group.name ?? ""} onChange={onChangeModel}
                    isInvalid={errors.group ? !!errors.group.name : false} />
                <Form.Control.Feedback type="invalid">{errors.group ? errors.group.name : null}</Form.Control.Feedback>
            </Form.Group>
            <Row className="mt-2 px-2">
                <Form.Group as={Col} sm="6">
                    <Form.Label className="mb-0">Начало</Form.Label><sup>*</sup>
                    <Form.Control type="date"
                        required name="group.startDate" value={values.group.startDate.split('T')[0] ?? ""} onChange={onChangeModel}
                        isInvalid={errors.group ? !!errors.group.startDate : false} />
                    <Form.Control.Feedback type="invalid">{errors.group ? errors.group.startDate : null}</Form.Control.Feedback>
                </Form.Group>
                <Form.Group as={Col} sm="6">
                    <Form.Label className="mb-0">Окончание</Form.Label><sup>*</sup>
                    <Form.Control type="date"
                        required name="group.enrollmentDate" value={values.group.enrollmentDate.split('T')[0] ?? ""} onChange={onChangeModel}
                        isInvalid={errors.group ? !!errors.group.enrollmentDate : false} />
                    <Form.Control.Feedback type="invalid">{errors.group ? errors.group.enrollmentDate : null}</Form.Control.Feedback>
                </Form.Group>
                <Row className="mt-3 px-2 align-items-end text-center">
                    <Form.Group as={Col} sm="4" className="slider-radio elegant">
                        <ToggleButton
                            id="isDaily" type="radio"
                            checked={values.group.formOfEducation.isDailyForm}
                            onChange={() => formikProps.setFieldValue("group.formOfEducation.isDailyForm", true)}
                            variant="empty" bsPrefix="empty" >
                            Дневная
                        </ToggleButton>
                        <ToggleButton
                            id="isEvening" type="radio"
                            checked={!values.group.formOfEducation.isDailyForm}
                            onChange={() => formikProps.setFieldValue("group.formOfEducation.isDailyForm", false)}
                            variant="empty" bsPrefix="empty" >
                            Заочная
                        </ToggleButton>
                    </Form.Group>
                    <Form.Group as={Col} sm="4" className="slider-radio elegant">
                        <ToggleButton
                            id="isBudget" type="radio"
                            checked={values.group.formOfEducation.isBudget}
                            onChange={() => formikProps.setFieldValue("group.formOfEducation.isBudget", true)}
                            variant="empty" bsPrefix="empty" >
                            Бюджет
                        </ToggleButton>
                        <ToggleButton
                            id="isPaid" type="radio"
                            checked={!values.group.formOfEducation.isBudget}
                            onChange={() => formikProps.setFieldValue("group.formOfEducation.isBudget", false)}
                            variant="empty" bsPrefix="empty">
                            Платное
                        </ToggleButton>
                    </Form.Group>
                    <Form.Group as={Col} sm="4" className="slider-radio elegant">
                        <ToggleButton
                            id="isFullTime" type="radio"
                            checked={values.group.formOfEducation.isFullTime}
                            onChange={() => formikProps.setFieldValue("group.formOfEducation.isFullTime", true)}
                            variant="empty" bsPrefix="empty" >
                            Полный
                        </ToggleButton>
                        <ToggleButton
                            id="isAbbreviated" type="radio"
                            checked={!values.group.formOfEducation.isFullTime}
                            onChange={() => formikProps.setFieldValue("group.formOfEducation.isFullTime", false)}
                            variant="empty" bsPrefix="empty" >
                            Сокращённый
                        </ToggleButton>
                    </Form.Group>
                </Row>
            </Row>
            <Row>
                <Col xl="4">
                    <SelectedGroupSubjects errors={errors ? errors.selectedSubjects : null} subjects={values.selectedSubjects} onChange={onChangeModel} />
                </Col >
                <Col xl="8">
                    <SelectedGroupSpecialities errors={errors ? errors.selectedSpecialities : null} specialities={values.selectedSpecialities} onChange={onChangeModel} />
                </Col>
            </Row>
        </React.Suspense>
    );
}