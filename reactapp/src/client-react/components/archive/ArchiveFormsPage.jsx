import React from 'react';
import { Link, useParams } from 'react-router-dom'

import ArchiveService from "../../services/Archive.service.js";
import useDocumentTitle from '../useDocumentTitle.jsx';

export default function ArchiveFormsPage() {
    const params = useParams();
    const year = params.year;
    useDocumentTitle("Архив " + year);

    const [forms, setForms] = React.useState([]);
    const [loading, setLoading] = React.useState(true);
    var numbers = [1, 2, 3, 4, 5]

    const loadData = async () => {
        const data = await ArchiveService.httpGetArchveForms(year);
        setForms(data);
        setLoading(false);
    }
    React.useEffect(() => {
        loadData();
    }, [])

    if (loading) {
        return <React.Suspense>
            <h1 className="text-center placeholder-glow"><span className="placeholder w-25"></span></h1>
            <hr className="mt-3 mx-0" />
            <div className="ps-lg-4 pe-lg-4 position-relative">
                {numbers.map((number) =>
                    <Link to="#" key={"ArchiveForm" + number} className="nav-link text-success p-0 mb-3 placeholder-glow"><span className="placeholder w-75"></span></Link>
                )}
            </div>
        </React.Suspense>
    }
    else {
        return (
            <React.Suspense>
                <h1 className="text-center">Архив за {year} год</h1>
                <hr className="mt-3 mx-0" />
                <div className="ps-lg-4 pe-lg-4 position-relative">
                    {forms.map((form) =>
                        <Link key={form} className="nav-link text-success p-0 mb-3" to={"/Archive/" + year + "/" + form}><h4>{form}</h4></Link>
                    )}
                </div>
            </React.Suspense>
        );
    }
}