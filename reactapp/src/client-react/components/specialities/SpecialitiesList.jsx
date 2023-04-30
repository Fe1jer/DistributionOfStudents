import Speciality from './Speciality.jsx';

import SpecialitiesService from "../../services/Specialities.service";

import ModalWindowCreate from './ModalWindows/ModalWindowCreate.jsx';
import ModalWindowEdit from './ModalWindows/ModalWindowEdit.jsx';
import ModalWindowDelete from './ModalWindows/ModalWindowDelete.jsx';

import Table from 'react-bootstrap/Table';
import Button from 'react-bootstrap/Button';

import $ from 'jquery';
import React, { useState } from 'react';

export default function SpecialitiesList({ specialities, onLoadSpecialities, shortName }) {
    const [disabledSpecialities, setDisabledSpecialities] = useState(null);
    const [selectedSpecialityId, setSelectedSpecialityId] = useState(null);
    const [editShow, setEditShow] = useState(false);
    const [createShow, setCreateShow] = useState(false);
    const [deleteShow, setDeleteShow] = useState(false);

    const loadDisabledSpecialities = async () => {
        var disabledSpecialitiesData = await SpecialitiesService.httpGetFacultyDisabledSpecialities(shortName);
        setDisabledSpecialities(disabledSpecialitiesData);
    }
    const _hideSpecialities = async () => {
        $("#showButton").show();
        $("#hideButton").hide();
        $(".isDisabled").each(function () {
            $(this).hide();
        });
    }
    const _showSpecialities = async () => {
        $("#showButton").hide();
        $("#hideButton").show();
        $(".isDisabled").each(function () {
            $(this).show();
        });
    }
    const handleEditClose = () => {
        setEditShow(false);
        setSelectedSpecialityId(null);
    };
    const handleCreateClose = () => {
        setCreateShow(false);
    };
    const handleDeleteClose = () => {
        setDeleteShow(false);
        setSelectedSpecialityId(null);
    };

    const onClickEditSpeciality = (id) => {
        setSelectedSpecialityId(id);
        setEditShow(true);
    }
    const onClickCreateSpeciality = () => {
        setCreateShow(true);
    }
    const onClickDeleteSpeciality = (id) => {
        setDeleteShow(true);
        setSelectedSpecialityId(id);
    }
    const _showEnableDisableButtons = () => {
        if (disabledSpecialities && disabledSpecialities.length > 0) {
            return <React.Suspense>
                <Button id="showButton" variant="outline-success" size="sm" className="py-0" onClick={_showSpecialities}>Показать скрытые специальности</Button>
                <Button id="hideButton" variant="outline-success" size="sm" className="py-0" onClick={_hideSpecialities}>Скрыть скрытые специальности</Button>
            </React.Suspense>
        }
    }
    const _showDisabledSpecialities = () => {
        if (disabledSpecialities && disabledSpecialities.length > 0) {
            return <tbody>{
                disabledSpecialities.map((item) =>
                    <Speciality key={item.directionCode ?? item.code + "-" + (item.directionName ?? item.fullName)} speciality={item} onClickEdit={onClickEditSpeciality} onClickDelete={onClickDeleteSpeciality} />
                )}
            </tbody>
        }
    }
    React.useEffect(() => {
        _hideSpecialities();
        if (!disabledSpecialities) {
            loadDisabledSpecialities();
        }
    }, [disabledSpecialities])

    return <React.Suspense>
        <div className="card shadow">
            <ModalWindowCreate show={createShow} handleClose={handleCreateClose} onLoadSpecialities={onLoadSpecialities} />
            <ModalWindowEdit show={editShow} handleClose={handleEditClose} specialityId={selectedSpecialityId} onLoadSpecialities={onLoadSpecialities} />
            <ModalWindowDelete show={deleteShow} handleClose={handleDeleteClose} specialityId={selectedSpecialityId} onLoadSpecialities={onLoadSpecialities} />
            <Table responsive className="table mb-0">
                <thead>
                    <tr>
                        <th width="130">Код</th>
                        <th>Специальность (направление специальности)</th>
                        <th>Описание</th>
                        <th className="text-center" width="120">
                            <Button variant="empty" className="p-0 text-success" onClick={onClickCreateSpeciality} >
                                <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" className="bi bi-plus-circle-fill suc" viewBox="0 0 16 16">
                                    <path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zM8.5 4.5a.5.5 0 0 0-1 0v3h-3a.5.5 0 0 0 0 1h3v3a.5.5 0 0 0 1 0v-3h3a.5.5 0 0 0 0-1h-3v-3z" />
                                </svg>
                            </Button >
                        </th>
                    </tr>
                </thead>
                <tbody>{
                    specialities.map((item) =>
                        <Speciality key={item.directionCode ?? item.code + "-" + (item.directionName ?? item.fullName)} speciality={item} onClickEdit={onClickEditSpeciality} onClickDelete={onClickDeleteSpeciality} />
                    )}
                </tbody>
                {_showDisabledSpecialities()}
            </Table>
        </div>
        {_showEnableDisableButtons()}
    </React.Suspense>;
}