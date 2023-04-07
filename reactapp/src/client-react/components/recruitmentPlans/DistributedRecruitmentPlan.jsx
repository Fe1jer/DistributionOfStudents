export default function GroupRecruitmentPlan({ index, specialityPlan }) {
    return <tr className="align-middle">
        <td>{index + 1}</td>
        <td>{specialityPlan.speciality.directionName ?? specialityPlan.speciality.fullName}</td>
        <td>{specialityPlan.count}</td>
        <td>{specialityPlan.passingScore}</td>
    </tr>;
}