import Form from 'react-bootstrap/Form';
import Sortable from "../Sortable.jsx";

import $ from 'jquery';

import React, { useState } from 'react';

export default function UpdateAdmissionSpetialitiesPriority({ form, specialitiesPriority, errors }) {
    const [updatedSpecialitiesPriority, setUpdatedSpecialitiesPriority] = useState(specialitiesPriority);
    const [specialitiesInPriority] = useState(updatedSpecialitiesPriority.filter(item => item.priority > 0).sort((a, b) => a.priority - b.priority).map(item => { return item.nameSpeciality }));
    const [specialitiesIsntInPriority] = useState(updatedSpecialitiesPriority.filter(item => item.priority === 0).map(item => { return item.nameSpeciality }));

    const onChangeSpecialitiesPriority = (nameSpeciality, priority) => {
        var updateSpecialitiesPriorityTemp = updatedSpecialitiesPriority;
        var item = updateSpecialitiesPriorityTemp.find(element => element.nameSpeciality === nameSpeciality);
        var index = updateSpecialitiesPriorityTemp.indexOf(item);
        updateSpecialitiesPriorityTemp[index].priority = priority;
        setUpdatedSpecialitiesPriority(updateSpecialitiesPriorityTemp);
    }

    const sortableModel = () => {
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
    }

    const onChangeModel = (event, ui) => {
        sortableModel();
        form.setFieldValue("specialitiesPriority", updatedSpecialitiesPriority);
    }

    React.useEffect(() => {
        sortableModel();
    }, []);

    return (
        <React.Suspense>
            <h5 className="text-center">Приоритет специальностей<sup>*</sup></h5>
            <Form.Group style={{ textAlign: "-webkit-center" }}>
                <Form.Control className="d-none" plaintext readOnly isInvalid={errors} />
                <Form.Control.Feedback className="mt-0" type="invalid">{errors}</Form.Control.Feedback>
            </Form.Group>
            <div className="priority">
                <Sortable onChange={onChangeModel} data={specialitiesInPriority} />
            </div>
            <h5 className="text-center">Специальности, не входящие в приоритет</h5>
            <div className="not">
                <Sortable onChange={onChangeModel} data={specialitiesIsntInPriority} />
            </div>
        </React.Suspense>
    );
}