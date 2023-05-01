import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';

import GroupsOfSpecialitiesService from '../../../services/GroupsOfSpecialities.service.js';
import SpecialitiesService from "../../../services/Specialities.service";
import SubjectsService from "../../../services/Subjects.service.js";

import UpdateGroupOfSpeciality from '../UpdateGroupOfSpeciality.jsx';
import { GroupOfSpecialitiesValidationSchema } from "../../../validations/GroupOfSpecialities.validation";

import ModalWindowPreloader from "../../ModalWindowPreloader";

import { Formik } from 'formik';
import React, { useState } from 'react';
import { useParams } from 'react-router-dom'

import { getToday } from "../../../../../src/js/datePicker.js"

export default function CreateModalWindow({ show, handleClose, onLoadGroups }) {
    const params = useParams();
    const facultyShortName = params.shortName;
    const defaultForm = {
        id: 0,
        year: 0,
        isDailyForm: true,
        isBudget: true,
        isFullTime: true
    }
    const defaultGroup = {
        id: 0,
        name: null,
        startDate: getToday(),
        enrollmentDate: getToday(),
        description: null,
        isCompleted: false
    }
    defaultGroup.formOfEducation = defaultForm

    const [isLoaded, setIsLoaded] = useState(false);
    const [specialities, setSpecialities] = useState([]);
    const [selectedSpecialities, setSelectedSpecialities] = useState([]);
    const [subjects, setSubjects] = useState([]);
    const [selectedSubjects, setSelectedSubjects] = useState([]);

    const handleSubmit = (values) => {
        onCreateGroup(values);
    }
    const loadSpecialities = async () => {
        const specialitiesData = await SpecialitiesService.httpGetFacultySpecialities(facultyShortName);
        setSpecialities(specialitiesData);
    }
    const loadSubjects = async () => {
        const subjectsData = await SubjectsService.httpGet();
        setSubjects(subjectsData);
    }
    const onCreateGroup = async (values) => {
        await GroupsOfSpecialitiesService.httpPost(facultyShortName, values);
        handleClose();
        onLoadGroups();
        setIsLoaded(false);
    }
    const loadData = () => {
        loadSpecialities();
        loadSubjects();
    }

    React.useEffect(() => {
        if (!isLoaded) {
            loadData();
            setIsLoaded(true);
        }
        if (specialities.length > 0 && selectedSpecialities.length === 0) {
            setSelectedSpecialities(specialities.map(item => { return { specialityName: item.directionName ?? item.fullName, specialityId: item.id, isSelected: false } }))
        }
        if (subjects.length > 0 && selectedSubjects.length === 0) {
            setSelectedSubjects(subjects.map(item => { return { subject: item.name, subjectId: item.id, isSelected: false } }))
        }
    }, [subjects, specialities, show])

    if (selectedSpecialities.length === 0 || selectedSubjects.length === 0) {
        return <ModalWindowPreloader show={show} handleClose={handleClose} />;
    }
    else {
        return (
            <Modal size="xl" fullscreen="lg-down" show={show} onHide={handleClose} backdrop="static" keyboard={false}>
                <Formik
                    validationSchema={GroupOfSpecialitiesValidationSchema}
                    onSubmit={handleSubmit}
                    initialValues={{ group: defaultGroup, selectedSpecialities, selectedSubjects }}>
                    {({ handleSubmit, handleChange, values, touched, errors }) => (
                        <Form noValidate onSubmit={handleSubmit}>
                            <Modal.Header closeButton>
                                <Modal.Title>Создать группу</Modal.Title>
                            </Modal.Header>
                            <Modal.Body>
                                <UpdateGroupOfSpeciality onChangeModel={handleChange} values={values} errors={errors} />
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
