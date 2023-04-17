import SelectedGroupSubjects from "./SelectedGroupSubjects.jsx";
import SelectedGroupSpecialities from "./SelectedGroupSpecialities.jsx";

import React from 'react';
import Form from 'react-bootstrap/Form';
import Col from 'react-bootstrap/Col';
import Row from 'react-bootstrap/Row';
import ToggleButton from 'react-bootstrap/ToggleButton';
import "../../../css/slider-radio.css";

export default function UpdateGroupOfSpeciality({ onChangeModel, group, form, selectedSubjects, selectedSpecialities, errors }) {
    const [updatedGroup, setUpdatedGroup] = React.useState({
        id: group.id,
        name: group.name,
        startDate: group.startDate,
        enrollmentDate: group.enrollmentDate,
        description: group.description
    });
    const [updatedForm, setUpdatedForm] = React.useState({
        id: 0,
        isDailyForm: form.isDailyForm,
        isBudget: form.isBudget,
        isFullTime: form.isFullTime,
    });
    const [updateSelectedSubjects, setUpdatedSelectedSubjects] = React.useState(selectedSubjects);
    const [updateSelectedSpecialities, setUpdatedSelectedSpecialities] = React.useState(selectedSpecialities);

    const { groupId, name, startDate, enrollmentDate, description } = updatedGroup;
    const { formId, isDailyForm, isBudget, isFullTime } = updatedForm;

    const handleGroupChange = (event) => {
        const { name, value } = event.target;
        setUpdatedGroup((prevState) => {
            return {
                ...prevState,
                [name]: value,
            };
        });
    }
    const handleFormChange = (event) => {
        const { name, value } = event.target;
        setUpdatedForm((prevState) => {
            return {
                ...prevState,
                [name]: value === 'true',
            };
        });
    }
    const handleSubjectsChange = (subjects) => {
        setUpdatedSelectedSubjects(subjects);
    }
    const handleSpecialitiesChange = (specialities) => {
        setUpdatedSelectedSpecialities(specialities);
    }
    React.useEffect(() => {
        if (updatedGroup && updatedForm && updateSelectedSubjects && updateSelectedSpecialities) {
            onChangeModel(updatedGroup, updatedForm, updateSelectedSubjects, updateSelectedSpecialities);
        }
    }, [updatedGroup, updatedForm, updateSelectedSubjects, updateSelectedSpecialities]);

    const _formGroupErrors = (errors) => {
        if (errors) {
            return (<React.Suspense>{
                errors.map((error) =>
                    <React.Suspense key={error}><span>{error}</span><br></br></React.Suspense>
                )}
            </React.Suspense>);
        }
    }

    return (
        <React.Suspense>
            <Form.Group className="px-2">
                <Form.Label className="mb-0">Название</Form.Label><sup>*</sup>
                <Form.Control type="text"
                    required name="name" value={name ?? ""} onChange={handleGroupChange}
                    isInvalid={errors ? !!errors["Group.Name"] : false} />
                <Form.Control.Feedback type="invalid">{errors ? _formGroupErrors(errors["Group.Name"]) : ""}</Form.Control.Feedback>
            </Form.Group>
            <Row className="mt-2 px-2">
                <Form.Group as={Col} sm="6">
                    <Form.Label className="mb-0">Начало</Form.Label><sup>*</sup>
                    <Form.Control type="date"
                        required name="startDate" value={startDate.split('T')[0] ?? ""} onChange={handleGroupChange}
                        isInvalid={errors ? !!errors["Group.StartDate"] : false} />
                    <Form.Control.Feedback type="invalid">{errors ? _formGroupErrors(errors["Group.StartDate"]) : ""}</Form.Control.Feedback>
                </Form.Group>
                <Form.Group as={Col} sm="6">
                    <Form.Label className="mb-0">Окончание</Form.Label><sup>*</sup>
                    <Form.Control type="date"
                        required name="enrollmentDate" value={enrollmentDate.split('T')[0] ?? ""} onChange={handleGroupChange}
                        isInvalid={errors ? !!errors["Group.EnrollmentDate"] : false} />
                    <Form.Control.Feedback type="invalid">{errors ? _formGroupErrors(errors["Group.EnrollmentDate"]) : ""}</Form.Control.Feedback>
                </Form.Group>
                <Row className="mt-3 px-2 align-items-end text-center">
                    <Form.Group as={Col} sm="4" className="slider-radio elegant">
                        <ToggleButton
                            id="IsDaily" type="radio" name="isDailyForm"
                            value="true" checked={isDailyForm} onChange={(e) => handleFormChange(e)}
                            variant="empty" bsPrefix="empty">
                            Дневная
                        </ToggleButton>
                        <ToggleButton
                            id="IsEvening" type="radio" name="isDailyForm"
                            value="false" checked={!isDailyForm} onChange={(e) => handleFormChange(e)}
                            variant="empty" bsPrefix="empty">
                            Заочная
                        </ToggleButton>
                    </Form.Group>
                    <Form.Group as={Col} sm="4" className="slider-radio elegant">
                        <ToggleButton
                            id="isBudget" type="radio" name="isBudget"
                            value="true" checked={isBudget} onChange={(e) => handleFormChange(e)}
                            variant="empty" bsPrefix="empty">
                            Бюджет
                        </ToggleButton>
                        <ToggleButton
                            id="IsPaid" type="radio" name="isBudget"
                            value="false" checked={!isBudget} onChange={(e) => handleFormChange(e)}
                            variant="empty" bsPrefix="empty">
                            Платное
                        </ToggleButton>
                    </Form.Group>
                    <Form.Group as={Col} sm="4" className="slider-radio elegant">
                        <ToggleButton
                            id="isFullTime" type="radio" name="isFullTime"
                            value="true" checked={isFullTime} onChange={(e) => handleFormChange(e)}
                            variant="empty" bsPrefix="empty">
                            Полный
                        </ToggleButton>
                        <ToggleButton
                            id="IsAbbreviated" type="radio" name="isFullTime"
                            value="false" checked={!isFullTime} onChange={(e) => handleFormChange(e)}
                            variant="empty" bsPrefix="empty">
                            Сокращённый
                        </ToggleButton>
                    </Form.Group>
                </Row>
            </Row>
            <Row>
                <Col xl="4">
                    <SelectedGroupSubjects modelErrors={errors ? errors.SelectedSubjects : null} subjects={updateSelectedSubjects} onChange={handleSubjectsChange} />
                </Col >
                <Col xl="8">
                    <SelectedGroupSpecialities modelErrors={errors ? errors.SelectedSpecialities : null} specialities={updateSelectedSpecialities} onChange={handleSpecialitiesChange} />
                </Col>
            </Row>
        </React.Suspense>
    );
}