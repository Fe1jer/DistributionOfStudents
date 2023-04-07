import Speciality from './Speciality.jsx';

import ModalWindowCreate from './ModalWindows/ModalWindowCreate.jsx';
import ModalWindowEdit from './ModalWindows/ModalWindowEdit.jsx';
import ModalWindowDelete from './ModalWindows/ModalWindowDelete.jsx';

import Table from 'react-bootstrap/Table';

import React, { useState } from 'react';

export default function SpecialitiesList({ specialities, onLoadSpecialities }) {
    const [updateSpecialityId, setUpdateSpecialityId] = useState(null);
    const [deleteSpecialityId, setDeleteSpecialityId] = useState(null);
    const [editShow, setEditShow] = useState(false);
    const [createShow, setCreateShow] = useState(false);
    const [deleteShow, setDeleteShow] = useState(false);

    const handleEditClose = () => {
        setEditShow(false);
        setUpdateSpecialityId(null);
    };
    const handleCreateClose = () => {
        setCreateShow(false);
    };
    const handleDeleteClose = () => {
        setDeleteShow(false);
        setDeleteSpecialityId(null);
    };

    const onClickEditSpeciality = (id) => {
        setUpdateSpecialityId(id);
        setEditShow(true);
    }
    const onClickCreateSpeciality = () => {
        setCreateShow(true);
    }
    const onClickDeleteSpeciality = (id) => {
        setDeleteShow(true);
        setDeleteSpecialityId(id);
    }

    return <div className="card shadow">
        <ModalWindowCreate show={createShow} handleClose={handleCreateClose} onLoadSpecialities={onLoadSpecialities} />
        <ModalWindowEdit show={editShow} handleClose={handleEditClose} specialityId={updateSpecialityId} onLoadSpecialities={onLoadSpecialities} />
        <ModalWindowDelete show={deleteShow} handleClose={handleDeleteClose} specialityId={deleteSpecialityId} onLoadSpecialities={onLoadSpecialities} />
        <Table responsive className="table mb-0">
            <thead>
                <tr>
                    <th width="130">Код</th>
                    <th>Специальность (направление специальности)</th>
                    <th>Описание</th>
                    <th className="text-center" width="80">
                        <button type="button" className="btn text-success p-0" onClick={onClickCreateSpeciality} >
                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" className="bi bi-plus-circle-fill suc" viewBox="0 0 16 16">
                                <path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zM8.5 4.5a.5.5 0 0 0-1 0v3h-3a.5.5 0 0 0 0 1h3v3a.5.5 0 0 0 1 0v-3h3a.5.5 0 0 0 0-1h-3v-3z" />
                            </svg>
                        </button >
                    </th>
                </tr>
            </thead>
            <tbody>{
                specialities.map((item) =>
                    <Speciality key={item.directionCode ?? item.code + "-" + (item.directionName ?? item.fullName)} speciality={item} onClickEdit={onClickEditSpeciality} onClickDelete={onClickDeleteSpeciality} />
                )}
            </tbody>
        </Table>
    </div>;
}