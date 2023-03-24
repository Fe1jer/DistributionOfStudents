export default function SpecialityArchive({ specialityArchive, number, countOfSpecialities, competition }) {
    var competitionTd = null;
    if (number == 0) {
        console.log(countOfSpecialities);
        competitionTd =
            (<td className="text-center" rowSpan={countOfSpecialities} >{competition}</td>);
    }
    return <tr className="align-middle">
        <td>{specialityArchive.speciality.directionCode ?? specialityArchive.speciality.code}</td>
        <td>{specialityArchive.speciality.directionName ?? specialityArchive.speciality.fullName}</td>
        <td>{specialityArchive.count}</td>
        {competitionTd}
        <td>{specialityArchive.passingScore}</td>
    </tr>;
}