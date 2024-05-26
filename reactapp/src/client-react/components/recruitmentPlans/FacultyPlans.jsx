import SpecialityPlansList from './SpecialityPlansList.jsx';

import { Link } from 'react-router-dom'
import React from 'react';

export default function FacultyPlans({ fullName, shortName, year, plansForSpecialities, onClickDelete }) {
    return <React.Suspense>
        <hr className="mt-4 mx-0" />
        <Link className="nav-link text-success p-0" to={"/Faculties/" + shortName} ><h4>{fullName}</h4></Link>
        <SpecialityPlansList plans={plansForSpecialities} shortName={shortName} year={year} onClickDelete={onClickDelete} />
    </React.Suspense>;
}