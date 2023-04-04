import React from 'react';
import Form from 'react-bootstrap/Form';
import Col from 'react-bootstrap/Col';
import Row from 'react-bootstrap/Row';

export default function UpdateSpeciality({ errors, speciality, onChangeModel }) {
    const textRef = React.useRef();

    const [updatedSpeciality, setUpdatedSpeciality] = React.useState({
        id: speciality.id,
        fullName: speciality.fullName,
        shortName: speciality.shortName,
        code: speciality.code,
        shortCode: speciality.shortCode,
        directionName: speciality.directionName,
        directionCode: speciality.directionCode,
        specializationName: speciality.specializationName,
        specializationCode: speciality.specializationCode,
        description: speciality.description
    });
    const { id, fullName, shortName, code, shortCode, directionName, directionCode, specializationName, specializationCode, description } = updatedSpeciality;

    const handleChange = (event) => {
        const { name, value } = event.target;
        setUpdatedSpeciality((prevState) => {
            return {
                ...prevState,
                [name]: value,
            };
        });
    }

    const _formGroupErrors = (errors) => {
        if (errors) {
            return (<React.Suspense>{
                errors.map((error) =>
                    <React.Suspense key={error}><span>{error}</span><br></br></React.Suspense>
                )}
            </React.Suspense>);
        }
    }

    React.useEffect(() => {
        if (textRef && textRef.current) {
            textRef.current.style.height = "0px";
            const taHeight = textRef.current.scrollHeight;
            textRef.current.style.height = taHeight + "px";
        }
        onChangeModel(updatedSpeciality);
    }, [updatedSpeciality]);
    console.log(errors ? !!errors.Code : false);
    return (
        <React.Suspense>
            <Form.Group>
                <Form.Label className="mb-0">Специальность</Form.Label><sup>*</sup>
                <Form.Control type="text"
                    required name="fullName" value={fullName ?? ""} onChange={handleChange}
                    isInvalid={errors ? !!errors.FullName : false} />
                <Form.Control.Feedback type="invalid">{errors ? _formGroupErrors(errors.FullName) : ""}</Form.Control.Feedback>
            </Form.Group>
            <Row className="mt-2">
                <Form.Group as={Col} sm={4}>
                    <Form.Label className="mb-0">Аббревиатура</Form.Label>
                    <Form.Control type="text"
                        name="shortName" value={shortName ?? ""} onChange={handleChange}
                        isInvalid={errors ? !!errors.ShortName : false} />
                    <Form.Control.Feedback type="invalid">{errors ? _formGroupErrors(errors.ShortName) : ""}</Form.Control.Feedback>
                </Form.Group>
                <Form.Group as={Col} sm={4}>
                    <Form.Label className="mb-0">Код специальности</Form.Label><sup>*</sup>
                    <Form.Control type="text"
                        required name="code" value={code ?? ""} onChange={handleChange}
                        isInvalid={errors ? !!errors.Code : false} />
                    <Form.Control.Feedback type="invalid">{errors ? _formGroupErrors(errors.Code) : ""}</Form.Control.Feedback>
                </Form.Group>
                <Form.Group as={Col} sm={4}>
                    <Form.Label className="mb-0">Сокращённый код специальности</Form.Label>
                    <Form.Control type="text"
                        name="shortCode" value={shortCode ?? ""} onChange={handleChange}
                        isInvalid={errors ? !!errors.ShortCode : false} />
                    <Form.Control.Feedback type="invalid">{errors ? _formGroupErrors(errors.ShortCode) : ""}</Form.Control.Feedback>
                </Form.Group>
            </Row>
            <Row className="mt-2">
                <Form.Group as={Col} sm={8}>
                    <Form.Label className="mb-0">Направление</Form.Label>
                    <Form.Control type="text"
                        name="directionName" value={directionName ?? ""} onChange={handleChange}
                        isInvalid={errors ? !!errors.DirectionName : false} />
                    <Form.Control.Feedback type="invalid">{errors ? _formGroupErrors(errors.DirectionName) : ""}</Form.Control.Feedback>
                </Form.Group>
                <Form.Group as={Col} sm={4}>
                    <Form.Label className="mb-0">Код направления</Form.Label>
                    <Form.Control type="text"
                        name="directionCode" value={directionCode ?? ""} onChange={handleChange}
                        isInvalid={errors ? !!errors.DirectionCode : false} />
                    <Form.Control.Feedback type="invalid">{errors ? _formGroupErrors(errors.DirectionCode) : ""}</Form.Control.Feedback>
                </Form.Group>
            </Row>
            <Row className="mt-2">
                <Form.Group as={Col} sm={8}>
                    <Form.Label className="mb-0">Специализация</Form.Label>
                    <Form.Control type="text"
                        name="specializationName" value={specializationName ?? ""} onChange={handleChange}
                        isInvalid={errors ? !!errors.SpecializationName : false} />
                    <Form.Control.Feedback type="invalid">{errors ? _formGroupErrors(errors.SpecializationName) : ""}</Form.Control.Feedback>
                </Form.Group>
                <Form.Group as={Col} sm={4}>
                    <Form.Label className="mb-0">Код специализации</Form.Label>
                    <Form.Control type="text"
                        name="specializationCode" value={specializationCode ?? ""} onChange={handleChange}
                        isInvalid={errors ? !!errors.SpecializationCode : false} />
                    <Form.Control.Feedback type="invalid">{errors ? _formGroupErrors(errors.SpecializationCode) : ""}</Form.Control.Feedback>
                </Form.Group>
            </Row>
            <Form.Group className="pt-2">
                <Form.Label className="mb-0">Код специальности</Form.Label>
                <Form.Control ref={textRef} type="text"
                    name="description" as="textarea" value={description ?? ""} onChange={handleChange}
                    isInvalid={errors ? !!errors.Description : false} />
                <Form.Control.Feedback type="invalid">{errors ? _formGroupErrors(errors.Description) : ""}</Form.Control.Feedback>
            </Form.Group>
        </React.Suspense >
    );
}