import React, { useState } from 'react';
import Button from 'react-bootstrap/Button';

export default function GroupRecruitmentPlan({ index, specialityPlan, onClickEdit }) {
    const [isHaveTarget] = useState(!!specialityPlan.target);

    const _showTargetInfo = () => {
        if (isHaveTarget) {
            return <tr className="align-middle">
                <td>{specialityPlan.target ? specialityPlan.target + "(цел.)" : null}</td>
                <td>{specialityPlan.targetPassingScore}</td>
            </tr>
        }
    }

    return <React.Suspense>
        <tr className="align-middle">
            <td rowSpan={isHaveTarget ? 2 : 1}>{index + 1}</td>
            <td rowSpan={isHaveTarget ? 2 : 1}>{specialityPlan.speciality.directionName ?? specialityPlan.speciality.fullName}</td>
            <td>{specialityPlan.count}</td>
            <td>{specialityPlan.passingScore}</td>
            <td rowSpan={isHaveTarget ? 2 : 1} className="text-center">
                <div className="d-inline-flex">
                    <Button variant="outline-success" size="sm" className="py-1" onClick={() => onClickEdit(specialityPlan.id)}>
                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" className="bi bi-pencil-square" viewBox="0 0 16 16">
                            <path d="M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z" />
                            <path fillRule="evenodd" d="M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5v11z" />
                        </svg>
                    </Button>
                </div>
            </td>
        </tr>
        {_showTargetInfo()}
    </React.Suspense>;
}