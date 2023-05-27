import SubjectsList from "./SubjectsList.jsx";
import TablePreloader from "../TablePreloader.jsx";

import SubjectsService from "../../services/Subjects.service.js";

import React from 'react';
import useDocumentTitle from "../useDocumentTitle.jsx";

export default function SubjectsPage() {
    useDocumentTitle("Предметы");

    const [subjects, setSubjects] = React.useState([]);
    const [loading, setLoading] = React.useState(true);
    // загрузка данных
    const loadData = async () => {
        try {
            setLoading(true);
            const subjectsData = await SubjectsService.httpGet();
            setSubjects(subjectsData);
        } catch (err) {
            console.error(err.message);
        } finally {
            setLoading(false);
        }
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
                <div className="ps-lg-4 pe-lg-4 position-relative">
                    <SubjectsList subjects={subjects} onLoadSubjects={loadData} />
                </div>
            </React.Suspense>
        );
    }
}
