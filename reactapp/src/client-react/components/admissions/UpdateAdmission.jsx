import UpdateAdmissionSpetialitiesPriority from "./UpdateAdmissionSpetialitiesPriority"
import UpdateAdmissionSubjectScores from "./UpdateAdmissionSubjectScores"

import { Field } from 'formik';
import React from 'react';

import Form from 'react-bootstrap/Form';
import Col from 'react-bootstrap/Col';
import Row from 'react-bootstrap/Row';

export default function UpdateAdmission({ values, errors, onChangeModel }) {
    return (
        <React.Suspense>
            <Row>
                <Form.Group as={Col} sm={4}>
                    <Form.Label className="mb-0">Фамилия</Form.Label><sup>*</sup>
                    <Form.Control type="text"
                        name="student.surname" value={values.student.surname ?? ""} onChange={onChangeModel}
                        isInvalid={errors.student ? !!errors.student.surname : false} />
                    <Form.Control.Feedback type="invalid">{errors.student ? errors.student.surname : ""}</Form.Control.Feedback>
                </Form.Group>
                <Form.Group as={Col} sm={4}>
                    <Form.Label className="mb-0">Имя</Form.Label><sup>*</sup>
                    <Form.Control type="text"
                        name="student.name" value={values.student.name ?? ""} onChange={onChangeModel}
                        isInvalid={errors.student ? !!errors.student.name : false} />
                    <Form.Control.Feedback type="invalid">{errors.student ? errors.student.name : ""}</Form.Control.Feedback>
                </Form.Group>
                <Form.Group as={Col} sm={4}>
                    <Form.Label className="mb-0">Отчество</Form.Label><sup>*</sup>
                    <Form.Control type="text"
                        name="student.patronymic" value={values.student.patronymic ?? ""} onChange={onChangeModel}
                        isInvalid={errors.student ? !!errors.student.patronymic : false} />
                    <Form.Control.Feedback type="invalid">{errors.student ? errors.student.patronymic : ""}</Form.Control.Feedback>
                </Form.Group>
            </Row>
            <Row>
                <Form.Group as={Col} sm={8} className="mt-2">
                    <Form.Label className="mb-0">Почта</Form.Label>
                    <Form.Control type="text"
                        name="email" value={values.email ?? ""} onChange={onChangeModel}
                        isInvalid={errors ? !!errors.email : false} />
                    <Form.Control.Feedback type="invalid">{errors ? errors.email : ""}</Form.Control.Feedback>
                </Form.Group>
                <Form.Group as={Col} sm={4} className="mt-2">
                    <Form.Label className="mb-0">Подача заявки</Form.Label><sup>*</sup>
                    <Form.Control type="datetime-local"
                        name="dateOfApplication" value={values.dateOfApplication ?? ""} onChange={onChangeModel}
                        isInvalid={errors ? !!errors.dateOfApplication : false} />
                    <Form.Control.Feedback type="invalid">{errors ? errors.dateOfApplication : ""}</Form.Control.Feedback>
                </Form.Group>
            </Row>
            <Row className="mt-3">
                <h5 className="text-center">Паспорт</h5>
                <Form.Group as={Col} sm={6}>
                    <Form.Label className="mb-0">Идентификационный номер</Form.Label>
                    <Form.Control type="text"
                        name="passportID" value={values.passportID ?? ""} onChange={onChangeModel}
                        isInvalid={errors ? !!errors.passportID : false} />
                    <Form.Control.Feedback type="invalid">{errors ? errors.passportID : ""}</Form.Control.Feedback>
                </Form.Group>
                <Form.Group as={Col} sm={2}>
                    <Form.Label className="mb-0">Серия</Form.Label>
                    <Form.Control type="text"
                        name="passportSeries" value={values.passportSeries ?? ""} onChange={onChangeModel}
                        isInvalid={errors ? !!errors.passportSeries : false} />
                    <Form.Control.Feedback type="invalid">{errors ? errors.passportSeries : ""}</Form.Control.Feedback>
                </Form.Group>
                <Form.Group as={Col} sm={4}>
                    <Form.Label className="mb-0">Номер</Form.Label>
                    <Form.Control type="number"
                        name="passportNumber" value={values.passportNumber ?? ""} onChange={onChangeModel}
                        isInvalid={errors ? !!errors.passportNumber : false} />
                    <Form.Control.Feedback type="invalid">{errors ? errors.passportNumber : ""}</Form.Control.Feedback>
                </Form.Group>
            </Row>
            <Row className="mt-3">
                <Col sm={4}>
                    <h5 className="text-center">Аттестат</h5>
                    <Form.Group sm={4} className="pb-2">
                        <Form.Label className="mb-0">Сумма баллов</Form.Label><sup>*</sup>
                        <Form.Control type="number"
                            name="student.gps" value={values.student.gps ?? ""} onChange={onChangeModel}
                            isInvalid={errors.student ? !!errors.student.gps : false} />
                        <Form.Control.Feedback type="invalid">{errors.student ? errors.student.gps : ""}</Form.Control.Feedback>
                    </Form.Group>
                    <UpdateAdmissionSubjectScores onChangeModel={onChangeModel} studentScores={values.studentScores} errors={errors ? errors.studentScores : null} />
                </Col>
                <Col sm={8}>
                    <Field name="specialitiesPriority">
                        {({ form }) => (
                            <UpdateAdmissionSpetialitiesPriority form={form} specialitiesPriority={values.specialitiesPriority} errors={errors ? errors.specialitiesPriority : null} />
                        )}
                    </Field>
                </Col>
            </Row>
        </React.Suspense>
    );
}