import Form from 'react-bootstrap/Form';

import React from 'react';

export default function UpdateAdmissionSubjectScores({ onChangeModel, studentScores, errors }) {
    return (
        <React.Suspense>
            <h5 className="text-center">Баллы по ЦТ(ЦЭ)</h5>
            <Form.Group style={{ textAlign: "-webkit-center" }}>
                <Form.Control className="d-none" plaintext readOnly isInvalid={!!errors} />
            </Form.Group>{
                studentScores.map((item, index) =>
                    <Form.Group key={item.subject.name} className="pb-1">
                        <Form.Label className="mb-0">{item.subject.name}</Form.Label><sup>*</sup>
                        <Form.Control name={"studentScores[" + index + "].score"} type="number"
                            value={item.score ?? ""} onChange={onChangeModel}
                            isInvalid={errors ? errors[index] ? !!errors[index].score : false : false} />
                        <Form.Control.Feedback className="mt-0" type="invalid">{errors ? errors[index] ? errors[index].score : "" : ""}</Form.Control.Feedback>
                    </Form.Group>
                )}
        </React.Suspense>
    );
}