import Form from 'react-bootstrap/Form';
import Card from 'react-bootstrap/Card';

import React from 'react';

export default function SelectedGroupSpecialities({ onChange, errors, specialities }) {
    return (
        <Card className="px-2 mt-3">
            <h5 className="text-center mb-0">Специальности, составляющие общий конкурс<sup>*</sup></h5>
            <Form.Group style={{ textAlign: "-webkit-center" }}>
                <Form.Control className="d-none" plaintext readOnly isInvalid={!!errors} />
                <Form.Control.Feedback className="mt-0" type="invalid">{errors}</Form.Control.Feedback>
            </Form.Group>
            <hr className="mt-2" />{
                specialities.map((item, index) =>
                    <Form.Check key={item.specialityName} className="mb-1"
                        name={"selectedSpecialities[" + index + "].isSelected"}
                        type="checkbox"
                        id={item.specialityId}
                        label={item.specialityName}
                        checked={item.isSelected}
                        onChange={onChange}
                        isInvalid={!!errors}
                    />
                )}
        </Card>
    );
}