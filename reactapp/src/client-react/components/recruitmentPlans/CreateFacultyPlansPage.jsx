import UpdateSpecialityPlansList from "./UpdateSpecialityPlansList.jsx";
import ModalWindowCreate from "./ModalWindows/ModalWindowCreate.jsx";

import FacultiesService from "../../services/Faculties.service.js";
import RecruitmentPlansService from "../../services/RecruitmentPlans.service.js";

import { RecruitmentPlansValidationSchema } from '../../validations/RecruitmentPlans.validation';

import Form from 'react-bootstrap/Form';

import { Formik } from 'formik';
import { Link, useParams, useNavigate } from 'react-router-dom'
import React, { useState } from 'react';

export default function CreateFacultyPlansPage() {
    const params = useParams();
    const facultyShortName = params.shortName;
    const lastYear = params.lastYear;

    const [plans, setPlans] = useState(null);
    const [year, setYear] = useState(parseInt(lastYear) + 1);
    const [facultyName, setFacultyName] = useState("");
    const [createShow, setCreateShow] = useState(false);

    const navigate = useNavigate();

    const loadData = async () => {
        if (year == 1) {
            var now = new Date();
            setYear(now.getFullYear());
        }
        const faciltyData = FacultiesService.httpGetByShortName(facultyShortName);
        const recruitmentsPlansData = RecruitmentPlansService.httpGetFacultyRecruitmentPlans(facultyShortName, year);
        setFacultyName((await faciltyData).fullName);
        setPlans(await recruitmentsPlansData);
    }

    const onCreateFacultyPlans = async () => {
        if (year != 1) {
            await RecruitmentPlansService.httpPost(facultyShortName, year, plans)
            handleCreateClose();
            navigate("/Faculties/" + facultyShortName);
        }
        else {
            handleCreateClose();
        }
    }
    const handleCreateClose = () => {
        setCreateShow(false);
    };
    const onClickCreatePlans = (values) => {
        setPlans(values.plans);
        setYear(values.year);
        setCreateShow(true);
    }

    React.useEffect(() => {
        loadData();
    }, [])

    if (plans && year) {
        return (
            <Formik
                validationSchema={RecruitmentPlansValidationSchema}
                onSubmit={onClickCreatePlans}
                initialValues={{ plans, year }}>
                {({ handleSubmit, handleChange, values, touched, errors }) => (
                    <React.Suspense>
                        <h1 className="input-group-lg text-center">План приёма на
                            <Form.Control className="form-control d-inline mx-2" min="0" style={{ width: 100 }} type="number" name={"year"}
                                value={values.year}
                                onChange={handleChange}
                                isInvalid={!!errors.year} />
                            год
                        </h1>
                        <hr />
                        <ModalWindowCreate show={createShow} handleClose={handleCreateClose} onCreatePlans={onCreateFacultyPlans} facultyShortName={facultyShortName} year={values.year} />
                        <div className="ps-lg-4 pe-lg-4 position-relative">
                            <h4>{facultyName}</h4>
                            <UpdateSpecialityPlansList plans={values.plans} errors={errors.plans ?? {}} onChange={handleChange} />
                        </div>
                        <div className="col text-center pt-4">
                            <button type="button" className="btn btn-outline-success btn-lg" onClick={handleSubmit}>
                                Создать
                            </button>
                            <Link type="button" className="btn btn-outline-secondary btn-lg" to={"/Faculties/" + facultyShortName}>В факультет</Link>
                        </div>
                    </React.Suspense>
                )}
            </Formik>
        );
    }
}