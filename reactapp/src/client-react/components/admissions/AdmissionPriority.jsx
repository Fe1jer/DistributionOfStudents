import OverlayTrigger from 'react-bootstrap/OverlayTrigger';
import Tooltip from 'react-bootstrap/Tooltip';
import * as React from "react";

export default function AdmissionSpetialitiesPriority({ admission, specialityPriority, plans, index }) {
    const numOfPlan = plans.findIndex(x => x.id === specialityPriority.recruitmentPlan.id);

    const _showPriority = () => {
        if (plans[numOfPlan].enrolledStudents.some(s => s.student.id === admission.student.id)) {
            if (plans[numOfPlan].count < plans[numOfPlan].enrolledStudents.length && admission.score == plans[numOfPlan].passingScore) {
                return <label className="bg-warning mx-1 px-1">
                    {numOfPlan + 1}
                </label>
            }
            else {
                return <label className="bg-success mx-1 px-1 text-light">
                    {numOfPlan + 1}
                </label>
            }
        }
        else {
            return <label className="mx-1">
                {numOfPlan + 1}
            </label>
        }
    }

    return (
        <React.Suspense key={admission.student.surname + admission.student.name + admission.student.patronymic + specialityPriority.priority} >
            <OverlayTrigger
                placement="top"
                overlay={<Tooltip>{plans[numOfPlan].speciality.directionName ?? plans[numOfPlan].speciality.fullName}</Tooltip>}>
                {_showPriority()}
            </OverlayTrigger>
            <label>
                {index + 1 === admission.specialityPriorities.length ? "" : "->"}
            </label>
        </React.Suspense >
    );
}