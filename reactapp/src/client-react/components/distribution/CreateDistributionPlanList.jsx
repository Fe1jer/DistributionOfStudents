import CreateDistributionPlanSutent from './CreateDistributionPlanSutent.jsx';

import Form from 'react-bootstrap/Form';
import Card from 'react-bootstrap/Card';
import Table from 'react-bootstrap/Table';
import React, { useState, useEffect } from 'react';

export default function CreateDistributionPLanLst({ plan, index, error, onChangeSelectedStudents }) {
    const [updatedSelectedStudents, setUpdatedSelectedStudents] = useState(null);

    const isDistributedStudent = (enrolledStudent) => {
        return plan.count < plan.enrolledStudents.length && generalStudentScore(enrolledStudent) == plan.passingScore;
    }
    const _formGroupErrors = (errors) => {
        if (errors) {
            return (<React.Suspense>{
                errors.map((error) => <React.Suspense key={error}><span>{error}</span><br></br></React.Suspense>)}
            </React.Suspense>);
        }
    }
    const generalStudentScore = (enrolledStudent) => {
        var generalScore = 0;
        enrolledStudent.student.admissions[0].studentScores.forEach(element => generalScore += element.score);
        return generalScore + enrolledStudent.student.gps;
    }

    const onChangeSelectedStudent = (studentIndex, value) => {
        var updateSelectedStudentsTemp = updatedSelectedStudents;
        updateSelectedStudentsTemp[studentIndex].IsDistributed = value;
        setUpdatedSelectedStudents(updateSelectedStudentsTemp);
        onChangeSelectedStudents(index, updatedSelectedStudents);
    }

    const initializationSelectedStudents = () => {
        var initializatedSelectedStudents = plan.enrolledStudents.map((item) => {
            return { studentId: item.student.id, isDistributed: !isDistributedStudent(item)}
        });

        setUpdatedSelectedStudents(initializatedSelectedStudents);
        onChangeSelectedStudents(index, initializatedSelectedStudents);
    };

    useEffect(() => {
        initializationSelectedStudents();
    }, [])

    return (
        <React.Suspense>
            <Form.Group>
                <Form.Control className="d-none" plaintext readOnly isInvalid={error} />
                <Form.Control.Feedback className="mt-0" type="invalid">{error ? _formGroupErrors(error) : ""}</Form.Control.Feedback>
            </Form.Group>
            <Card className="shadow">
                <Table responsive className="table mb-0">
                    <thead>
                        <tr>
                            <th width="20">№</th>
                            <th></th>
                            <th width="30%">ФИО</th>
                            <th width="30%">Баллы по ЦТ(ЦЭ)</th>
                            <th width="10%">Аттестат</th>
                            <th width="10%">Сумма</th>
                            <th width="20%">Другое</th>
                        </tr>
                    </thead>
                    <tbody>{
                        plan.enrolledStudents.map((item, index) =>
                            <CreateDistributionPlanSutent key={item.student.name + item.student.surname + item.student.patronymic}
                                index={index} enrolledStudent={item} onChangeSelectedStudent={onChangeSelectedStudent} isDistributed={!isDistributedStudent(item)} />
                        )}
                    </tbody>
                </Table>
            </Card>
        </React.Suspense>);
}