import UpdateSpecialityPlansList from "./UpdateSpecialityPlansList.jsx";
import ModalWindowEdit from "./ModalWindows/ModalWindowEdit.jsx";

import FacultiesService from "../../services/Faculties.service.js";
import RecruitmentPlansService from "../../services/RecruitmentPlans.service.js";

import { RecruitmentPlansValidationSchema } from '../../validations/RecruitmentPlans.validation';

import { Formik } from 'formik';
import { Link, useParams, useNavigate } from 'react-router-dom'
import React, { useState } from 'react';

export default function EditFacultyPlansPage() {
    const params = useParams();
    const facultyShortName = params.shortName;
    const year = params.year;

    const [plans, setPlans] = useState(null);
    const [facultyName, setFacultyName] = useState("");
    const [editShow, setEditShow] = useState(false);

    const navigate = useNavigate();

    const loadData = async () => {
        const faciltyData = FacultiesService.httpGetByShortName(facultyShortName);
        const recruitmentsPlansData = RecruitmentPlansService.httpGetFacultyRecruitmentPlans(facultyShortName, year);

        setFacultyName((await faciltyData).fullName);
        setPlans(await recruitmentsPlansData);
    }
    const onChangePlans = (changedPlans) => {
        setPlans(changedPlans);
    }

    const onEditFacultyPlans = async () => {
        if (year != 1) {
            await RecruitmentPlansService.httpPut(facultyShortName, year, plans)
            handleEditClose();
            navigate("/Faculties/" + facultyShortName);
        }
        else {
            handleEditClose();
        }
    }
    const handleEditClose = () => {
        setEditShow(false);
    };
    const onClickEditPlans = (values) => {
        setPlans(values.plans);
        setEditShow(true);
    }

    React.useEffect(() => {
        loadData();
    }, [])

    if (plans && year) {
        return (
            <Formik
                validationSchema={RecruitmentPlansValidationSchema}
                onSubmit={onClickEditPlans}
                initialValues={{ plans, year }}>
                {({ handleSubmit, handleChange, values, touched, errors }) => (
                    <React.Suspense>
                        <h1 className="input-group-lg text-center">План приёма на {values.year} год</h1>
                        <hr />
                        <ModalWindowEdit show={editShow} handleClose={handleEditClose} onEditPlans={onEditFacultyPlans} facultyShortName={facultyShortName} year={year} />
                        <div className="ps-lg-4 pe-lg-4 position-relative">
                            <h4>{facultyName}</h4>
                            <UpdateSpecialityPlansList plans={values.plans} errors={errors.plans ?? {}} onChange={handleChange} />
                        </div>
                        <div className="col text-center pt-4">
                            <button type="button" className="btn btn-outline-success btn-lg" onClick={handleSubmit}>
                                Сохранить
                            </button>
                            <Link type="button" className="btn btn-outline-secondary btn-lg" to={"/Faculties/" + facultyShortName}>В факультет</Link>
                        </div>
                    </React.Suspense>
                )}
            </Formik>
        );
    }
}