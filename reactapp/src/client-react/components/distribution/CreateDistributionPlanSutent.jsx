import Form from 'react-bootstrap/Form';

import React from "react";

export default function CreateDistributionPLanStudent({ student, enrolledStudent, handleChange, index, planIndex, isControversial }) {
    const generalScore = () => {
        var generalScore = 0;
        student.admissions[0].studentScores.forEach(element => generalScore += element.score);
        return generalScore + student.gps;
    }

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
                    {student.surname} {student.name}  {student.patronymic}
                </label>
            </td>
            <td>
                {student.admissions[0].studentScores.map((item) =>
                    <p key={item.subject.name} className="m-0" style={{ lineHeight: "18px" }}>
                        {item.subject.name} : {item.score}
                    </p>
                )}
            </td>
            <td>{student.gps}</td>
            <td>{generalScore()}</td>
            <td>
            </td>
        </tr >
    );
}