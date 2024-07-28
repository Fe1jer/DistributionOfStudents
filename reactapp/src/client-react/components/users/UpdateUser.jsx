import React from 'react';
import Form from 'react-bootstrap/Form';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';

export default function UpdateUser({ errors, user, onChangeModel }) {
    return (

        <React.Suspense>
            <Form.Group>
                <Form.Label className="mb-0">Имя пользователя</Form.Label><sup>*</sup>
                <Form.Control type="text"
                    name="userName" value={user.userName ?? ""} onChange={onChangeModel}
                    isInvalid={!!errors.userName} />
                <Form.Control.Feedback type="invalid">{errors.userName}</Form.Control.Feedback>
            </Form.Group>
            <Row className="mt-2">
                <Form.Group as={Col} sm={4}>
                    <Form.Label className="mb-0">Фамилия</Form.Label><sup>*</sup>
                    <Form.Control type="text"
                        name="surname" value={user.surname ?? ""} onChange={onChangeModel}
                        isInvalid={!!errors.surname} />
                    <Form.Control.Feedback type="invalid">{errors.surname}</Form.Control.Feedback>
                </Form.Group>
                <Form.Group as={Col} sm={4}>
                    <Form.Label className="mb-0">Имя</Form.Label><sup>*</sup>
                    <Form.Control type="text"
                        name="name" value={user.name ?? ""} onChange={onChangeModel}
                        isInvalid={!!errors.name} />
                    <Form.Control.Feedback type="invalid">{errors.name}</Form.Control.Feedback>
                </Form.Group>
                <Form.Group as={Col} sm={4}>
                    <Form.Label className="mb-0">Отчество</Form.Label><sup>*</sup>
                    <Form.Control type="text"
                        name="patronymic" value={user.patronymic ?? ""} onChange={onChangeModel}
                        isInvalid={!!errors.patronymic} />
                    <Form.Control.Feedback type="invalid">{errors.patronymic}</Form.Control.Feedback>
                </Form.Group>
            </Row>
        </React.Suspense>
    );
}