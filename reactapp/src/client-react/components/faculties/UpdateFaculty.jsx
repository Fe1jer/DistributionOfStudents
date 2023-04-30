import React from 'react';
import Form from 'react-bootstrap/Form';
import Image from 'react-bootstrap/Image'

import { Field } from 'formik';

export default function UpdateFaculty({ form, modelErrors, errors, onChangeModel }) {
    const [preview, setPreview] = React.useState(form.img);

    const showPreview = (e, form) => {
        const reader = new FileReader();
        reader.onload = x => {
            setPreview(x.target.result);
        }
        reader.readAsDataURL(e.target.files[0]);
        form.setFieldValue("fileImg", e.currentTarget.files[0]);
    };

    const _formGroupErrors = (errors) => {
        if (errors) {
            return errors.map((error) => <React.Suspense key={error}><span>{error}</span><br></br></React.Suspense>)
        }
    }
    return (
        <React.Suspense>
            <Form.Group style={{ textAlign: "-webkit-center" }}>
                <Form.Control className="p-0 d-none" plaintext readOnly isInvalid={modelErrors ? !!modelErrors : false} />
                <Form.Control.Feedback type="invalid">{modelErrors ? _formGroupErrors(modelErrors) : ""}</Form.Control.Feedback>
            </Form.Group>
            <Form.Group style={{ textAlign: "-webkit-center" }}>
                <Image className="scale card-image m-0 text-center" src={preview} style={{ objectFit: "cover", width: 290 }}></Image>
                <Field name="fileImg">
                    {({ form }) => (
                        <Form.Control name="fileImg" type="file" accept=".jpg, .jpeg, .png" style={{ width: 290 }}
                            onChange={e => showPreview(e, form)}
                            isInvalid={!!errors.fileImg} />
                    )}
                </Field>
                <Form.Control.Feedback className="m-0" type="invalid">{errors.img}</Form.Control.Feedback>
            </Form.Group>
            <Form.Group className="pt-3">
                <Form.Label className="mb-0">Полное название</Form.Label><sup>*</sup>
                <Form.Control type="text" name="fullName"
                    value={form.fullName ?? ""}
                    onChange={onChangeModel}
                    isInvalid={!!errors.fullName} />
                <Form.Control.Feedback type="invalid">{errors.fullName}</Form.Control.Feedback>
            </Form.Group>
            <Form.Group className="pt-3">
                <Form.Label className="mb-0">Сокращенное название</Form.Label><sup>*</sup>
                <Form.Control type="text" name="shortName"
                    value={form.shortName ?? ""}
                    onChange={onChangeModel}
                    isInvalid={!!errors.shortName} />
                <Form.Control.Feedback type="invalid">{errors.shortName}</Form.Control.Feedback>
            </Form.Group>
        </React.Suspense>
    );
}