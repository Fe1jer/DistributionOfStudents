import UpdateSpecialityPlansList from "./UpdateSpecialityPlansList.jsx";
import ModalWindowEdit from "./ModalWindows/ModalWindowEdit.jsx";

import RecruitmentPlansApi from "../../api/RecruitmentPlansApi.js";

import { Link, useParams, useNavigate } from 'react-router-dom'
import React from 'react';

export default function EditFacultyPlansPage() {
    const params = useParams();
    const facultyShortName = params.facultyShortName;
    const year = params.year;

    const [plans, setPlans] = React.useState([]);
    const [facultyName, setFacultyName] = React.useState("");
    const [errors, setErrors] = React.useState({});

    const navigate = useNavigate();

    const loadData = () => {
        var xhr = new XMLHttpRequest();
        xhr.open("get", RecruitmentPlansApi.getRecruitmentPlanUrl(facultyShortName, year), true);
        xhr.onload = function () {
            var data = JSON.parse(xhr.responseText);
            setPlans(data.plansForSpecialities);
            setFacultyName(data.facultyFullName);
        }.bind(this);
        xhr.send();
    }
    const onChangePlans = (changedPlans) => {
        setPlans(changedPlans);
    }

    const onEditFacultyPlans = () => {
        if (year != 1) {
            var xhr = new XMLHttpRequest();
            xhr.open("put", RecruitmentPlansApi.getPutUrl(facultyShortName, year), true);
            xhr.setRequestHeader("Content-Type", "application/json")
            xhr.onload = function () {
                if (xhr.status === 200) {
                    $('#facultyPlansEditModalWindow').modal('hide');
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
            $('#facultyPlansEditModalWindow').modal('hide');
        }
    }
    React.useEffect(() => {
        loadData();
    }, [])

    return (
        <React.Suspense>
            <h1 className="input-group-lg text-center">План приёма на {year} год</h1>
            <hr />
            <ModalWindowEdit onEdit={onEditFacultyPlans} />
            <div className="ps-lg-4 pe-lg-4 position-relative">
                <h4>{facultyName}</h4>
                <UpdateSpecialityPlansList year={year} plans={plans} errors={errors} onChange={onChangePlans} />
            </div>
            <div className="col text-center pt-4">
                <button type="button" className="btn btn-outline-success btn-lg" data-bs-toggle="modal" data-bs-target="#facultyPlansEditModalWindow" data-bs-year={year} data-bs-facultyshortname={facultyShortName}>
                    Сохранить
                </button>
                <Link type="button" className="btn btn-outline-secondary btn-lg" to={"/Faculties/" + facultyShortName}>В факультет</Link>
            </div>
        </React.Suspense>
    );
}