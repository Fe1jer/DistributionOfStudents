import GroupsOfSpecialitiesList from "./GroupsOfSpecialitiesList.jsx";

import TablePreloader from "../TablePreloader.jsx";

import ModalWindowCreate from './ModalWindows/ModalWindowCreate.jsx';
import ModalWindowEdit from './ModalWindows/ModalWindowEdit.jsx';
import ModalWindowDelete from './ModalWindows/ModalWindowDelete.jsx';

import GroupsOfSpecialitiesApi from '../../api/GroupsOfSpecialitiesApi.js';

import React, { useState } from 'react';
import { useParams } from 'react-router-dom'

export default function GroupsOfSpecialities({ year }) {
    const params = useParams();
    const shortName = params.shortName;

    const [groupsOfSpecialities, setGroupsOfSpecialities] = useState(null);
    const [deleteShow, setDeleteShow] = useState(false);
    const [editShow, setEditShow] = useState(false);
    const [createShow, setCreateShow] = useState(false);
    const [deleteGroupId, setDeleteGroupId] = useState();
    const [editGroupId, setEditGroupId] = useState();

    const loadGroupsOfSpecialities = () => {
        var xhr = new XMLHttpRequest();
        xhr.open("get", GroupsOfSpecialitiesApi.getFacultyGroupsUrl(shortName, year), true);
        xhr.onload = function () {
            var data = JSON.parse(xhr.responseText);
            setGroupsOfSpecialities(data);
        }.bind(this);
        xhr.send();
    }

    React.useEffect(() => {
        loadGroupsOfSpecialities();
    }, [])

    const handleDeleteClose = () => {
        setDeleteGroupId(null);
        setDeleteShow(false);
    };
    const onClickDelete = (groupId) => {
        setDeleteGroupId(groupId);
        setDeleteShow(true);
    }
    const handleEditClose = () => {
        setEditGroupId(null);
        setEditShow(false);
    };
    const onClickEdit = (groupId) => {
        setEditGroupId(groupId);
        setEditShow(true);
    }
    const handleCreateClose = () => {
        setCreateShow(false);
    };
    const onClickCreate = () => {
        setCreateShow(true);
    }

    if (!groupsOfSpecialities) {
        return <React.Suspense>
            <hr className="mt-4" />
            <h4 className="placeholder-glow"><span className="placeholder w-25"></span></h4>
            <TablePreloader />
        </React.Suspense>
    }
    else {
        var groups = null;
        var distributedGroups = null;

        if (year) {
            groups = <React.Suspense>
                <ModalWindowCreate show={createShow} handleClose={handleCreateClose} onLoadGroups={loadGroupsOfSpecialities} />
                <ModalWindowEdit show={editShow} handleClose={handleEditClose} groupId={editGroupId} onLoadGroups={loadGroupsOfSpecialities} />
                <ModalWindowDelete show={deleteShow} handleClose={handleDeleteClose} groupId={deleteGroupId} year={year} onLoadGroups={loadGroupsOfSpecialities} />
                <hr className="mt-4" />
                <h4>Ход приёма документов</h4>
                <GroupsOfSpecialitiesList facultyShortName={shortName} groups={groupsOfSpecialities.filter(group => !group.isCompleted)} onClickDelete={onClickDelete} onClickCreate={onClickCreate} onClickEdit={onClickEdit} />
            </React.Suspense>;

            if (groupsOfSpecialities.filter(group => group.isCompleted).length > 0) {
                distributedGroups = <React.Suspense >
                    <hr className="mt-4" />
                    <h4>Зачисление</h4>
                    <GroupsOfSpecialitiesList facultyShortName={shortName} groups={groupsOfSpecialities.filter(group => group.isCompleted)} />
                </React.Suspense>;
            }
        }

        return (
            <React.Suspense>
                {groups}
                {distributedGroups}
            </React.Suspense>
        );
    }
}