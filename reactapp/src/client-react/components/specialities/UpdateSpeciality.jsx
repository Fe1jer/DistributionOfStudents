import React from 'react';
import Form from 'react-bootstrap/Form';
import Col from 'react-bootstrap/Col';
import Row from 'react-bootstrap/Row';

export default function UpdateSpeciality({ errors, speciality, onChangeModel }) {
    const textRef = React.useRef();
    React.useEffect(() => {
        if (textRef && textRef.current) {
            textRef.current.style.height = "0px";
            const taHeight = textRef.current.scrollHeight;
            textRef.current.style.height = taHeight + "px";
        }
    }, [speciality]);
    return (
        <React.Suspense>
            <Form.Group>
                <Form.Label className="mb-0">Специальность</Form.Label><sup>*</sup>
                <Form.Control type="text" name="fullName" value={speciality.fullName ?? ""}
                    onChange={onChangeModel}
                    isInvalid={!!errors.fullName} />
                <Form.Control.Feedback type="invalid">{errors.fullName}</Form.Control.Feedback>
            </Form.Group>
            <Row className="mt-2">
                <Form.Group as={Col} sm={4}>
                    <Form.Label className="mb-0">Аббревиатура</Form.Label>
                    <Form.Control type="text" name="shortName" value={speciality.shortName ?? ""}
                        onChange={onChangeModel}
                        isInvalid={!!errors.shortName} />
                    <Form.Control.Feedback type="invalid">{errors.shortName}</Form.Control.Feedback>
                </Form.Group>
                <Form.Group as={Col} sm={4}>
                    <Form.Label className="mb-0">Код специальности</Form.Label><sup>*</sup>
                    <Form.Control type="text" name="code" value={speciality.code ?? ""}
                        onChange={onChangeModel}
                        isInvalid={!!errors.code} />
                    <Form.Control.Feedback type="invalid">{errors.code}</Form.Control.Feedback>
                </Form.Group>
                <Form.Group as={Col} sm={4}>
                    <Form.Label className="mb-0">Сокращённый код специальности</Form.Label>
                    <Form.Control type="text" name="shortCode" value={speciality.shortCode ?? ""}
                        onChange={onChangeModel}
                        isInvalid={!!errors.shortCode} />
                    <Form.Control.Feedback type="invalid">{errors.shortCode}</Form.Control.Feedback>
                </Form.Group>
            </Row>
            <Row className="mt-2">
                <Form.Group as={Col} sm={8}>
                    <Form.Label className="mb-0">Направление</Form.Label>
                    <Form.Control type="text" name="directionName" value={speciality.directionName ?? ""}
                        onChange={onChangeModel}
                        isInvalid={!!errors.directionName} />
                    <Form.Control.Feedback type="invalid">{errors.directionName}</Form.Control.Feedback>
                </Form.Group>
                <Form.Group as={Col} sm={4}>
                    <Form.Label className="mb-0">Код направления</Form.Label>
                    <Form.Control type="text" name="directionCode" value={speciality.directionCode ?? ""}
                        onChange={onChangeModel}
                        isInvalid={!!errors.directionCode} />
                    <Form.Control.Feedback type="invalid">{errors.directionCode}</Form.Control.Feedback>
                </Form.Group>
            </Row>
            <Row className="mt-2">
                <Form.Group as={Col} sm={8}>
                    <Form.Label className="mb-0">Специализация</Form.Label>
                    <Form.Control type="text" name="specializationName" value={speciality.specializationName ?? ""}
                        onChange={onChangeModel}
                        isInvalid={!!errors.specializationName} />
                    <Form.Control.Feedback type="invalid">{errors.specializationName}</Form.Control.Feedback>
                </Form.Group>
                <Form.Group as={Col} sm={4}>
                    <Form.Label className="mb-0">Код специализации</Form.Label>
                    <Form.Control type="text" name="specializationCode" value={speciality.specializationCode ?? ""}
                        onChange={onChangeModel}
                        isInvalid={!!errors.specializationCode} />
                    <Form.Control.Feedback type="invalid">{errors.specializationCode}</Form.Control.Feedback>
                </Form.Group>
            </Row>
            <Form.Group className="pt-2">
                <Form.Label className="mb-0">Описание</Form.Label>
                <Form.Control ref={textRef} name="description" type="text" as="textarea" value={speciality.description ?? ""}
                    onChange={onChangeModel}
                    isInvalid={errors ? !!errors.Description : false} />
                <Form.Control.Feedback type="invalid">{errors.description}</Form.Control.Feedback>
            </Form.Group>
        </React.Suspense >
    );
}