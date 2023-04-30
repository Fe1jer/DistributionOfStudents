import FacultyArchive from './FacultyArchive.jsx';

import TablePreloader from "../TablePreloader.jsx";

import ArchiveService from "../../services/Archive.service.js";

import React from 'react';
import { useParams } from 'react-router-dom'

export default function FacultiesArchivePlans() {
    const params = useParams();
    const year = params.year;
    const form = params.form;

    const [facultiesArchive, setFacultiesArchive] = React.useState([]);
    const [loading, setLoading] = React.useState(true);
    var numbers = [1, 2]

    const loadData = async () => {
        const data = await ArchiveService.httpGetArchveByYearAndForm(year, form);
        setFacultiesArchive(data);
        setLoading(false);
    }

    React.useEffect(() => {
        loadData();
    }, [])

    if (loading) {
        return (
            <React.Suspense>
                <h1 className="text-center placeholder-glow"><span className="placeholder w-100"></span></h1>
                <div id="content" className="ps-lg-4 pe-lg-4 position-relative">{
                    numbers.map((item) =>
                        <React.Suspense key={"FacultiesArchivePreloader" + item} >
                            <hr className="mt-4 mx-0" />
                            <p className="placeholder-glow"><span className="placeholder w-25"></span></p>
                            <TablePreloader />
                        </React.Suspense>
                    )}
                </div>
            </React.Suspense>
        );
    }
    else {
        return (
            <React.Suspense>
                <h1 className="text-center">{form} за {year} год</h1>
                <div className="ps-lg-4 pe-lg-4 position-relative">{
                    facultiesArchive.map((item) =>
                        <FacultyArchive key={item.facultyFullName + year + form} facultyArchive={item} />
                    )}
                </div>
            </React.Suspense>
        );
    }
}