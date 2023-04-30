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
import { useParams } from 'react-router-dom'
import React, { useState } from 'react';

export default function EditModalWindow({ show, handleClose, onLoadGroups, groupId }) {
    const params = useParams();
    const facultyShortName = params.shortName;

    const [isLoaded, setIsLoaded] = useState(false);
    const [group, setGroup] = useState(null);
    const [specialities, setSpecialities] = useState([]);
    const [groupSpecialities, setGroupSpecialities] = useState(null);
    const [selectedSpecialities, setSelectedSpecialities] = useState(null);
    const [subjects, setSubjects] = useState([]);
    const [groupSubjects, setGroupSubjects] = useState(null);
    const [selectedSubjects, setSelectedSubjects] = useState(null);

    const handleSubmit = (values) => {
        onEditGroup(values);
    }
    const setDefaultValues = () => {
        setIsLoaded(false);
        setGroupSubjects(null);
        setSelectedSubjects(null);
        setGroupSpecialities(null);
        setSelectedSpecialities(null);
        setGroup(null);
    }
    const onEditGroup = async (values) => {
        await GroupsOfSpecialitiesService.httpPut(facultyShortName, values);
        handleClose();
        onLoadGroups();
        setDefaultValues();
    }
    const loadData = async () => {
        const groupData = GroupsOfSpecialitiesService.httpGetById(groupId);
        const facultySpecialitiesData = SpecialitiesService.httpGetFacultySpecialities(facultyShortName);
        const groupSpecialitiesData = SpecialitiesService.httpGetGroupSpecialities(groupId);
        const subjectsData = SubjectsService.httpGet();
        const groupSubjectsData = SubjectsService.httpGetGroupSubjects(groupId);

        setGroup(await groupData);
        setSpecialities(await facultySpecialitiesData);
        setGroupSpecialities(await groupSpecialitiesData);
        setSubjects(await subjectsData);
        setGroupSubjects(await groupSubjectsData);
    }
    React.useEffect(() => {
        if (groupId) {
            if (!isLoaded) {
                loadData();
                setIsLoaded(true);
            }
            if (specialities.length > 0 && groupSpecialities && !selectedSpecialities) {
                setSelectedSpecialities(specialities.map(item => { return { specialityName: item.directionName ?? item.fullName, specialityId: item.id, isSelected: groupSpecialities.some(gS => gS.id === item.id) } }))
            }
            if (subjects.length > 0 && groupSubjects && !selectedSubjects) {
                setSelectedSubjects(subjects.map(item => { return { subject: item.name, subjectId: item.id, isSelected: groupSubjects.some(gS => gS.id === item.id) } }))
            }
        }
        else {
            setDefaultValues();
        }
    }, [subjects, specialities, groupSubjects, groupSpecialities, groupId])

    if (!group || !selectedSubjects || !selectedSpecialities) {
        return <ModalWindowPreloader show={show} handleClose={handleClose} />;
    }
    else {
        return (
            <Modal size="xl" fullscreen="lg-down" show={show} onHide={handleClose} backdrop="static" keyboard={false}>
                <Formik
                    validationSchema={GroupOfSpecialitiesValidationSchema}
                    onSubmit={handleSubmit}
                    initialValues={{ group, selectedSpecialities, selectedSubjects }}>
                    {({ handleSubmit, handleChange, values, touched, errors }) => (
                        <Form noValidate onSubmit={handleSubmit}>
                            <Modal.Header closeButton>
                                <Modal.Title>Изменить группу</Modal.Title>
                            </Modal.Header>
                            <Modal.Body>
                                <UpdateGroupOfSpeciality onChangeModel={handleChange} values={values} errors={errors} />
                            </Modal.Body>
                            <Modal.Footer>
                                <Button variant="secondary" onClick={handleClose}>Закрыть</Button>
                                <Button type="submit" variant="primary">Сохранить</Button>
                            </Modal.Footer>
                        </Form >
                    )}
                </Formik>
            </Modal>
        );
    }
}
