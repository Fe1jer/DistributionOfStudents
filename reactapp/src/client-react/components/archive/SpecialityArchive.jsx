import React, { useState } from 'react';

export default function SpecialityArchive({ specialityArchive, number, countOfSpecialities, competition }) {
    const [isHaveTarget] = useState(!!specialityArchive.target);
    console.log(specialityArchive);
    var competitionTd = null;
    if (number == 0) {
        competitionTd =
            (<td className="text-center" rowSpan={countOfSpecialities} >{competition}</td>);
    }
    const _showTargetInfo = () => {
        if (isHaveTarget) {
            return <tr className="align-middle">
                <td>{specialityArchive.target ? specialityArchive.target + "(цел.)" : null}</td>
                <td>{specialityArchive.targetPassingScore}</td>
            </tr>
        }
    }

    return <React.Suspense>
        <tr className="align-middle">
            <td rowSpan={isHaveTarget ? 2 : 1}>{specialityArchive.speciality.directionCode ?? specialityArchive.speciality.code}</td>
            <td rowSpan={isHaveTarget ? 2 : 1}>{specialityArchive.speciality.directionName ?? specialityArchive.speciality.fullName}</td>
            <td>{specialityArchive.count}</td>
            {competitionTd}
            <td>{specialityArchive.passingScore}</td>
        </tr>
        {_showTargetInfo()}
    </React.Suspense>;
}