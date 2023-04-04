import React from 'react';
import Form from 'react-bootstrap/Form';

export default function UpdateSubject({ errors, subject, onChangeModel }) {
    const [updatedSubject, setUpdatedSubject] = React.useState({
        id: subject.id,
        name: subject.name
    });
    const { id, name } = updatedSubject;

    const handleChange = (event) => {
        const { name, value } = event.target;
        setUpdatedSubject((prevState) => {
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
        onChangeModel(updatedSubject);
    }, [updatedSubject]);

    return (
        <Form.Group>
            <Form.Label className="mb-0">Название</Form.Label><sup>*</sup>
            <Form.Control type="text"
                required name="name" value={name ?? ""} onChange={handleChange}
                isInvalid={errors ? !!errors.Name : false} />
            <Form.Control.Feedback type="invalid">{errors ? _formGroupErrors(errors.Name) : ""}</Form.Control.Feedback>
        </Form.Group>
    );
}