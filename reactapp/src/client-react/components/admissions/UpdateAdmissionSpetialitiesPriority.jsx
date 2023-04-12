import Form from 'react-bootstrap/Form';
import Sortable from "../Sortable.jsx";
import $ from 'jquery';

import React from 'react';

export default function UpdateAdmissionSpetialitiesPriority({ onChangeModel, specialitiesPriority, modelErrors }) {
    const [specialitiesInPriority, setSpecialitiesInPriority] = React.useState(specialitiesPriority.filter(item => item.priority > 0).sort((a, b) => a.priority - b.priority).map(item => { return item.nameSpeciality }));
    const [specialitiesIsntInPriority, setSpecialitiesIsntInPriority] = React.useState(specialitiesPriority.filter(item => item.priority === 0).map(item => { return item.nameSpeciality }));
    const [updatedSpecialitiesPriority, setUpdatedSpecialitiesPriority] = React.useState(specialitiesPriority);

    const onChangeSpecialitiesPriority = (nameSpeciality, priority) => {
        var updateSpecialitiesPriorityTemp = updatedSpecialitiesPriority;
        var item = updateSpecialitiesPriorityTemp.find(element => element.nameSpeciality == nameSpeciality);
        var index = updateSpecialitiesPriorityTemp.indexOf(item);
        updateSpecialitiesPriorityTemp[index].priority = priority;
        setUpdatedSpecialitiesPriority(updateSpecialitiesPriorityTemp);
    }

    const change = (event, ui) => {
        $(".not .one").each(function () {
            const nameSpeciality = $(this).find(".item").text();
            onChangeSpecialitiesPriority(nameSpeciality, 0);
            $(this).find("strong").removeClass("alert-success");
            $(this).find("strong").addClass("alert-danger");
            $(this).find(".index").text(0);
        });
        $(".priority .one").each(function (index) {
            const nameSpeciality = $(this).find(".item").text();
            onChangeSpecialitiesPriority(nameSpeciality, index + 1);
            $(this).find(".index").text(index + 1);
            $(this).find("strong").removeClass("alert-danger");
            $(this).find("strong").addClass("alert-success");
        });
        onChangeModel(updatedSpecialitiesPriority.slice());
    }

    React.useEffect(() => {
        change();
    }, []);

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
        <React.Suspense>
            <h5 className="text-center">Приоритет специальностей<sup>*</sup></h5>
            <Form.Group style={{ textAlign: "-webkit-center" }}>
                <Form.Control className="d-none" plaintext readOnly isInvalid={modelErrors} />
                <Form.Control.Feedback className="mt-0" type="invalid">{modelErrors ? _formGroupErrors(modelErrors) : ""}</Form.Control.Feedback>
            </Form.Group>
            <div className="priority">
                <Sortable onChange={change} data={specialitiesInPriority} />
            </div>
            <h5 className="text-center">Специальности, не входящие в приоритет</h5>
            <div className="not">
                <Sortable onChange={change} data={specialitiesIsntInPriority} />
            </div>
        </React.Suspense>
    );
}