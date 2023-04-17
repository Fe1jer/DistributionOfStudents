import EnrolledStudent from "./EnrolledStudent.jsx"

import Table from 'react-bootstrap/Table';
import Card from 'react-bootstrap/Card';

import * as React from "react";

export default function EnrolledStudentsList({ enrolledStudents }) {
    return <Card className="shadow">
        <Table responsive className="mb-0">
            <thead>
                <tr>
                    <th width="20">№</th>
                    <th>ФИО</th>
                </tr>
            </thead>
            <tbody>{
                enrolledStudents.map((item, index) =>
                    <EnrolledStudent key={item.student.surname + item.student.name + item.student.patronymic} index={index} student={item.student} />
                )}
            </tbody>
        </Table>
    </Card>;
}