import Form from 'react-bootstrap/Form';
import Sortable from "../Sortable.jsx";

import $ from 'jquery';

import React, { useState } from 'react';

export default function UpdateAdmissionSpetialitiesPriority({ form, specialityPriorities, errors }) {
    const [updatedSpecialityPriorities, setUpdatedSpecialityPriorities] = useState(specialityPriorities);
    const [specialitiesInPriority] = useState(updatedSpecialityPriorities.filter(item => item.priority > 0).sort((a, b) => a.priority - b.priority).map(item => { return item.specialityName }));
    const [specialitiesIsntInPriority] = useState(updatedSpecialityPriorities.filter(item => item.priority === 0).map(item => { return item.specialityName }));

    const onChangeSpecialityPriorities = (specialityName, priority) => {
        var updateSpecialityPrioritiesTemp = updatedSpecialityPriorities;
        var item = updateSpecialityPrioritiesTemp.find(element => element.specialityName === specialityName);
        var index = updateSpecialityPrioritiesTemp.indexOf(item);
        updateSpecialityPrioritiesTemp[index].priority = priority;
        setUpdatedSpecialityPriorities(updateSpecialityPrioritiesTemp);
    }

    const sortableModel = () => {
        $(".not .one").each(function () {
            const specialityName = $(this).find(".item").text();
            onChangeSpecialityPriorities(specialityName, 0);
            $(this).find("strong").removeClass("alert-success");
            $(this).find("strong").addClass("alert-danger");
            $(this).find(".index").text(0);
        });
        $(".priority .one").each(function (index) {
            const specialityName = $(this).find(".item").text();
            onChangeSpecialityPriorities(specialityName, index + 1);
            $(this).find(".index").text(index + 1);
            $(this).find("strong").removeClass("alert-danger");
            $(this).find("strong").addClass("alert-success");
        });
    }

    const onChangeModel = (event, ui) => {
        sortableModel();
        form.setFieldValue("specialityPriorities", updatedSpecialityPriorities);
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