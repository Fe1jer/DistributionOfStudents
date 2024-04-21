import UpdateSpecialityPlan from "./UpdateSpecialityPlan.jsx";

import Table from 'react-bootstrap/Table';
import React from 'react';

export default function UpdateSpecialityPlansList({ plans, errors, onChange }) {
    return <Table responsive bordered className="mb-0">
        <thead>
            <tr>
                <th rowSpan="2">Специальность (направление специальности)</th>
                <th colSpan="4">Дневная форма получения образования</th>
                <th colSpan="4">Заочная форма получения образования</th>
            </tr>
            <tr>
                <th width="85">Бюджет</th>
                <th width="85">Платное</th>
                <th width="85">Бюджет сокр.срок</th>
                <th width="85">Платное сокр.срок</th>
                <th width="85">Бюджет</th>
                <th width="85">Платное</th>
                <th width="85">Бюджет сокр.срок</th>
                <th width="85">Платное сокр.срок</th>
            </tr>
        </thead>
        <tbody>{
            plans.map((item, index) =>
                <UpdateSpecialityPlan key={item.fullName} index={index} specialityPlan={item} errors={errors[index] ?? {}} handleChange={onChange} />
            )}
        </tbody>
    </Table>;
}