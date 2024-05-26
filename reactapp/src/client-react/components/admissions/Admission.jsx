import AdmissionPriority from "./AdmissionPriority.jsx";

import DeleteButton from '../adminButtons/DeleteButton';
import EditButton from '../adminButtons/EditButton';

import Button from 'react-bootstrap/Button';

import * as React from "react";

export default function Admission({ admission, plans, onClickDelete, onClickEdit, onClickDetails }) {
    return <tr className="align-middle">
        <td>
            <Button variant="empty" className="nav-link text-success p-0" onClick={() => onClickDetails(admission.id)}>
                {admission.student.fullName}
            </Button>
        </td>
        <td>{new Date(admission.dateOfApplication).toLocaleString("ru-ru")}</td>
        <td>{admission.score}</td>
        <td>{admission.specialityPriorities.sort((a, b) => a.priority - b.priority).map((item, index) =>
            <AdmissionPriority key={admission.id + item.priority} admission={admission} specialityPriority={item} plans={plans} index={index} />)}
        </td>
        <td className="text-center">
            <div className="d-inline-flex">
                <EditButton size="sm" onClick={() => onClickEdit(admission.id)} />
                <DeleteButton size="sm" onClick={() => onClickDelete(admission.id)} />
            </div>
        </td>
    </tr>;
}