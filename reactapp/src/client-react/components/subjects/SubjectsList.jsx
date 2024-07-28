import Subject from './Subject.jsx';

import CreateButton from '../adminButtons/CreateButton.jsx';
import ModalWindowCreate from './ModalWindows/ModalWindowCreate.jsx';
import ModalWindowEdit from './ModalWindows/ModalWindowEdit.jsx';
import ModalWindowDelete from './ModalWindows/ModalWindowDelete.jsx';

import Table from 'react-bootstrap/Table';

import React, { useState } from 'react';

export default function SubjectsList({ subjects, onLoadSubjects }) {
    const [updateSubjectId, setUpdateSubjectId] = useState(null);
    const [deleteSubjectId, setDeleteSubjectId] = useState(null);
    const [editShow, setEditShow] = useState(false);
    const [createShow, setCreateShow] = useState(false);
    const [deleteShow, setDeleteShow] = useState(false);

    const handleEditClose = () => {
        setEditShow(false);
        setUpdateSubjectId(null);
    };
    const handleCreateClose = () => {
        setCreateShow(false);
    };
    const handleDeleteClose = () => {
        setDeleteShow(false);
        setDeleteSubjectId(null);
    };

    const onClickEditSubject = (id) => {
        setUpdateSubjectId(id);
        setEditShow(true);
    }
    const onClickCreateSubject = () => {
        setCreateShow(true);
    }
    const onClickDeleteSubject = (id) => {
        setDeleteShow(true);
        setDeleteSubjectId(id);
    }

    return (
        <div className="card shadow" >
            <ModalWindowCreate show={createShow} handleClose={handleCreateClose} onLoadSubjects={onLoadSubjects} />
            <ModalWindowEdit show={editShow} handleClose={handleEditClose} subjectId={updateSubjectId} onLoadSubjects={onLoadSubjects} />
            <ModalWindowDelete show={deleteShow} handleClose={handleDeleteClose} subjectId={deleteSubjectId} onLoadSubjects={onLoadSubjects} />
            <Table responsive className="table mb-0">
                <thead>
                    <tr>
                        <th width="20">
                            №
                        </th>
                        <th>
                            Название
                        </th>
                        <th className="text-center" width="100">
                            <CreateButton size="sm" onClick={onClickCreateSubject} />
                        </th>
                    </tr>
                </thead>
                <tbody>{
                    subjects.map((item, index) =>
                        <Subject num={index + 1} key={item.id + item.name} subject={item} onClickEdit={onClickEditSubject} onClickDelete={onClickDeleteSubject} />
                    )}
                </tbody>
            </Table>
        </div >
    );
}