import Subject from './Subject.jsx';

import ModalWindowCreate from './ModalWindows/ModalWindowCreate.jsx';
import ModalWindowEdit from './ModalWindows/ModalWindowEdit.jsx';
import ModalWindowDelete from './ModalWindows/ModalWindowDelete.jsx';

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
            <table className="table mb-0">
                <thead>
                    <tr>
                        <th width="20">
                            №
                        </th>
                        <th>
                            Название
                        </th>
                        <th className="text-center" width="120">
                            <button type="button" className="btn btn-sm text-success ms-2 p-0" onClick={onClickCreateSubject} >
                                <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" className="bi bi-plus-circle-fill suc" viewBox="0 0 16 16">
                                    <path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zM8.5 4.5a.5.5 0 0 0-1 0v3h-3a.5.5 0 0 0 0 1h3v3a.5.5 0 0 0 1 0v-3h3a.5.5 0 0 0 0-1h-3v-3z" />
                                </svg>
                            </button >
                        </th>
                    </tr>
                </thead>
                <tbody>{
                    subjects.map((item, index) =>
                        <Subject num={index + 1} key={item.id + item.name} subject={item} onClickEdit={onClickEditSubject} onClickDelete={onClickDeleteSubject} />
                    )}
                </tbody>
            </table>
        </div >
    );
}