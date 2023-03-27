import React from 'react';
import { Link } from 'react-router-dom'

import ArchiveApi from "../../api/ArchiveApi.js";

export default function ArchiveYearsPage() {
    const [years, setYears] = React.useState([]);
    const [loading, setLoading] = React.useState(true);
    var numbers = [1, 2, 3, 4, 5]

    const loadData = () => {
        var xhr = new XMLHttpRequest();
        xhr.open("get", ArchiveApi.getYearsUrl(), true);
        xhr.onload = function () {
            var data = JSON.parse(xhr.responseText);
            setYears(data);
            setLoading(false);
        }.bind(this);
        xhr.send();
    }
    React.useEffect(() => {
        loadData();
    }, [])

    if (loading) {
        return <React.Suspense>
            <h1 className="text-center"><span className="placeholder w-25"></span></h1>
            <hr className="mt-3 mx-0" />
            <div className="ps-lg-4 pe-lg-4 position-relative">
                {numbers.map((number) =>
                    <a key={"ArchiveYear" + number} className="nav-link text-success p-0 mb-3 placeholder-glow"><span className="placeholder w-25"></span></a>
                )}
            </div>
        </React.Suspense>
    }
    else {
        return (
            <React.Suspense>
                <h1 className="text-center">Архив</h1>
                <hr className="mt-3 mx-0" />
                <div className="ps-lg-4 pe-lg-4 position-relative">
                    {years.map((year) =>
                        <Link key={year} className="nav-link text-success p-0 mb-3" to={"/Archive/" + year}><h4>Проходные баллы в БНТУ в {year} году</h4></Link>
                    )}
                </div>
            </React.Suspense>
        );
    }
}