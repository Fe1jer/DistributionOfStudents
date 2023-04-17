import Form from 'react-bootstrap/Form';
import Card from 'react-bootstrap/Card';

import React from 'react';

export default function SelectedGroupSubjects({ onChange, modelErrors, subjects }) {
    const [updateSelectedSubjects, setUpdatedSelectedSubjects] = React.useState(subjects);

    const onChangeSelectedSubjects = (e) => {
        var updateSelectedSubjectsTemp = updateSelectedSubjects;
        var item = updateSelectedSubjectsTemp.find(element => element.subject == e.target.id);
        var index = updateSelectedSubjectsTemp.indexOf(item);
        updateSelectedSubjectsTemp[index].isSelected = e.target.checked;
        setUpdatedSelectedSubjects(updateSelectedSubjectsTemp.slice());
    }
    React.useEffect(() => {
        onChange(updateSelectedSubjects);
    }, [updateSelectedSubjects]);

    const _formGroupErrors = (errors) => {
        if (errors) {
            return (<React.Suspense>{
                errors.map((error) =>
                    <React.Suspense key={error}><span>{error}</span><br></br></React.Suspense>
                )}
            </React.Suspense>);

        }
    }

    return (
        <Card className="px-2 mt-3">
            <h5 className="text-center">Предметы, по которым нужны сертификаты<sup>*</sup></h5>
            <Form.Group style={{ textAlign: "-webkit-center" }}>
                <Form.Control className="d-none" plaintext readOnly isInvalid={modelErrors} />
                <Form.Control.Feedback className="mt-0" type="invalid">{modelErrors ? _formGroupErrors(modelErrors) : ""}</Form.Control.Feedback>
            </Form.Group>
            <hr className="mt-2" />{
                updateSelectedSubjects.map((item) =>
                    <Form.Group key={item.subject} className="mb-1">
                        <Form.Check
                            type="checkbox"
                            id={item.subject}
                            label={item.subject}
                            checked={item.isSelected}
                            onChange={onChangeSelectedSubjects}
                            isInvalid={modelErrors}
                        />
                    </Form.Group>
                )}
        </Card>
    );
}