import StudentsList from "./StudentsList";
import Search from "../Searh";
import Pagination from "../Pagination";
import TablePreloader from "../TablePreloader";

import StudentsService from "../../services/Students.service";

import { useLocation } from 'react-router-dom';
import React from 'react';
import useDocumentTitle from "../useDocumentTitle";

export default function StudentsPage() {
    useDocumentTitle("Студенты");

    const location = useLocation();
    var searchStudentsParam = new URLSearchParams(location.search).get("searchStudents") ?? "";

    const [students, setStudents] = React.useState([]);
    const [searhText, setSearhText] = React.useState(searchStudentsParam);
    const [currentPage, setCurrentPage] = React.useState(0);
    const [pageLimit, setPageLimit] = React.useState(30);
    const [countSearchStudents, setCountSearchStudents] = React.useState(0);
    const [pageNeighbours] = React.useState(4);
    const [loading, setLoading] = React.useState(true);

    const loadData = async () => {
        const data = await StudentsService.httpGet(searhText, currentPage, pageLimit);
        setStudents(data.students);
        setCountSearchStudents(data.countOfSearchStudents);
        setLoading(false);
    }
    const onSearhChange = (text) => {
        setSearhText(text);
    }
    const onPageChanged = (data) => {
        setCurrentPage(data.currentPage);
        setPageLimit(data.pageLimit);
    }
    React.useEffect(() => {
        loadData();
    }, [searhText, currentPage])

    if (loading) {
        return <React.Suspense>
            <h1 className="text-center placeholder-glow"><span className="placeholder w-25"></span></h1>
            <hr />
            <TablePreloader />
        </React.Suspense>
    }
    else {
        return (
            <React.Suspense>
                <h1 className="text-center">Все заявки абитуриентов</h1>
                <hr />
                <div className="ps-lg-4 pe-lg-4 position-relative">
                    <Search filter={onSearhChange} defaultValue={searhText} className="mb-2" />
                    <StudentsList key={students} students={students} />
                    <Pagination key={countSearchStudents} totalRecords={countSearchStudents} pageLimit={pageLimit} pageNeighbours={pageNeighbours} onPageChanged={onPageChanged} />
                </div>
            </React.Suspense>
        );
    }
}
