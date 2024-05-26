import SpecialityPlan from './SpecialityPlan.jsx';

import DeleteButton from '../adminButtons/DeleteButton';
import EditButton from '../adminButtons/EditButton';
import Table from 'react-bootstrap/Table';

import { useNavigate } from 'react-router-dom'

export default function SpecialityPlansList({ shortName, year, plans, onClickDelete }) {
    const navigate = useNavigate();

    return <div className="shadow">
        <Table responsive bordered className="mb-0">
            <thead>
                <tr>
                    <th rowSpan="2">Специальность (направление специальности)</th>
                    <th colSpan="4">Дневная форма получения образования</th>
                    <th colSpan="4">Заочная форма получения образования</th>
                    <th rowSpan="2" width="60" className="text-center align-middle">
                        <EditButton onClick={() => navigate("/Faculties/" + shortName + "/RecruitmentPlans/" + year + "/Edit")} />
                        <DeleteButton onClick={() => onClickDelete(shortName, year)} />
                    </th>
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
                plans.map((item) =>
                    <SpecialityPlan key={item.fullName} specialityPlan={item} />
                )}
            </tbody>
        </Table>
    </div>;
}