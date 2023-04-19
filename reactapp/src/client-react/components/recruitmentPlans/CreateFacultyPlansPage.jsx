import UpdateSpecialityPlansList from "./UpdateSpecialityPlansList.jsx";
import ModalWindowCreate from "./ModalWindows/ModalWindowCreate.jsx";

import RecruitmentPlansApi from "../../api/RecruitmentPlansApi.js";
import FacultiesService from "../../services/Faculties.service.js";

import { Link, useParams, useNavigate } from 'react-router-dom'
import React, { useState } from 'react';

export default function CreateFacultyPlansPage() {
    const params = useParams();
    const facultyShortName = params.shortName;
    const lastYear = params.lastYear;

    const [plans, setPlans] = useState([]);
    const [year, setYear] = useState(parseInt(lastYear) + 1);
    const [facultyName, setFacultyName] = useState("");
    const [errors, setErrors] = useState({});
    const [createShow, setCreateShow] = useState(false);

    const navigate = useNavigate();

    const loadData = async () => {
        if (year == 1) {
            var now = new Date();
            setYear(now.getFullYear());
        }
        else {
            var xhr1 = new XMLHttpRequest();
            xhr1.open("get", RecruitmentPlansApi.getFacultyRecruitmentPlansUrl(facultyShortName, year), true);
            xhr1.onload = function () {
                var data = JSON.parse(xhr1.responseText);
                setPlans(data);
            }.bind(this);
            xhr1.send();

            const faciltyData = await FacultiesService.httpGetByShortName(facultyShortName);
            setFacultyName(faciltyData.fullName);
        }
    }
    const onCangeYear = (e) => {
        setYear(e.target.value);
    }
    const onChangePlans = (changedPlans) => {
        setPlans(changedPlans);
    }

    const onCreateFacultyPlans = () => {
        if (year != 1) {
            var xhr = new XMLHttpRequest();
            xhr.open("post", RecruitmentPlansApi.getPostUrl(facultyShortName, year), true);
            xhr.setRequestHeader("Content-Type", "application/json")
            xhr.onload = function () {
                if (xhr.status === 200) {
                    handleCreateClose();
                    navigate("/Faculties/" + facultyShortName);
                }
                else if (xhr.status === 400) {
                    var a = eval('({obj:[' + xhr.response + ']})');
                    setErrors(a.obj[0].errors);
                }
            }.bind(this);
            xhr.send(JSON.stringify(plans));
        }
        else {
            handleCreateClose();
        }
    }
    const handleCreateClose = () => {
        setCreateShow(false);
    };
    const onClickCreatePlans = () => {
        setCreateShow(true);
    }

    React.useEffect(() => {
        loadData();
    }, [year])

    return (
        <React.Suspense>
            <h1 className="input-group-lg text-center">План приёма на <input min="0" type="number" onChange={onCangeYear} className="form-control d-inline" value={year} style={{ width: 100 }} /> год</h1>
            <hr />
            <ModalWindowCreate show={createShow} handleClose={handleCreateClose} onCreatePlans={onCreateFacultyPlans} facultyShortName={facultyShortName} year={year} />
            <div className="ps-lg-4 pe-lg-4 position-relative">
                <h4>{facultyName}</h4>
                <UpdateSpecialityPlansList year={year} plans={plans} errors={errors} onChange={onChangePlans} />
            </div>
            <div className="col text-center pt-4">
                <button type="button" className="btn btn-outline-success btn-lg" onClick={() => onClickCreatePlans()}>
                    Создать
                </button>
                <Link type="button" className="btn btn-outline-secondary btn-lg" to={"/Faculties/" + facultyShortName}>В факультет</Link>
            </div>
        </React.Suspense>
    );
}