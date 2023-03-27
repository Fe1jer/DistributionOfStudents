import SpecialityPlansList from './SpecialityPlansList.jsx';

import { Link } from 'react-router-dom'
import React from 'react';

export default function FacultyPlans({ facultyFullName, facultyShortName, year, plansForSpecialities }) {
    return <React.Suspense>
        <hr className="mt-4 mx-0" />
        <Link className="nav-link text-success p-0" to={"/Faculties/" + facultyShortName} ><h4>{facultyFullName}</h4></Link>
        <SpecialityPlansList plans={plansForSpecialities} facultyShortName={facultyShortName} year={year} />
    </React.Suspense>;
}