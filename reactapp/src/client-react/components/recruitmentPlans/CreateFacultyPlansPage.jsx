import UpdateSpecialityPlansList from "./UpdateSpecialityPlansList.jsx";
import ModalWindowCreate from "./ModalWindows/ModalWindowCreate.jsx";

import RecruitmentPlansApi from "../../api/RecruitmentPlansApi.js";
import FacultiesApi from "../../api/FacultiesApi.js";

import { Link, useParams, useNavigate } from 'react-router-dom'
import React from 'react';

export default function CreateFacultyPlansPage() {
    const params = useParams();
    const facultyShortName = params.facultyShortName;
    const lastYear = params.lastYear;

    const [plans, setPlans] = React.useState([]);
    const [year, setYear] = React.useState(lastYear + 1);
    const [facultyName, setFacultyName] = React.useState("");
    const [errors, setErrors] = React.useState({});

    const navigate = useNavigate();

    const loadData = () => {
        if (year == 1) {
            var now = new Date();
            setYear(now.getFullYear());
        }
        var xhr = new XMLHttpRequest();
        xhr.open("get", RecruitmentPlansApi.getFacultyRecruitmentPlansUrl(facultyShortName, year), true);
        xhr.onload = function () {
            var data = JSON.parse(xhr.responseText);
            setPlans(data);
        }.bind(this);
        xhr.send();

        xhr = new XMLHttpRequest();
        xhr.open("get", FacultiesApi.getFacultyUrl(facultyShortName), true);
        xhr.onload = function () {
            var data = JSON.parse(xhr.responseText);
            setFacultyName(data.fullName);
        }.bind(this);
        xhr.send();
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
                    $('#facultyPlansCreateModalWindow').modal('hide');
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
            $('#facultyPlansCreateModalWindow').modal('hide');
        }
    }
    React.useEffect(() => {
        loadData();
    }, [year])

    return (
        <React.Suspense>
            <h1 className="input-group-lg text-center">План приёма на <input min="0" type="number" onChange={onCangeYear} className="form-control d-inline" value={year} style={{ width: 100 }} /> год</h1>
            <hr />
            <ModalWindowCreate onCreate={onCreateFacultyPlans} />
            <div className="ps-lg-4 pe-lg-4 position-relative">
                <h4>{facultyName}</h4>
                <UpdateSpecialityPlansList year={year} plans={plans} errors={errors} onChange={onChangePlans} />
            </div>
            <div className="col text-center pt-4">
                <button type="button" className="btn btn-outline-success btn-lg" data-bs-toggle="modal" data-bs-target="#facultyPlansCreateModalWindow" data-bs-year={year} data-bs-facultyshortname={facultyShortName}>
                    Создать
                </button>
                <Link type="button" className="btn btn-outline-secondary btn-lg" to={"/Faculties/" + facultyShortName}>В факультет</Link>
            </div>
        </React.Suspense>
    );
}