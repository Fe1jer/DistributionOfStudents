import RecruitmentPlansService from "../../services/RecruitmentPlans.service.js";
import DistributionService from '../../services/Distribution.service.js';

import DeleteButton from '../adminButtons/DeleteButton';
import EditButton from '../adminButtons/EditButton';
import Placeholder from 'react-bootstrap/Placeholder';

import { Link } from 'react-router-dom'
import React, { useState, useEffect } from "react";

import { getTodayTimeNull } from "../../../../src/js/datePicker.js"

export default function GroupOfSpecialities({ group, facultyShortName, onClickDelete, onClickEdit }) {
    const [plans, setPlans] = useState(null);
    const [competition, setCompetition] = useState(null);

    const loadSpecialities = async () => {
        const data = await RecruitmentPlansService.httpGetGroupRecruitmentPlans(facultyShortName, group.id);
        setPlans(data);
    }
    const loadCompetition = async () => {
        const data = await DistributionService.httpGetGroupCompetition(facultyShortName, group.id);
        setCompetition(data);
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
    const _showButtons = () => {
        return <td className="text-center">
            <div className="d-inline-flex">
                <EditButton onClick={() => onClickEdit(group.id)} />
                <DeleteButton onClick={() => onClickDelete(group.id)} />
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
        <td>{competition ?? <Placeholder className="w-100" />}</td>
        <td>{_showPlans()}</td>
        {!group.isCompleted ? _showButtons() : null}
    </tr>;
}