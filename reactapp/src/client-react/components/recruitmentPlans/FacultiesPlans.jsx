import FacultyPlans from './FacultyPlans.jsx';
import ModalWindowDelete from "./ModalWindows/ModalWindowDelete.jsx";

import TablePreloader from "../TablePreloader.jsx";

import RecruitmentPlansService from "../../services/RecruitmentPlans.service.js";

import React, { useState } from 'react';
import useDocumentTitle from '../useDocumentTitle.jsx';

export default function FacultiesPlans() {
    const [year, setYear] = useState(0);
    useDocumentTitle("План приёма " + year);

    const [deleteFacultyShortName, setDeleteFacultyShortName] = useState(null);
    const [deleteYear, setDeleteYear] = useState(null);
    const [facultiesPlans, setFacultiesPlans] = useState([]);
    const [loading, setLoading] = useState(true);
    const [deleteShow, setDeleteShow] = useState(false);
    var numbers = [1, 2]

    const loadData = async () => {
        const recruitmentsPlansData = await RecruitmentPlansService.httpGet();
        setFacultiesPlans(recruitmentsPlansData);
        setLoading(false);
        const year = recruitmentsPlansData.lenght !== 0 ? Math.max(...recruitmentsPlansData.map(o => o.year)) : 0;
        setYear(year);
    }

    const onDeleteFacultyPlans = async (facultyShortName, year) => {
        if (year != null) {
            await RecruitmentPlansService.httpDelete(facultyShortName, year)
            loadData();
        }
        handleDeleteClose();
    }

    const handleDeleteClose = () => {
        setDeleteShow(false);
        setDeleteYear(null);
        setDeleteFacultyShortName(null);
    };
    const onClickDeleteFacultyPlans = (facultyShortName, year) => {
        setDeleteYear(year);
        setDeleteFacultyShortName(facultyShortName);
        setDeleteShow(true);
    }

    React.useEffect(() => {
        loadData();
    }, [])

    if (loading) {
        return (
            <React.Suspense>
                <h1 className="text-center placeholder-glow"><span className="placeholder w-25"></span></h1>
                <div id="content" className="ps-lg-4 pe-lg-4 position-relative">{
                    numbers.map((item) =>
                        <React.Suspense key={"FacultiesPlansPreloader" + item}>
                            <hr className="mt-4 mx-0" />
                            <p className="placeholder-glow"><span className="placeholder w-25"></span></p>
                            <TablePreloader />
                        </React.Suspense>
                    )}
                </div>
            </React.Suspense>
        );
    }
    else {
        return (
            <React.Suspense>
                <h1 className="text-center"> План приёма на {year} год</h1>
                <div className="ps-lg-4 pe-lg-4 position-relative">
                    <ModalWindowDelete show={deleteShow} handleClose={handleDeleteClose} onDeletePlans={onDeleteFacultyPlans} facultyShortName={deleteFacultyShortName} year={deleteYear} /> {
                        facultiesPlans.map((item) =>
                            <FacultyPlans key={item.facultyShortName} facultyFullName={item.facultyFullName} facultyShortName={item.facultyShortName} year={item.year} plansForSpecialities={item.plansForSpecialities} onClickDelete={onClickDeleteFacultyPlans} />
                        )}
                </div>
            </React.Suspense>
        );
    }
}