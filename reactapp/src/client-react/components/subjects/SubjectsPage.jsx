import SubjectsList from "./SubjectsList.jsx";
import { SubjectDeleteForm, SubjectUpdateForm } from './SubjectForms.jsx';
import TablePreloader from "../TablePreloader.jsx";

import SubjectsApi from "../../api/SubjectsApi.js";

import React from 'react';

export default function SubjectsPage() {
    const [subjects, setSubjects] = React.useState([]);
    const [loading, setLoading] = React.useState(true);
    // загрузка данных
    const loadData = () => {
        var xhr = new XMLHttpRequest();
        xhr.open("get", SubjectsApi.getSubjectsUrl(), true);
        xhr.onload = function () {
            var data = JSON.parse(xhr.responseText);
            setSubjects(data);
            setLoading(false);
        }.bind(this);
        xhr.send();
    }
    React.useEffect(() => {
        loadData();
    }, [])
    if (loading) {
        return <React.Suspense>
            <h1 className="text-center placeholder-glow"><span className="placeholder w-50"></span></h1>
            <hr />
            <TablePreloader />
        </React.Suspense>
    }
    else {
        return (
            <React.Suspense>
                <h1 className="text-center">Список предметов для сдачи ЦТ и ЦЭ</h1>
                <hr />
                <SubjectUpdateForm onLoadData={loadData} />
                <SubjectDeleteForm onLoadData={loadData} />
                <div className="ps-lg-4 pe-lg-4 position-relative">
                    <SubjectsList key={subjects} subjects={subjects} />
                </div>
            </React.Suspense>
        );
    }
}
