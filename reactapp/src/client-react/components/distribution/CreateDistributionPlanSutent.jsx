import Form from 'react-bootstrap/Form';

import React from "react";

export default function CreateDistributionPLanStudent({ enrolledStudent, handleChange, index, planIndex, isControversial }) {

    return (
        <tr className="align-middle">
            <td>{index + 1}</td>
            <td>
                <Form.Group>
                    <Form.Check name={"distributedPlans[" + planIndex + "].distributedStudents[" + index + "].isDistributed"}
                        type="checkbox" checked={enrolledStudent.isDistributed} readOnly
                        onChange={isControversial ? handleChange : null} />
                </Form.Group>
            </td>
            <td>
                <label className={isControversial ? "bg-warning px-1" : "px-1"}>
                    {enrolledStudent.student.surname} {enrolledStudent.student.name}  {enrolledStudent.student.patronymic}
                </label>
            </td>
            <td>
                {enrolledStudent.studentScores.map((item) =>
                    <p key={item.subject.name} className="m-0" style={{ lineHeight: "18px" }}>
                        {item.subject.name} : {item.score}
                    </p>
                )}
            </td>
            <td>{enrolledStudent.student.gpa}</td>
            <td>{enrolledStudent.score}</td>
            <td>
            </td>
        </tr >
    );
}