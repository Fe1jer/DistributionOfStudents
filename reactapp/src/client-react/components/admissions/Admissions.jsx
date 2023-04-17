import AdmissionsList from "./AdmissionsList.jsx";
import Search from "../Searh.jsx";
import Pagination from "../Pagination.jsx";
import TablePreloader from "../TablePreloader.jsx";

import ModalWindowCreate from './ModalWindows/ModalWindowCreate.jsx';
import ModalWindowEdit from './ModalWindows/ModalWindowEdit.jsx';
import ModalWindowDelete from './ModalWindows/ModalWindowDelete.jsx';
import ModalWindowDetails from './ModalWindows/ModalWindowDetails.jsx';

import AdmissionsApi from "../../api/AdmissionsApi.js";

import React, { useState } from 'react';
import { useLocation } from 'react-router-dom';

export default function Admissions({ groupId, plans, onLoadGroup }) {
    const location = useLocation();
    var searchStudentsParam = new URLSearchParams(location.search).get("searchStudents") ?? "";

    const [admissions, setAdmissions] = useState([]);
    const [searhText, setSearhText] = useState(searchStudentsParam);
    const [currentPage, setCurrentPage] = useState(0);
    const [pageLimit, setPageLimit] = useState(20);
    const [countSearchAdmissions, setCountSearchAdmissions] = useState(0);
    const [pageNeighbours, setPageNeighbours] = useState(4);
    const [loading, setLoading] = useState(true);
    const [deleteAdmissionId, setDeleteAdmissionId] = useState(null);
    const [editAdmissionId, setEditAdmissionId] = useState(null);
    const [detailsAdmissionId, setDetailsAdmissionId] = useState(null);
    const [deleteShow, setDeleteShow] = useState(false);
    const [editShow, setEditShow] = useState(false);
    const [createShow, setCreateShow] = useState(false);
    const [detailsShow, setDetailsShow] = useState(false);

    const loadData = () => {
        var xhr = new XMLHttpRequest();
        xhr.open("get", AdmissionsApi.getGroupAdmissionsUrl(groupId, searhText, currentPage, pageLimit), true);
        xhr.onload = function () {
            var data = JSON.parse(xhr.responseText);
            setAdmissions(data.admissions);
            setCountSearchAdmissions(data.countOfSearchStudents);
            setLoading(false);
        }.bind(this);
        xhr.send();
    }
    const handleDeleteClose = () => {
        setDeleteShow(false);
        setDeleteAdmissionId(null);
    };
    const onClickDeleteAdmission = (id) => {
        setDeleteShow(true);
        setDeleteAdmissionId(id);
    }
    const handleEditClose = () => {
        setEditShow(false);
        setEditAdmissionId(null);
    };
    const onClickEditAdmission = (id) => {
        setEditShow(true);
        setEditAdmissionId(id);
    }
    const handleCreateClose = () => {
        setCreateShow(false);
    };
    const onClickCreateAdmission = () => {
        setCreateShow(true);
    }
    const handleDetailsClose = () => {
        setDetailsShow(false);
        setDetailsAdmissionId(null);
    };
    const onClickDetailsAdmission = (id) => {
        setDetailsAdmissionId(id);
        setDetailsShow(true);
    }
    const onSearhChange = (text) => {
        setSearhText(text);
    }
    const onPageChanged = (data) => {
        setCurrentPage(data.currentPage);
        setPageLimit(data.pageLimit);
    }
    React.useEffect(() => {
        loadData();
    }, [searhText, currentPage])

    if (loading) {
        return <React.Suspense>
            <TablePreloader />
        </React.Suspense>
    }
    else {
        return (
            <React.Suspense>
                <ModalWindowCreate show={createShow} handleClose={handleCreateClose} onLoadAdmissions={loadData} onLoadGroup={onLoadGroup} />
                <ModalWindowEdit show={editShow} handleClose={handleEditClose} admissionId={editAdmissionId} onLoadAdmissions={loadData} onLoadGroup={onLoadGroup} />
                <ModalWindowDelete show={deleteShow} handleClose={handleDeleteClose} admissionId={deleteAdmissionId} onLoadAdmissions={loadData} onLoadGroup={onLoadGroup} />
                <ModalWindowDetails show={detailsShow} handleClose={handleDetailsClose} admissionId={detailsAdmissionId} />
                <Search filter={onSearhChange} defaultValue={searhText} />
                <AdmissionsList key={admissions} admissions={admissions} plans={plans}
                    onClickDelete={onClickDeleteAdmission} onClickEdit={onClickEditAdmission} onClickCreate={onClickCreateAdmission} onClickDetails={onClickDetailsAdmission} />
                <Pagination key={countSearchAdmissions} totalRecords={countSearchAdmissions} pageLimit={pageLimit} pageNeighbours={pageNeighbours} onPageChanged={onPageChanged} />
            </React.Suspense>
        );
    }
}
