import SpecialitiesList from '../specialities/SpecialitiesList.jsx';
import SpecialityPlansList from '../recruitmentPlans/SpecialityPlansList.jsx';

import ModalWindowPlansDelete from "../recruitmentPlans/ModalWindows/ModalWindowDelete.jsx";

import TablePreloader from "../TablePreloader.jsx";

import FacultiesApi from "../../api/FacultiesApi.js";
import RecruitmentPlansApi from '../../api/RecruitmentPlansApi.js';
import SpecialitiesApi from '../../api/SpecialitiesApi.js';

import React, { useState } from 'react';
import { Link, useParams } from 'react-router-dom'

export default function FacultyPage() {
    const params = useParams();
    const shortName = params.shortName;

    const [faculty, setFaculty] = useState();
    const [loading, setLoading] = useState(true);
    const [facultyPlans, setFacultyPlans] = useState([]);
    const [facultyPlansYear, setFacultyPlansYear] = useState();
    const [specialities, setSpecialities] = useState([]);
    const [deletePlansShow, setDeletePlansShow] = useState(false);
    var numbers = [1, 2]

    const loadData = () => {
        loadFaculty();
        loadSpecialities();
        loadFacultyPlans();
    }
    const loadFaculty = () => {
        var xhr = new XMLHttpRequest();
        xhr.open("get", FacultiesApi.getFacultyUrl(shortName), true);
        xhr.onload = function () {
            var data = JSON.parse(xhr.responseText);
            setFaculty(data); setLoading(false);
        }.bind(this);
        xhr.send();
    }

    const updateSpecialities = () => {
        loadSpecialities();
        loadFacultyPlans();
    }
    const loadSpecialities = () => {
        var xhr = new XMLHttpRequest();
        xhr.open("get", SpecialitiesApi.getFacultySpecialitiesUrl(shortName), true);
        xhr.onload = function () {
            var data = JSON.parse(xhr.responseText);
            setSpecialities(data);
        }.bind(this);
        xhr.send();
    }
    const loadFacultyPlans = () => {
        var xhr = new XMLHttpRequest();
        xhr.open("get", RecruitmentPlansApi.getFacultyLastYearRecruitmentPlasUrl(shortName), true);
        xhr.onload = function () {
            var data = JSON.parse(xhr.responseText);
            setFacultyPlans(data.plansForSpecialities);
            setFacultyPlansYear(data.year);
        }.bind(this);
        xhr.send();
    }

    const onDeleteFacultyPlans = (facultyShortName) => {
        if (facultyPlansYear !== "0") {
            var xhr = new XMLHttpRequest();
            xhr.open("delete", RecruitmentPlansApi.getDeleteUrl(facultyShortName, facultyPlansYear), true);
            xhr.setRequestHeader("Content-Type", "application/json")
            xhr.onload = function () {
                if (xhr.status === 200) {
                    loadData();
                }
            }.bind(this);
            xhr.send();
        }
        handleDeletePlansClose();
    }

    const handleDeletePlansClose = () => {
        setDeletePlansShow(false);
    };
    const onClickPlansDelete = () => {
        setDeletePlansShow(true);
    }

    const _showRecruitmenPlans = () => {
        var title = null;
        var tableFacultyPlans = null;

        if (specialities.length !== 0 && specialities !== []) {
            title = <React.Suspense >
                <hr className="mt-4" />
                <h4>
                    План приёма на {facultyPlansYear} год
                    <Link className="text-success ms-2" to={"/Faculties/" + shortName + "/RecruitmentPlans/" + facultyPlansYear + "/Create"}>
                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" className="bi bi-plus-circle-fill suc" viewBox="0 0 16 16">
                            <path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zM8.5 4.5a.5.5 0 0 0-1 0v3h-3a.5.5 0 0 0 0 1h3v3a.5.5 0 0 0 1 0v-3h3a.5.5 0 0 0 0-1h-3v-3z" />
                        </svg>
                    </Link>
                </h4>
            </React.Suspense>;

            if (facultyPlansYear) {
                tableFacultyPlans = <React.Suspense >
                    <SpecialityPlansList facultyShortName={shortName} year={facultyPlansYear} plans={facultyPlans} onClickDelete={onClickPlansDelete} />
                    <ModalWindowPlansDelete show={deletePlansShow} handleClose={handleDeletePlansClose} onDeletePlans={onDeleteFacultyPlans} facultyShortName={shortName} year={facultyPlansYear} />
                </React.Suspense>;
            }
        }

        return (
            <React.Suspense>
                {title}
                {tableFacultyPlans}
            </React.Suspense>
        );
    }

    React.useEffect(() => {
        loadData();
    }, [])

    if (loading) {
        return (
            <React.Suspense>
                <h1 className="text-center placeholder-glow"><span className="placeholder w-25"></span></h1>
                <hr />
                <div id="content" className="ps-lg-4 pe-lg-4 position-relative">{
                    numbers.map((item) =>
                        <React.Suspense key={"FacultyPreloader" + item} >
                            <p className="placeholder-glow"><span className="placeholder w-25"></span></p>
                            <TablePreloader />
                            <hr className="mt-4 mx-0" />
                        </React.Suspense>
                    )}
                </div>
            </React.Suspense>
        );
    }
    else {
        return (
            <React.Suspense>
                <h1 className="text-center">{faculty.fullName}</h1>
                <hr className="mt-4" />
                <div className="ps-lg-4 pe-lg-4 pt-2 position-relative">
                    <h4>Специальности</h4>
                    <SpecialitiesList onLoadSpecialities={updateSpecialities} specialities={specialities} />
                    {_showRecruitmenPlans()}
                </div>
            </React.Suspense>
        );
    }
}