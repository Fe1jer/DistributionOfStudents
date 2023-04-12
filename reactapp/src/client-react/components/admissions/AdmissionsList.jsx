import Admission from './Admission.jsx';

import Button from 'react-bootstrap/Button';
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
                        <Button variant="empty" className="text-success p-0" onClick={() => onClickCreate()}>
                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" className="bi bi-plus-circle-fill suc" viewBox="0 0 16 16">
                                <path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zM8.5 4.5a.5.5 0 0 0-1 0v3h-3a.5.5 0 0 0 0 1h3v3a.5.5 0 0 0 1 0v-3h3a.5.5 0 0 0 0-1h-3v-3z" />
                            </svg>
                        </Button >
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