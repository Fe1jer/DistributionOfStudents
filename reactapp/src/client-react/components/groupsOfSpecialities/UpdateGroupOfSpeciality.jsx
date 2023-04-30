import SelectedGroupSubjects from "./SelectedGroupSubjects.jsx";
import SelectedGroupSpecialities from "./SelectedGroupSpecialities.jsx";

import React from 'react';
import Form from 'react-bootstrap/Form';
import Col from 'react-bootstrap/Col';
import Row from 'react-bootstrap/Row';
import ToggleButton from 'react-bootstrap/ToggleButton';

import { Field } from 'formik';

import "../../../css/slider-radio.css";

export default function UpdateGroupOfSpeciality({ onChangeModel, values, errors }) {
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
                    <Field name="group.formOfEducation.isDailyForm">
                        {({ form }) => (
                            <Form.Group as={Col} sm="4" className="slider-radio elegant">
                                <ToggleButton
                                    id="isDaily" type="radio"
                                    value={JSON.parse(true)} checked={values.group.formOfEducation.isDailyForm}
                                    onChange={() => form.setFieldValue("group.formOfEducation.isDailyForm", true)}
                                    variant="empty" bsPrefix="empty" >
                                    Дневная
                                </ToggleButton>
                                <ToggleButton
                                    id="isEvening" type="radio"
                                    value={JSON.parse(false)} checked={!values.group.formOfEducation.isDailyForm}
                                    onChange={() => form.setFieldValue("group.formOfEducation.isDailyForm", false)}
                                    variant="empty" bsPrefix="empty" >
                                    Заочная
                                </ToggleButton>
                            </Form.Group>
                        )}
                    </Field>
                    <Field name="group.formOfEducation.isBudget">
                        {({ form }) => (
                            <Form.Group as={Col} sm="4" className="slider-radio elegant">
                                <ToggleButton
                                    id="isBudget" type="radio"
                                    value={1} checked={values.group.formOfEducation.isBudget}
                                    onChange={() => form.setFieldValue("group.formOfEducation.isBudget", true)}
                                    variant="empty" bsPrefix="empty" >
                                    Бюджет
                                </ToggleButton>
                                <ToggleButton
                                    id="isPaid" type="radio"
                                    value={0} checked={!values.group.formOfEducation.isBudget}
                                    onChange={() => form.setFieldValue("group.formOfEducation.isBudget", false)}
                                    variant="empty" bsPrefix="empty">
                                    Платное
                                </ToggleButton>
                            </Form.Group>
                        )}
                    </Field>
                    <Field name="group.formOfEducation.isFullTime">
                        {({ form }) => (
                            <Form.Group as={Col} sm="4" className="slider-radio elegant">
                                <ToggleButton
                                    id="isFullTime" type="radio"
                                    value={JSON.parse(true)} checked={values.group.formOfEducation.isFullTime}
                                    onChange={() => form.setFieldValue("group.formOfEducation.isFullTime", true)}
                                    variant="empty" bsPrefix="empty" >
                                    Полный
                                </ToggleButton>
                                <ToggleButton
                                    id="isAbbreviated" type="radio"
                                    value={JSON.parse(false)} checked={!values.group.formOfEducation.isFullTime}
                                    onChange={() => form.setFieldValue("group.formOfEducation.isFullTime", false)}
                                    variant="empty" bsPrefix="empty" >
                                    Сокращённый
                                </ToggleButton>
                            </Form.Group>
                        )}
                    </Field>
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