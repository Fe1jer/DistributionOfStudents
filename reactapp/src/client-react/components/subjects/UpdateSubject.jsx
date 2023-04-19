import React from 'react';
import Form from 'react-bootstrap/Form';

export default function UpdateSubject({ errors, subject, onChangeModel }) {
    return (
        <Form.Group>
            <Form.Label className="mb-0">Название</Form.Label><sup>*</sup>
            <Form.Control type="text"
                name="name" value={subject.name ?? ""} onChange={onChangeModel}
                isInvalid={!!errors.name} />
            <Form.Control.Feedback type="invalid">{errors.name}</Form.Control.Feedback>
        </Form.Group>
    );
}