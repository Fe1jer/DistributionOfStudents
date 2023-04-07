import DistributedRecruitmentPlan from "./DistributedRecruitmentPlan.jsx";

import Table from 'react-bootstrap/Table';
import Card from 'react-bootstrap/Card';
import React from 'react';


export default function DistribitedRecruitmentPlansList({ plans }) {
    return <Card className="shadow">
        <Table responsive className="mb-0">
            <thead>
                <tr>
                    <th width="20">№</th>
                    <th>Специальность (направление специальности)</th>
                    <th>План приема</th>
                    <th>Проходной балл</th>
                </tr>
            </thead>
            <tbody>{
                plans.map((item, index) =>
                    <DistributedRecruitmentPlan key={JSON.stringify(item)} index={index} specialityPlan={item} />
                )}
            </tbody>
        </Table>
    </Card>;
}