import SpecialitiesList from '../specialities/SpecialitiesList.jsx';
import SpecialityPlansList from '../recruitmentPlans/SpecialityPlansList.jsx';
import GroupsOfSpecialities from '../groupsOfSpecialities/GroupsOfSpecialities.jsx';

import ModalWindowPlansDelete from "../recruitmentPlans/ModalWindows/ModalWindowDelete.jsx";

import TablePreloader from "../TablePreloader.jsx";

import FacultiesService from "../../services/Faculties.service.js";
import RecruitmentPlansApi from '../../api/RecruitmentPlansApi.js';
import SpecialitiesService from "../../services/Specialities.service";

import Placeholder from 'react-bootstrap/Placeholder';

import React, { useState } from 'react';
import { Link, useParams } from 'react-router-dom'

export default function FacultyPage() {
    const params = useParams();
    const shortName = params.shortName;

    const [faculty, setFaculty] = useState();
    const [loading, setLoading] = useState(true);
    const [facultyPlans, setFacultyPlans] = useState([]);
    const [facultyPlansYear, setFacultyPlansYear] = useState(null);
    const [specialities, setSpecialities] = useState([]);
    const [deletePlansShow, setDeletePlansShow] = useState(false);

    var numbers = [1, 2]

    const loadData = () => {
        loadFaculty();
        loadSpecialities();
        loadFacultyPlans();
    }
    const loadFaculty = async () => {
        const faciltyData = await FacultiesService.httpGetByShortName(shortName);
        setFaculty(faciltyData);
        setLoading(false);
    }

    const updateSpecialities = () => {
        loadSpecialities();
        loadFacultyPlans();
    }
    const loadSpecialities = async () => {
        var specialitiesData = await SpecialitiesService.httpGetFacultySpecialities(shortName);
        setSpecialities(specialitiesData);
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
                    loadFacultyPlans();
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

    if (loading || facultyPlansYear === null) {
        return (
            <React.Suspense>
                <Placeholder as="h1" animation="glow" className="text-center"><Placeholder className="w-50" /></Placeholder>
                <hr />
                <div id="content" className="ps-lg-4 pe-lg-4 position-relative">{
                    numbers.map((item) =>
                        <React.Suspense key={"FacultyPreloader" + item} >
                            <Placeholder as="p" animation="glow"><Placeholder className="w-25" /></Placeholder>
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
                <hr />
                <div className="ps-lg-4 pe-lg-4 pt-2 position-relative">
                    <h4>Специальности</h4>
                    <SpecialitiesList onLoadSpecialities={updateSpecialities} specialities={specialities} />
                    {_showRecruitmenPlans()}
                    <GroupsOfSpecialities year={facultyPlansYear} />
                </div>
            </React.Suspense>
        );
    }
}