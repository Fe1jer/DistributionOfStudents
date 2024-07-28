import FacultyCard from './FacultyCard.jsx';
import FacultyCardPreloader from "./FacultyCardPreloader.jsx";

import ModalWindowDelete from "./ModalWindows/ModalWindowDelete.jsx";
import ModalWindowEdit from "./ModalWindows/ModalWindowEdit.jsx";
import ModalWindowCreate from "./ModalWindows/ModalWindowCreate.jsx";
import CreateButton from '../adminButtons/CreateButton.jsx';

import Row from 'react-bootstrap/Row';

import FacultiesService from "../../services/Faculties.service.js";

import "../../../css/faculty.css"

import React, { useState } from 'react';
import useDocumentTitle from '../useDocumentTitle.jsx';

export default function FacultiesPage() {
    useDocumentTitle("Факультеты");

    const [facultiesPlans, setFacultiesPlans] = useState([]);
    const [loading, setLoading] = useState(true);
    var numbers = [1, 2, 3, 4, 5, 6, 7, 8]
    const [deleteFullNameShow, setDeleteFullName] = useState(null);
    const [deleteShortNameShow, setDeleteShortName] = useState(null);
    const [updateShortNameShow, setUpdateShortName] = useState(null);
    const [editShow, setEditShow] = useState(false);
    const [createShow, setCreateShow] = useState(false);
    const [deleteShow, setDeleteShow] = useState(false);

    const loadData = async () => {
        const faciltiesData = await FacultiesService.httpGet();
        setFacultiesPlans(faciltiesData);
        setLoading(false);
    }

    const handleEditClose = () => {
        setUpdateShortName(null);
        setEditShow(false);
    };
    const handleCreateClose = () => {
        setCreateShow(false);
    };
    const handleDeleteClose = () => {
        setDeleteFullName(null);
        setDeleteShortName(null)
        setDeleteShow(false);
    };

    const onClickEditFaculty = (shortName) => {
        setUpdateShortName(shortName);
        setEditShow(true);
    }
    const onClickCreateFaculty = () => {
        setCreateShow(true);
    }
    const onClickDeleteFaculty = (shortName, fullName) => {
        setDeleteFullName(fullName);
        setDeleteShortName(shortName)
        setDeleteShow(true);
    }

    React.useEffect(() => {
        loadData();
    }, [])

    if (loading) {
        return (
            <div className="ps-lg-4 pe-lg-4 pt-2 position-relative">
                <h1 className="placeholder-glow"><span className="placeholder" style={{ width: 220 }}></span></h1>
                <Row className="mx-1">{
                    numbers.map((item) =>
                        <FacultyCardPreloader key={"FacultyCardPreloader" + item} />
                    )}
                </Row>
            </div>
        );
    }
    else {
        return (
            <React.Suspense>
                <div className="ps-lg-4 pe-lg-4 pt-2 position-relative">
                    <h1 className="d-inline-block">
                        Факультеты
                        <CreateButton className="ms-2" onClick={() => onClickCreateFaculty()} />
                    </h1>
                    <ModalWindowDelete show={deleteShow} handleClose={handleDeleteClose} shortName={deleteShortNameShow} fullName={deleteFullNameShow} onLoadFaculties={loadData} />
                    <ModalWindowEdit show={editShow} handleClose={handleEditClose} shortName={updateShortNameShow} onLoadFaculties={loadData} />
                    <ModalWindowCreate show={createShow} handleClose={handleCreateClose} onLoadFaculties={loadData} />

                    <Row className="mx-1">{
                        facultiesPlans.map((item) =>
                            <FacultyCard key={item.shortName + item.fullName} faculty={item} onClickDelete={onClickDeleteFaculty} onClickEdit={onClickEditFaculty} />
                        )}
                    </Row>
                </div>
            </React.Suspense>
        );
    }
}
