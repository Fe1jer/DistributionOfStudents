import Form from 'react-bootstrap/Form';

import React, { useState } from "react";

export default function CreateDistributionPLanStudent({ enrolledStudent, onChangeSelectedStudent, index, isDistributed }) {
    const [selected, setSelected] = useState(isDistributed);

    const onUpdateSelectedStudent = (e) => {
        setSelected(e.target.checked);
        onChangeSelectedStudent(index, e.target.checked)
    }

    const generalScore = () => {
        var generalScore = 0;
        enrolledStudent.student.admissions[0].studentScores.forEach(element => generalScore += element.score);
        return generalScore + enrolledStudent.student.gps;
    }

    return (
        <tr className="align-middle">
            <td>{index + 1}</td>
            <td>
                <Form.Group>
                    <Form.Check type="checkbox" checked={selected} readOnly onChange={!isDistributed ? onUpdateSelectedStudent : null} />
                </Form.Group>
            </td>
            <td>
                <label className={!isDistributed ? "bg-warning px-1" : "px-1"}>
                    {enrolledStudent.student.surname} {enrolledStudent.student.name}  {enrolledStudent.student.patronymic}
                </label>
            </td>
            <td>
                {enrolledStudent.student.admissions[0].studentScores.map((item) =>
                    <p key={item.subject.name} className="m-0" style={{ lineHeight: "18px" }}>
                        {item.subject.name} : {item.score}
                    </p>
                )}
            </td>
            <td>{enrolledStudent.student.gps}</td>
            <td>{generalScore()}</td>
            <td>
            </td>
        </tr >
    );
}