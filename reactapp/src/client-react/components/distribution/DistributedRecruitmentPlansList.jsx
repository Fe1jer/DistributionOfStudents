import DistributedRecruitmentPlan from "../recruitmentPlans/DistributedRecruitmentPlan";
import ModalWindowEditItem from "../recruitmentPlans/ModalWindows/ModalWindowEditItem";

import Table from 'react-bootstrap/Table';
import React, { useState } from 'react';

export default function DistributedRecruitmentPlansList({ plans, reloadPlans }) {
    const [selectedPlanId, setSelectedPlanId] = useState(null);
    const [editShow, setEditShow] = useState(false);

    const handleEditClose = () => {
        setEditShow(false);
        setSelectedPlanId(null);
    };
    const onClickEditSpeciality = (id) => {
        setSelectedPlanId(id);
        setEditShow(true);
    }

    return <div className="shadow">
        <ModalWindowEditItem show={editShow} handleClose={handleEditClose} planId={selectedPlanId} onLoadPlans={reloadPlans} />
        <Table bordered responsive className="mb-0">
            <thead>
                <tr>
                    <th width="20">№</th>
                    <th>Специальность (направление специальности)</th>
                    <th>План приема</th>
                    <th>Проходной балл</th>
                    <th></th>
                </tr>
            </thead>
            <tbody>{
                plans.map((item, index) =>
                    <DistributedRecruitmentPlan key={JSON.stringify(item)} index={index} specialityPlan={item} onClickEdit={onClickEditSpeciality} />
                )}
            </tbody>
        </Table>
    </div>;
}