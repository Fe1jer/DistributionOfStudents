import SpecialitiesList from '../specialities/SpecialitiesList.jsx';
import SpecialityPlansList from '../recruitmentPlans/SpecialityPlansList.jsx';
import GroupsOfSpecialities from '../groupsOfSpecialities/GroupsOfSpecialities.jsx';

import ModalWindowPlansDelete from "../recruitmentPlans/ModalWindows/ModalWindowDelete.jsx";

import TablePreloader from "../TablePreloader.jsx";

import CreateButton from '../adminButtons/CreateButton.jsx';
import FacultiesService from "../../services/Faculties.service.js";
import RecruitmentPlansService from "../../services/RecruitmentPlans.service.js";
import SpecialitiesService from "../../services/Specialities.service";

import Placeholder from 'react-bootstrap/Placeholder';

import React, { useState } from 'react';
import { Link, useParams, useNavigate } from 'react-router-dom'
import useDocumentTitle from '../useDocumentTitle.jsx';

export default function FacultyPage() {
    const navigate = useNavigate();
    const params = useParams();
    const shortName = params.shortName;
    useDocumentTitle(shortName);

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
    const loadFacultyPlans = async () => {
        const data = await RecruitmentPlansService.httpGetLastFacultyPlans(shortName);
        setFacultyPlans(data.plansForSpecialities);
        setFacultyPlansYear(data.year);
    }

    const onDeleteFacultyPlans = async (facultyShortName) => {
        if (facultyPlansYear !== "0") {
            await RecruitmentPlansService.httpDelete(facultyShortName, facultyPlansYear);
            loadFacultyPlans();
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
                <hr />
                <h4>
                    План приёма на {facultyPlansYear} год
                    <CreateButton className="ms-2" onClick={() => navigate("/Faculties/" + shortName + "/RecruitmentPlans/" + facultyPlansYear + "/Create")} />
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
                    <SpecialitiesList onLoadSpecialities={updateSpecialities} specialities={specialities} shortName={shortName} />
                    {_showRecruitmenPlans()}
                    <GroupsOfSpecialities year={facultyPlansYear} />
                </div>
            </React.Suspense>
        );
    }
}