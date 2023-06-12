import CreateButton from '../adminButtons/CreateButton.jsx';
import Admission from './Admission.jsx';

import Table from 'react-bootstrap/Table';

export default function AdmissionsList({ admissions, plans, onClickDelete, onClickEdit, onClickCreate, onClickDetails }) {
    return <div className="card shadow">
        <Table responsive className="table mb-0">
            <thead>
                <tr>
                    <th>ФИО</th>
                    <th>Подача заявки</th>
                    <th>Сумма баллов</th>
                    <th>Приоритет (№ спец.)</th>
                    <th className="text-center" width="80">
                        <CreateButton onClick={() => onClickCreate()} />
                    </th>
                </tr>
            </thead>
            <tbody>{
                admissions.map((item) =>
                    <Admission key={item.student.surname + item.student.name + item.student.patronymic} admission={item} plans={plans}
                        onClickDelete={onClickDelete} onClickEdit={onClickEdit} onClickDetails={onClickDetails} />
                )}
            </tbody>
        </Table>
    </div>;
} 