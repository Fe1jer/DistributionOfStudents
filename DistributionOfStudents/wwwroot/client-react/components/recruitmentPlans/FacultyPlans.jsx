import SpecialityPlansList from './SpecialityPlansList.jsx';

export default function FacultyPlans({ facultyFullName, facultyShortName, year, plansForSpecialities }) {
    return <React.Suspense>
        <hr className="mt-4 mx-0" />
        <a className="nav-link text-success p-0" href={"/Faculties/" + facultyShortName} ><h4>{facultyFullName}</h4></a>
        <SpecialityPlansList plans={plansForSpecialities} facultyShortName={facultyShortName} year={year} />
    </React.Suspense>;
}