import Speciality from './Speciality';

import SpecialitiesService from "../../services/Specialities.service";

import ModalWindowCreate from './ModalWindows/ModalWindowCreate';
import ModalWindowEdit from './ModalWindows/ModalWindowEdit';
import ModalWindowDelete from './ModalWindows/ModalWindowDelete';
import ModalWindowEnable from './ModalWindows/ModalWindowEnable';
import ModalWindowDisable from './ModalWindows/ModalWindowDisable';
import CreateButton from '../adminButtons/CreateButton';

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
    const [enableShow, setEnableShow] = useState(false);
    const [disableShow, setDisableShow] = useState(false);

    const reloadSpecialities = () => {
        onLoadSpecialities();
        loadDisabledSpecialities();
    }

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
    const handleEnableClose = () => {
        setEnableShow(false);
        setSelectedSpecialityId(null);
    };
    const handleDisableClose = () => {
        setDisableShow(false);
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
    const onClickEnableSpeciality = (id) => {
        setSelectedSpecialityId(id);
        setEnableShow(true);
    }
    const onClickDisableSpeciality = (id) => {
        setSelectedSpecialityId(id);
        setDisableShow(true);
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
                    <Speciality key={item.directionCode ?? item.code + "-" + (item.directionName ?? item.fullName)} speciality={item}
                        onClickEdit={onClickEditSpeciality} onClickDelete={onClickDeleteSpeciality} onClickEnable={onClickEnableSpeciality} />
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
            <ModalWindowCreate show={createShow} handleClose={handleCreateClose} onLoadSpecialities={reloadSpecialities} />
            <ModalWindowEdit show={editShow} handleClose={handleEditClose} specialityId={selectedSpecialityId} onLoadSpecialities={reloadSpecialities} />
            <ModalWindowDelete show={deleteShow} handleClose={handleDeleteClose} specialityId={selectedSpecialityId} onLoadSpecialities={reloadSpecialities} />
            <ModalWindowEnable show={enableShow} handleClose={handleEnableClose} specialityId={selectedSpecialityId} onLoadSpecialities={reloadSpecialities} />
            <ModalWindowDisable show={disableShow} handleClose={handleDisableClose} specialityId={selectedSpecialityId} onLoadSpecialities={reloadSpecialities} />
            <Table responsive className="table mb-0">
                <thead>
                    <tr>
                        <th width="130">Код</th>
                        <th>Специальность (направление специальности)</th>
                        <th>Описание</th>
                        <th className="text-center" width="120">
                            <CreateButton onClick={onClickCreateSpeciality} />
                        </th>
                    </tr>
                </thead>
                <tbody>{
                    specialities.map((item) =>
                        <Speciality key={item.directionCode ?? item.code + "-" + (item.directionName ?? item.fullName)} speciality={item}
                            onClickDisable={onClickDisableSpeciality} onClickEdit={onClickEditSpeciality} onClickDelete={onClickDeleteSpeciality} />
                    )}
                </tbody>
                {_showDisabledSpecialities()}
            </Table>
        </div>
        {_showEnableDisableButtons()}
    </React.Suspense>;
}