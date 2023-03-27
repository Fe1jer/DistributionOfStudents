﻿import FacultyPlans from './FacultyPlans.jsx';
import FacultiesPlansPreloader from "./FacultiesPlansPreloader.jsx";
import ModalWindowDelete from "./ModalWindows/ModalWindowDelete.jsx";

import RecruitmentPlansApi from "../../api/RecruitmentPlansApi.js";

import React from 'react';

export default function FacultiesPlans() {
    const [facultiesPlans, setFacultiesPlans] = React.useState([]);
    const [loading, setLoading] = React.useState(true);
    const [year, setYear] = React.useState(0);
    var numbers = [1, 2]

    const loadData = () => {
        var xhr = new XMLHttpRequest();
        xhr.open("get", RecruitmentPlansApi.getRecruitmentPlansUrl(), true);
        xhr.onload = function () {
            var data = JSON.parse(xhr.responseText);
            setFacultiesPlans(data);
            setLoading(false);
            const year = data.lenght != 0 ? Math.max(...data.map(o => o.year)) : 0;
            setYear(year);
        }.bind(this);
        xhr.send();
    }

    const onDeleteFaculty = (facultyShortName) => {
        if (year != "0") {
            var xhr = new XMLHttpRequest();
            xhr.open("delete", RecruitmentPlansApi.getDeleteUrl(facultyShortName, year), true);
            xhr.setRequestHeader("Content-Type", "application/json")
            xhr.onload = function () {
                if (xhr.status === 200) {
                    loadData();
                }
            }.bind(this);
            xhr.send();
        }
        $('#facultyPlansDeleteModalWindow').modal('hide');
    }

    React.useEffect(() => {
        loadData();
    }, [])

    if (loading) {
        return (
            <React.Suspense>
                <h1 className="text-center"><span className="placeholder w-25"></span></h1>
                <div id="content" className="ps-lg-4 pe-lg-4 position-relative">{
                    numbers.map((item) =>
                        <FacultiesPlansPreloader key={"FacultiesPlansPreloader" + item} />
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
                    <ModalWindowDelete onDelete={onDeleteFaculty} />{
                        facultiesPlans.map((item) =>
                            <FacultyPlans key={item.facultyShortName} facultyFullName={item.facultyFullName} facultyShortName={item.facultyShortName} year={item.year} plansForSpecialities={item.plansForSpecialities} />
                        )}
                </div>
            </React.Suspense>
        );
    }
}