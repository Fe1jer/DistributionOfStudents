import CreateDistributionPlanSutent from './CreateDistributionPlanSutent.jsx';

import Form from 'react-bootstrap/Form';
import Card from 'react-bootstrap/Card';
import Table from 'react-bootstrap/Table';
import React from 'react';

export default function CreateDistributionPLanLst({ planIndex, plan, distributedPlan, errors, handleChange }) {
    const isControversial = (enrolledStudent) => {
        return plan.count < plan.enrolledStudents.length && enrolledStudent.score === plan.passingScore;
    }

    return (
        <React.Suspense>
            <Form.Group>
                <Form.Control className="d-none" plaintext readOnly isInvalid={errors} />
                <Form.Control.Feedback className="mt-0" type="invalid">{errors ? errors.distributedStudents : ""}</Form.Control.Feedback>
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
                        distributedPlan.distributedStudents.map((item, index) =>
                            <CreateDistributionPlanSutent key={item.studentId}
                                index={index} planIndex={planIndex} enrolledStudent={item} handleChange={handleChange} isControversial={isControversial(plan.enrolledStudents[index])} />
                        )}
                    </tbody>
                </Table>
            </Card>
        </React.Suspense>);
}