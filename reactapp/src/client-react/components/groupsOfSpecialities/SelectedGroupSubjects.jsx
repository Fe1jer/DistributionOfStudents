import Form from 'react-bootstrap/Form';
import Card from 'react-bootstrap/Card';

import React from 'react';

export default function SelectedGroupSubjects({ onChange, errors, subjects }) {
    return (
        <Card className="px-2 mt-3">
            <h5 className="text-center mb-0">Предметы, по которым нужны сертификаты<sup>*</sup></h5>
            <Form.Group style={{ textAlign: "-webkit-center" }}>
                <Form.Control className="d-none" plaintext readOnly isInvalid={!!errors} />
                <Form.Control.Feedback className="mt-0" type="invalid">{errors}</Form.Control.Feedback>
            </Form.Group>
            <hr className="mt-2" />{
                subjects.map((item, index) =>
                    <Form.Check key={item.name} className="mb-1"
                        name={"subjects[" + index + "].isSelected"}
                        type="checkbox"
                        id={item.id}
                        label={item.name}
                        checked={item.isSelected}
                        onChange={onChange}
                        isInvalid={errors}
                    />
                )}
        </Card>
    );
}