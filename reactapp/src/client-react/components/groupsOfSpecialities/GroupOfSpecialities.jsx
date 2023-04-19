import RecruitmentPlansService from "../../services/RecruitmentPlans.service.js";
import DistributionApi from "../../api/DistributionApi.js";

import Button from 'react-bootstrap/Button';
import Placeholder from 'react-bootstrap/Placeholder';

import { Link } from 'react-router-dom'
import React, { useState, useEffect } from "react";

import { getTodayTimeNull } from "../../../../src/js/datePicker.js"

export default function GroupOfSpecialities({ group, facultyShortName, onClickDelete, onClickEdit }) {
    const [plans, setPlans] = useState(null);
    const [competition, setCompetition] = useState([]);

    const loadSpecialities = async () => {
        const data = await RecruitmentPlansService.httpGetGroupRecruitmentPlans(facultyShortName, group.id);
        setPlans(data);
    }
    const loadCompetition = () => {
        var xhr = new XMLHttpRequest();
        xhr.open("get", DistributionApi.getGroupCompetitionUrl(facultyShortName, group.id), true);
        xhr.onload = function () {
            var data = JSON.parse(xhr.responseText);
            setCompetition(data);
        }.bind(this);
        xhr.send();
    }

    useEffect(() => {
        loadSpecialities();
        loadCompetition();
    }, [group])

    const _showPlans = () => {
        if (plans) {
            return plans.map((item) =>
                <React.Suspense key={group.name + "-" + (item.speciality.directionName ?? item.speciality.fullName)} >
                    {item.speciality.directionName ?? item.speciality.fullName}
                    <p className="m-1"></p>
                </React.Suspense>
            )
        }
        else {
            return [1, 2, 3].map((item) =>
                <Placeholder key={group.name + "-" + item} className="m-1" as="p" animation="glow">
                    <Placeholder className="w-100" />
                </Placeholder>
            )
        }
    }

    var buttons = null;
    if (!group.isCompleted) {
        buttons = <td className="text-center">
            <div className="d-inline-flex">
                <Button variant="outline-success" className="py-1" onClick={() => onClickEdit(group.id)}>
                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" className="bi bi-pencil-square" viewBox="0 0 16 16">
                        <path d="M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z" />
                        <path fillRule="evenodd" d="M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5v11z" />
                    </svg>
                </Button>
                <Button variant="outline-danger" className="py-1" onClick={() => onClickDelete(group.id)}>
                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" className="bi bi-trash3-fill" viewBox="0 0 16 16">
                        <path d="M11 1.5v1h3.5a.5.5 0 0 1 0 1h-.538l-.853 10.66A2 2 0 0 1 11.115 16h-6.23a2 2 0 0 1-1.994-1.84L2.038 3.5H1.5a.5.5 0 0 1 0-1H5v-1A1.5 1.5 0 0 1 6.5 0h3A1.5 1.5 0 0 1 11 1.5Zm-5 0v1h4v-1a.5.5 0 0 0-.5-.5h-3a.5.5 0 0 0-.5.5ZM4.5 5.029l.5 8.5a.5.5 0 1 0 .998-.06l-.5-8.5a.5.5 0 1 0-.998.06Zm6.53-.528a.5.5 0 0 0-.528.47l-.5 8.5a.5.5 0 0 0 .998.058l.5-8.5a.5.5 0 0 0-.47-.528ZM8 4.5a.5.5 0 0 0-.5.5v8.5a.5.5 0 0 0 1 0V5a.5.5 0 0 0-.5-.5Z" />
                    </svg>
                </Button>
            </div>
        </td>;
    }


    return <tr className="align-middle">
        <td>
            <Link className="nav-link text-success p-0" to={"/Faculties/" + facultyShortName + "/" + group.id}>
                {group.name}
            </Link>
        </td>
        <td className={!group.isCompleted && new Date(group.enrollmentDate) < new Date(getTodayTimeNull()) ? "text-danger" : ""}>
            {new Date(group.startDate).toLocaleDateString("ru-ru")}
            <p className="m-1"></p>
            {new Date(group.enrollmentDate).toLocaleDateString("ru-ru")}
        </td>
        <td>{competition}</td>
        <td>{_showPlans()}</td>
        {buttons}
    </tr>;
}