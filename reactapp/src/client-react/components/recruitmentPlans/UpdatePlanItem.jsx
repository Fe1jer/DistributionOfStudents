import Form from 'react-bootstrap/Form';
import Col from 'react-bootstrap/Col';
import Row from 'react-bootstrap/Row';

import React from 'react';

export default function UpdatePlanItem({ values, errors, handleChange }) {
    return <Row>
        <Form.Group as={Col} sm="6">
            <Form.Label className="mb-0">Всего</Form.Label><sup>*</sup>
            <Form.Control type="number"
                name="count" value={values.count} onChange={handleChange}
                isInvalid={errors.count} />
            <Form.Control.Feedback type="invalid">{errors.count}</Form.Control.Feedback>
        </Form.Group>
        <Form.Group as={Col} sm="6">
            <Form.Label className="mb-0">Целевое</Form.Label><sup>*</sup>
            <Form.Control type="number"
                name="target" value={values.target} onChange={handleChange}
                isInvalid={errors.target} />
            <Form.Control.Feedback type="invalid">{errors.target}</Form.Control.Feedback>
        </Form.Group>
    </Row>;
}