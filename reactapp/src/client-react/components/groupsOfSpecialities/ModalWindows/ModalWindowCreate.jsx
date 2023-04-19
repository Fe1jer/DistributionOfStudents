import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';

import GroupsOfSpecialitiesApi from '../../../api/GroupsOfSpecialitiesApi.js';
import SpecialitiesApi from '../../../api/SpecialitiesApi.js';
import SubjectsService from "../../../services/Subjects.service.js";

import UpdateGroupOfSpeciality from '../UpdateGroupOfSpeciality.jsx';

import React, { useState } from 'react';
import { useParams } from 'react-router-dom'

import { getToday } from "../../../../../src/js/datePicker.js"

export default function CreateModalWindow({ show, handleClose, onLoadGroups }) {
    const params = useParams();
    const facultyShortName = params.shortName;
    const defaultForm = {
        id: 0,
        year: true,
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
    }

    const [isLoaded, setIsLoaded] = useState(false);
    const [group, setGroup] = useState(defaultGroup);
    const [form, setForm] = useState(defaultForm);
    const [specialities, setSpecialities] = useState([]);
    const [selectedSpecialities, setSelectedSpecialities] = useState([]);
    const [subjects, setSubjects] = useState([]);
    const [selectedSubjects, setSelectedSubjects] = useState([]);
    const [errors, setErrors] = useState({});
    const [validated, setValidated] = useState(false);

    const handleSubmit = (e) => {
        e.preventDefault();
        onCreateGroup();
        setValidated(true);
    }
    const setDefaultValues = () => {
        setValidated(false);
        setIsLoaded(false);
        setSelectedSubjects([]);
        setSelectedSpecialities([]);
        setForm(defaultForm);
        setGroup(defaultGroup);
    }
    const onChangeModel = (updatedGroup, updatedForm, updateSelectedSubjects, updateSelectedSpecialities) => {
        setSelectedSpecialities(updateSelectedSpecialities);
        setSelectedSubjects(updateSelectedSubjects);
        setGroup(updatedGroup);
        setForm(updatedForm);
    }
    const loadSpecialities = () => {
        var xhr = new XMLHttpRequest();
        xhr.open("get", SpecialitiesApi.getFacultySpecialitiesUrl(facultyShortName), true);
        xhr.onload = function () {
            var data = JSON.parse(xhr.responseText);
            setSpecialities(data);
        }.bind(this);
        xhr.send();
    }
    const loadSubjects = async () => {
        const subjectsData = await SubjectsService.httpGet();
        setSubjects(subjectsData);
    }
    const onCreateGroup = () => {
        group.formOfEducation = form
        var jsonData = {
            group,
            selectedSpecialities,
            selectedSubjects
        }
        setErrors(null);
        var xhr = new XMLHttpRequest();
        xhr.open("post", GroupsOfSpecialitiesApi.getPostUrl(facultyShortName), true);
        xhr.setRequestHeader("Content-Type", "application/json")
        xhr.onload = function () {
            if (xhr.status === 200) {
                setValidated(false);
                handleClose();
                onLoadGroups();
                setDefaultValues();
            }
            else if (xhr.status === 400) {
                var a = eval('({obj:[' + xhr.response + ']})');
                setErrors(a.obj[0].errors);
            }
        }.bind(this);
        xhr.send(JSON.stringify(jsonData));
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
        if (specialities.length > 0 && selectedSpecialities.length == 0) {
            setSelectedSpecialities(specialities.map(item => { return { specialityName: item.directionName ?? item.fullName, specialityId: item.id, isSelected: false } }))
        }
        if (subjects.length > 0 && selectedSubjects.length == 0) {
            setSelectedSubjects(subjects.map(item => { return { subject: item.name, subjectId: item.id, isSelected: false } }))
        }
    }, [subjects, specialities, show])

    if (selectedSpecialities.length === 0 || selectedSubjects.length === 0) {
        return (
            <Modal show={show} onHide={handleClose} backdrop="static" keyboard={false}>
                <Modal.Header closeButton>
                    <Modal.Title>Создать группу</Modal.Title>
                </Modal.Header>
                <Modal.Body className="text-center">
                    Загрузка...
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={handleClose}>Закрыть</Button>
                    <Button variant="primary">Сохранить</Button>
                </Modal.Footer>
            </Modal>
        );
    }
    else {
        return (
            <Modal size="xl" fullscreen="lg-down" show={show} onHide={handleClose} backdrop="static" keyboard={false}>
                <Form noValidate validated={validated} onSubmit={handleSubmit}>
                    <Modal.Header closeButton>
                        <Modal.Title>Создать группу</Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        <UpdateGroupOfSpeciality onChangeModel={onChangeModel} group={group} form={form} selectedSubjects={selectedSubjects} selectedSpecialities={selectedSpecialities} errors={errors} />
                    </Modal.Body>
                    <Modal.Footer>
                        <Button variant="secondary" onClick={handleClose}>Закрыть</Button>
                        <Button type="submit" variant="primary">Сохранить</Button>
                    </Modal.Footer>
                </Form >
            </Modal>
        );
    }
}
