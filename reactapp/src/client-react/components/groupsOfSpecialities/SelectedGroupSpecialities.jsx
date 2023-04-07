import Form from 'react-bootstrap/Form';
import Card from 'react-bootstrap/Card';

import React from 'react';

export default function SelectedGroupSpecialities({ onChange, modelErrors, specialities }) {
    const [updateSelectedSpecialities, setUpdatedSelectedSpecialities] = React.useState(specialities);

    const onChangeSelectedSpecialities = (e) => {
        var updateSelectedSpecialitiesTemp = updateSelectedSpecialities;
        var item = updateSelectedSpecialitiesTemp.find(element => element.specialityName == e.target.id);
        var index = updateSelectedSpecialitiesTemp.indexOf(item);
        updateSelectedSpecialitiesTemp[index].isSelected = e.target.checked;
        setUpdatedSelectedSpecialities(updateSelectedSpecialitiesTemp.slice());
    }
    React.useEffect(() => {
        onChange(updateSelectedSpecialities);
    }, [updateSelectedSpecialities]);

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
            <h5 className="text-center">Специальности, составляющие общий конкурс<sup>*</sup></h5>
            <Form.Group style={{ textAlign: "-webkit-center" }}>
                <Form.Control className="d-none" plaintext readOnly isInvalid={modelErrors} />
                <Form.Control.Feedback className="mt-0" type="invalid">{modelErrors ? _formGroupErrors(modelErrors) : ""}</Form.Control.Feedback>
            </Form.Group>
            <hr className="mt-2" />{
                updateSelectedSpecialities.map((item) =>
                        <Form.Check key={item.specialityName} className="mb-1"
                            type="checkbox"
                            id={item.specialityName}
                            label={item.specialityName}
                            checked={item.isSelected}
                            onChange={onChangeSelectedSpecialities}
                            isInvalid={modelErrors}
                        />
                )}
        </Card>
    );
}