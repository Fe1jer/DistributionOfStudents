import UpdateSpecialityPlansList from "./UpdateSpecialityPlansList.jsx";
import ModalWindowEdit from "./ModalWindows/ModalWindowEdit.jsx";

import RecruitmentPlansApi from "../../api/RecruitmentPlansApi.js";
import FacultiesApi from "../../api/FacultiesApi.js";

import { Link, useParams, useNavigate } from 'react-router-dom'
import React, { useState } from 'react';

export default function EditFacultyPlansPage() {
    const params = useParams();
    const facultyShortName = params.shortName;
    const year = params.year;

    const [plans, setPlans] = useState([]);
    const [facultyName, setFacultyName] = useState("");
    const [errors, setErrors] = useState({});
    const [editShow, setEditShow] = useState(false);

    const navigate = useNavigate();

    const loadData = () => {
        var xhr1 = new XMLHttpRequest();
        xhr1.open("get", RecruitmentPlansApi.getFacultyRecruitmentPlansUrl(facultyShortName, year), true);
        xhr1.onload = function () {
            var data = JSON.parse(xhr1.responseText);
            setPlans(data);
        }.bind(this);
        xhr1.send();

        var xhr2 = new XMLHttpRequest();
        xhr2.open("get", FacultiesApi.getFacultyUrl(facultyShortName), true);
        xhr2.onload = function () {
            var data = JSON.parse(xhr2.responseText);
            setFacultyName(data.fullName);
        }.bind(this);
        xhr2.send();
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
                    handleEditClose();
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
            handleEditClose();
        }
    }
    const handleEditClose = () => {
        setEditShow(false);
    };
    const onClickEditPlans = () => {
        setEditShow(true);
    }

    React.useEffect(() => {
        loadData();
    }, [])

    return (
        <React.Suspense>
            <h1 className="input-group-lg text-center">План приёма на {year} год</h1>
            <hr />
            <ModalWindowEdit show={editShow} handleClose={handleEditClose} onEditPlans={onEditFacultyPlans} facultyShortName={facultyShortName} year={year} />
            <div className="ps-lg-4 pe-lg-4 position-relative">
                <h4>{facultyName}</h4>
                <UpdateSpecialityPlansList year={year} plans={plans} errors={errors} onChange={onChangePlans} />
            </div>
            <div className="col text-center pt-4">
                <button type="button" className="btn btn-outline-success btn-lg" onClick={() => onClickEditPlans()}>
                    Сохранить
                </button>
                <Link type="button" className="btn btn-outline-secondary btn-lg" to={"/Faculties/" + facultyShortName}>В факультет</Link>
            </div>
        </React.Suspense>
    );
}