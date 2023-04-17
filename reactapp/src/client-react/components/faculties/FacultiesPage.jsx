import FacultyCard from './FacultyCard.jsx';
import FacultyCardPreloader from "./FacultyCardPreloader.jsx";

import ModalWindowDelete from "./ModalWindows/ModalWindowDelete.jsx";
import ModalWindowEdit from "./ModalWindows/ModalWindowEdit.jsx";
import ModalWindowCreate from "./ModalWindows/ModalWindowCreate.jsx";

import Row from 'react-bootstrap/Row';

import FacultiesApi from "../../api/FacultiesApi.js";

import "../../../css/faculty.css"

import React, { useState } from 'react';

export default function FacultiesPage() {
    const [facultiesPlans, setFacultiesPlans] = useState([]);
    const [loading, setLoading] = useState(true);
    var numbers = [1, 2, 3, 4, 5, 6, 7, 8]
    const [deleteFullNameShow, setDeleteFullName] = useState(null);
    const [deleteShortNameShow, setDeleteShortName] = useState(null);
    const [updateShortNameShow, setUpdateShortName] = useState(null);
    const [editShow, setEditShow] = useState(false);
    const [createShow, setCreateShow] = useState(false);
    const [deleteShow, setDeleteShow] = useState(false);

    const loadData = () => {
        var xhr = new XMLHttpRequest();
        xhr.open("get", FacultiesApi.getFacultiesUrl(), true);
        xhr.onload = function () {
            var data = JSON.parse(xhr.responseText);
            setFacultiesPlans(data);
            setLoading(false);
        }.bind(this);
        xhr.send();
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
                        <button className="btn text-success ms-2 p-0" onClick={() => onClickCreateFaculty()}>
                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" className="bi bi-plus-circle-fill suc" viewBox="0 0 16 16">
                                <path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zM8.5 4.5a.5.5 0 0 0-1 0v3h-3a.5.5 0 0 0 0 1h3v3a.5.5 0 0 0 1 0v-3h3a.5.5 0 0 0 0-1h-3v-3z" />
                            </svg>
                        </button>
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
