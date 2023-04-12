import UpdateAdmissionSpetialitiesPriority from "./UpdateAdmissionSpetialitiesPriority"
import UpdateAdmissionSubjectScores from "./UpdateAdmissionSubjectScores"

import React, { useState } from 'react';
import Form from 'react-bootstrap/Form';
import Col from 'react-bootstrap/Col';
import Row from 'react-bootstrap/Row';

export default function UpdateAdmission({ admission, student, specialitiesPriority, studentScores, errors, onChangeModel }) {
    const [updatedAdmission, setUpdatedAdmission] = useState({
        id: admission.id,
        dateOfApplication: admission.dateOfApplication
    });
    const [updatedStudent, setUpdatedStudent] = useState({
        id: student.id, gps: student.gps,
        name: student.name, surname: student.surname, patronymic: student.patronymic
    });
    const [updatedSpecialitiesPriority, setUpdatedSpecialitiesPriority] = useState(specialitiesPriority);
    const [updatedStudentScores, setUpdatedStudentScores] = useState(studentScores);

    const { admissionId, dateOfApplication } = updatedAdmission;
    const { studentId, gps, name, surname, patronymic } = updatedStudent;

    const handleChangeStudentScores = (studentScores) => {
        setUpdatedStudentScores(studentScores);
    }
    const handleChangeSpecialitiesPriority = (specialitiesPriority) => {
        setUpdatedSpecialitiesPriority(specialitiesPriority);
    }
    const handleChangeAdmission = (event) => {
        const { name, value } = event.target;
        setUpdatedAdmission((prevState) => {
            return {
                ...prevState,
                [name]: value,
            };
        });
    }
    const handleChangeStudent = (event) => {
        const { name, value } = event.target;
        setUpdatedStudent((prevState) => {
            return {
                ...prevState,
                [name]: value,
            };
        });
    }
    const _formGroupErrors = (errors) => {
        if (errors) {
            return (<React.Suspense>{
                errors.map((error) =>
                    <React.Suspense key={error}><span>{error}</span><br></br></React.Suspense>
                )}
            </React.Suspense>);
        }
    }
    React.useEffect(() => {
        onChangeModel(updatedAdmission, updatedStudent, updatedStudentScores, updatedSpecialitiesPriority);
    }, [updatedAdmission, updatedStudent, updatedStudentScores, updatedSpecialitiesPriority]);
    return (
        <React.Suspense>
            <Row>
                <Form.Group as={Col} sm={4}>
                    <Form.Label className="mb-0">Фамилия</Form.Label><sup>*</sup>
                    <Form.Control type="text"
                        required name="surname" value={surname ?? ""} onChange={handleChangeStudent}
                        isInvalid={errors ? !!errors["Student.Surname"] : false} />
                    <Form.Control.Feedback type="invalid">{errors ? _formGroupErrors(errors["Student.Surname"]) : ""}</Form.Control.Feedback>
                </Form.Group>
                <Form.Group as={Col} sm={4}>
                    <Form.Label className="mb-0">Имя</Form.Label><sup>*</sup>
                    <Form.Control type="text"
                        required name="name" value={name ?? ""} onChange={handleChangeStudent}
                        isInvalid={errors ? !!errors["Student.Name"] : false} />
                    <Form.Control.Feedback type="invalid">{errors ? _formGroupErrors(errors["Student.Name"]) : ""}</Form.Control.Feedback>
                </Form.Group>
                <Form.Group as={Col} sm={4}>
                    <Form.Label className="mb-0">Отчество</Form.Label><sup>*</sup>
                    <Form.Control type="text"
                        required name="patronymic" value={patronymic ?? ""} onChange={handleChangeStudent}
                        isInvalid={errors ? !!errors["Student.Patronymic"] : false} />
                    <Form.Control.Feedback type="invalid">{errors ? _formGroupErrors(errors["Student.Patronymic"]) : ""}</Form.Control.Feedback>
                </Form.Group>
            </Row>
            <Row>
                <Form.Group as={Col} sm={8} className="mt-2">
                    <Form.Label>Подача заявки</Form.Label><sup>*</sup>
                    <Form.Control type="datetime-local"
                        required name="dateOfApplication" value={dateOfApplication ?? ""} onChange={handleChangeAdmission}
                        isInvalid={errors ? !!errors.dateOfApplication : false} />
                    <Form.Control.Feedback type="invalid">{errors ? _formGroupErrors(errors.dateOfApplication) : ""}</Form.Control.Feedback>
                </Form.Group>
                <Form.Group as={Col} sm={4} className="mt-2">
                    <Form.Label>Сумма баллов аттестата</Form.Label><sup>*</sup>
                    <Form.Control type="number" min={0}
                        required name="gps" value={gps ?? ""} onChange={handleChangeStudent}
                        isInvalid={errors ? !!errors["Student.GPS"] : false} />
                    <Form.Control.Feedback type="invalid">{errors ? _formGroupErrors(errors["Student.GPS"]) : ""}</Form.Control.Feedback>
                </Form.Group>
            </Row>
            <Row>
                <Col sm={4} className="mt-3">
                    <UpdateAdmissionSubjectScores onChangeModel={handleChangeStudentScores} studentScores={studentScores} modelErrors={errors ? errors : null} />
                </Col>
                <Col sm={8} className="mt-3">
                    <UpdateAdmissionSpetialitiesPriority onChangeModel={handleChangeSpecialitiesPriority} specialitiesPriority={updatedSpecialitiesPriority} modelErrors={errors ? errors.SpecialitiesPriority : null} />
                </Col>
            </Row>
        </React.Suspense>
    );
}