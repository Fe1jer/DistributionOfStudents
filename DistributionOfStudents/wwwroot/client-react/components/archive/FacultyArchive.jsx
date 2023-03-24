import GroupOfSpecialitiesArchive from './GroupOfSpecialitiesArchive.jsx';

export default function FacultyArchive({ facultyArchive }) {
    return <React.Suspense>
        <hr className="mt-4 mx-0" />
        <h4>{facultyArchive.facultyFullName}</h4>
        <div className="shadow">
            <GroupOfSpecialitiesArchive facultyArchive={facultyArchive} />
        </div>
    </React.Suspense>;
}