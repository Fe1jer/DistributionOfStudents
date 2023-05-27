import Student from './Student.jsx';

import Table from 'react-bootstrap/Table';

export default function StudentsList({ students }) {
    return <div className="card shadow">
        <Table responsive className="table mb-0">
            <thead>
                <tr>
                    <th>ФИО</th>
                    <th>Факультет</th>
                    <th>Форма образования</th>
                </tr>
            </thead>
            <tbody>{
                students.map((item) =>
                    <Student key={item.fullName} student={item} />
                )}
            </tbody>
        </Table>
    </div>;
}