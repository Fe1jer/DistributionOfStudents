import GroupOfSpecialitiesArchive from './GroupOfSpecialitiesArchive.jsx';

import React from 'react';

export default function FacultyArchive({ facultyArchive }) {
    return <React.Suspense>
        <hr className="mt-4 mx-0" />
        <h4>{facultyArchive.fullName}</h4>
        <div className="shadow">
            <GroupOfSpecialitiesArchive facultyArchive={facultyArchive} />
        </div>
    </React.Suspense>;
}