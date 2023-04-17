import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';

import GroupsOfSpecialitiesApi from '../../../api/GroupsOfSpecialitiesApi.js';
import SpecialitiesApi from '../../../api/SpecialitiesApi.js';
import SubjectsApi from '../../../api/SubjectsApi.js';

import UpdateGroupOfSpeciality from '../UpdateGroupOfSpeciality.jsx';

import { useParams } from 'react-router-dom'
import React, { useState } from 'react';

export default function EditModalWindow({ show, handleClose, onLoadGroups, groupId }) {
    const params = useParams();
    const facultyShortName = params.shortName;

    const [isLoaded, setIsLoaded] = useState(false);
    const [form, setForm] = useState(null);
    const [group, setGroup] = useState(null);
    const [specialities, setSpecialities] = useState([]);
    const [groupSpecialities, setGroupSpecialities] = useState(null);
    const [selectedSpecialities, setSelectedSpecialities] = useState(null);
    const [subjects, setSubjects] = useState([]);
    const [groupSubjects, setGroupSubjects] = useState(null);
    const [selectedSubjects, setSelectedSubjects] = useState(null);
    const [errors, setErrors] = useState({});
    const [validated, setValidated] = useState(false);

    const handleSubmit = (e) => {
        e.preventDefault();
        onEditGroup();
        setValidated(true);
    }
    const setDefaultValues = () => {
        setValidated(false);
        setIsLoaded(false);
        setGroupSubjects(null);
        setSelectedSubjects(null);
        setGroupSpecialities(null);
        setSelectedSpecialities(null);
        setForm(null);
        setGroup(null);
    }
    const onChangeModel = (updatedGroup, updatedForm, updateSelectedSubjects, updateSelectedSpecialities) => {
        setSelectedSpecialities(updateSelectedSpecialities);
        setSelectedSubjects(updateSelectedSubjects);
        setGroup(updatedGroup);
        setForm(updatedForm);
    }
    const loadGroup = () => {
        var xhr = new XMLHttpRequest();
        xhr.open("get", GroupsOfSpecialitiesApi.getGroupUrl(groupId), true);
        xhr.onload = function () {
            var data = JSON.parse(xhr.responseText);
            setGroup(data);
            setForm(data.formOfEducation);
        }.bind(this);
        xhr.send();
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
    const loadGroupSpecialities = () => {
        var xhr = new XMLHttpRequest();
        xhr.open("get", SpecialitiesApi.getGroupSpecialitiesUrl(groupId), true);
        xhr.onload = function () {
            var data = JSON.parse(xhr.responseText);
            setGroupSpecialities(data);
        }.bind(this);
        xhr.send();
    }
    const loadSubjects = () => {
        var xhr = new XMLHttpRequest();
        xhr.open("get", SubjectsApi.getSubjectsUrl(), true);
        xhr.onload = function () {
            var data = JSON.parse(xhr.responseText);
            setSubjects(data);
        }.bind(this);
        xhr.send();
    }
    const loadGroupSubjects = () => {
        var xhr = new XMLHttpRequest();
        xhr.open("get", SubjectsApi.getGroupSubjectsUrl(groupId), true);
        xhr.onload = function () {
            var data = JSON.parse(xhr.responseText);
            setGroupSubjects(data);
        }.bind(this);
        xhr.send();
    }
    const onEditGroup = () => {
        group.formOfEducation = form
        var jsonData = {
            group,
            selectedSpecialities,
            selectedSubjects
        }
        setErrors(null);
        var xhr = new XMLHttpRequest();
        xhr.open("put", GroupsOfSpecialitiesApi.getPutUrl(facultyShortName), true);
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
        loadGroup();
        loadGroupSpecialities();
        loadGroupSubjects();
        loadSpecialities();
        loadSubjects();
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
        return (
            <Modal show={show} onHide={handleClose} backdrop="static" keyboard={false}>
                <Modal.Header closeButton>
                    <Modal.Title>Сохранить изменения</Modal.Title>
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
                        <Modal.Title>Изменить группу</Modal.Title>
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
