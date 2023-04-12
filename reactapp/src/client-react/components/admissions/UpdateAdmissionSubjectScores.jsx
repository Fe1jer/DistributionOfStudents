import Form from 'react-bootstrap/Form';

import React from 'react';

export default function UpdateAdmissionSubjectScores({ onChangeModel, studentScores, modelErrors }) {
    const [updatedStudentScores, setUpdatedStudentScores] = React.useState(studentScores);

    const onChangeStudentScores = (e) => {
        var updateStudentScoresTemp = studentScores;
        var item = updateStudentScoresTemp.find(element => element.subject.name == e.target.id);
        var index = updateStudentScoresTemp.indexOf(item);
        updateStudentScoresTemp[index].score = e.target.value;
        setUpdatedStudentScores(updateStudentScoresTemp.slice());
    }
    React.useEffect(() => {
        onChangeModel(updatedStudentScores);
    }, [updatedStudentScores]);
    const _formGroupErrors = (errors) => {
        if (errors) {
            return (<React.Suspense>{
                errors.map((error) =>
                    <React.Suspense key={error}><span>{error}</span><br></br></React.Suspense>
                )}
            </React.Suspense>);
        }
    }
    return (
        <React.Suspense>
            <h5 className="text-center">Баллы по ЦТ(ЦЭ)</h5>
            <Form.Group style={{ textAlign: "-webkit-center" }}>
                <Form.Control className="d-none" plaintext readOnly isInvalid={modelErrors ? !!modelErrors["StudentScores"] : false} />
                <Form.Control.Feedback className="mt-0" type="invalid">{modelErrors ? _formGroupErrors(modelErrors["StudentScores"]) : ""}</Form.Control.Feedback>
            </Form.Group>{
                studentScores.map((item, index) =>
                    <Form.Group key={item.subject.name} className="pb-1">
                        <Form.Label className="mb-0">{item.subject.name}</Form.Label><sup>*</sup>
                        <Form.Control type="number" min={0} id={item.subject.name}
                            required name="gps" value={item.score ?? ""} onChange={onChangeStudentScores}
                            isInvalid={modelErrors ? !!modelErrors["StudentScores[" + index + "].Score"] : false} />
                        <Form.Control.Feedback className="mt-0" type="invalid">{modelErrors ? _formGroupErrors(modelErrors["StudentScores[" + index + "].Score"]) : ""}</Form.Control.Feedback>
                    </Form.Group>
                )}
        </React.Suspense>
    );
}