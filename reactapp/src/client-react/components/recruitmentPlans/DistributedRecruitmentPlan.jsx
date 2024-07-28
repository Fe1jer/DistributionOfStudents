import React, { useState } from 'react';
import EditButton from '../adminButtons/EditButton';

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
            <td rowSpan={isHaveTarget ? 2 : 1}>{specialityPlan.specialityName}</td>
            <td>{specialityPlan.count}</td>
            <td>{specialityPlan.passingScore}</td>
            <td rowSpan={isHaveTarget ? 2 : 1} className="text-center">
                <div className="d-inline-flex">
                    <EditButton size="sm" onClick={() => onClickEdit(specialityPlan.id)} />
                </div>
            </td>
        </tr>
        {_showTargetInfo()}
    </React.Suspense>;
}