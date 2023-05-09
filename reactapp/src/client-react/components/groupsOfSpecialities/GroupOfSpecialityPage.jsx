import DistributedRecruitmentPlansList from "../distribution/DistributedRecruitmentPlansList.jsx";
import DistributedPlan from "../distribution/DistributedPlan.jsx";
import Admissions from "../admissions/Admissions.jsx";

import ModalWindowDeleteDitribution from "../distribution/ModalWindows/ModalWindowDelete.jsx";

import GroupsOfSpecialitiesService from '../../services/GroupsOfSpecialities.service.js';
import RecruitmentPlansService from "../../services/RecruitmentPlans.service.js";
import StatisticService from "../../services/Statistic.service.js";

import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import Card from 'react-bootstrap/Card';
import Button from 'react-bootstrap/Button';
import Placeholder from 'react-bootstrap/Placeholder';

import TablePreloader from "../TablePreloader.jsx";

import { Chart as ChartJS, CategoryScale, LinearScale, PointElement, LineElement, Title, Tooltip, Legend, } from 'chart.js';
import { Line } from 'react-chartjs-2';
import getData from '../../../js/showStatistic';

import { getTodayTimeNull } from "../../../../src/js/datePicker.js"
import { Link, useParams } from 'react-router-dom'
import React, { useState } from 'react';
import useDocumentTitle from "../useDocumentTitle.jsx";
ChartJS.register(CategoryScale, LinearScale, PointElement, LineElement, Title, Tooltip, Legend);

export default function GroupOfSpecialityPage() {
    const [groupName, setGroupName] = useState("Группа специальностей");
    useDocumentTitle(groupName);
    const params = useParams();
    const facultyShortName = params.shortName;
    const groupId = params.groupId;

    const [loading, setLoading] = useState(true);
    const [loadedGroupStatistic, setLoadedGroupStatistic] = useState(false);
    const [loadedPlansStatistic, setLoadedPlansStatistic] = useState(false);
    const [group, setGroup] = useState(null);
    const [plans, setPlans] = useState(null);
    const [plansStatistic, setPlansStatistic] = useState(null);
    const [groupStatistic, setGroupStatistic] = useState(null);
    const [deleteDistributionShow, setDeleteDistributionShow] = useState(false);
    const [canDistribution, setCanDistribution] = useState('disabled');

    const loadGroup = async () => {
        const groupData = await GroupsOfSpecialitiesService.httpGetById(groupId);
        setLoading(false);
        setGroupName(groupData.name)
        setGroup(groupData);
        setCanDistribution(new Date(getTodayTimeNull()) < new Date(groupData.enrollmentDate) ? 'disabled' : '');
    }
    const loadPlans = async () => {
        const recruitmentsPlansData = await RecruitmentPlansService.httpGetGroupRecruitmentPlans(facultyShortName, groupId);
        setPlans(recruitmentsPlansData);
    }
    const loadPlansStatistic = async () => {
        const data = await StatisticService.httpGetPlansStatistic(facultyShortName, groupId);
        setPlansStatistic(data);
    }
    const loadGroupStatistic = async () => {
        const data = await StatisticService.httpGetGroupStatistic(groupId);
        setGroupStatistic(data);
    }
    const loadData = () => {
        setLoadedPlansStatistic(false);
        setLoadedGroupStatistic(false);
        loadGroup();
        loadPlans();
    }

    const handleDeleteDistributionClose = () => {
        setDeleteDistributionShow(false);
    };
    const onClickDeleteDistribution = () => {
        setDeleteDistributionShow(true);
    }

    React.useEffect(() => {
        if (loading) {
            loadData();
        }
        if (group && !loadedGroupStatistic) {
            setLoadedGroupStatistic(true);
            loadGroupStatistic();
        }
        if (plans && !loadedPlansStatistic) {
            setLoadedPlansStatistic(true);
            loadPlansStatistic();
        }
    }, [group, plans])

    const _showPlansStatistic = () => {
        if (plansStatistic) {
            return <Line data={getData(plansStatistic)} />;
        }
    }
    const _showGroupStatistic = () => {
        if (groupStatistic) {
            return <Line data={getData(groupStatistic)} />;
        }
    }
    const _showIsCompleted = () => {
        if (!group.isCompleted && new Date(group.enrollmentDate) < new Date(getTodayTimeNull())) {
            return <b className="ps-1 text-danger">Завершено</b>;
        }
    }
    const _showStatistics = () => {
        return <Row>
            <Col xl="6">
                <hr className="mt-4" />
                <h4>График проходных баллов</h4>
                <Card className="shadow">{_showPlansStatistic()}</Card>
            </Col>
            <Col xl="6">
                <hr className="mt-4" />
                <h4>График количества заявок по группе</h4>
                <Card className="shadow">{_showGroupStatistic()}</Card>
            </Col>
        </Row>;
    }
    const _showContent = () => {
        if (!group.isCompleted && plans.length > 0) {
            return <React.Suspense>
                <h4 className="d-flex">
                    Заявки абитуриентов
                    <Link type="button" to="./Distribution/Create" className={'btn btn-sm btn-outline-success ' + canDistribution + ' ms-2'}>
                        Распределить
                    </Link>
                </h4>
                <Admissions groupId={groupId} plans={plans} onLoadGroup={loadData} />
                {_showStatistics()}
            </React.Suspense>;
        }
        else if (group.isCompleted) {
            return <React.Suspense>
                <h4 className="d-flex">
                    Зачисленные студенты
                    <ModalWindowDeleteDitribution show={deleteDistributionShow} handleClose={handleDeleteDistributionClose} onLoadGroup={loadData} groupId={groupId} facultyShortName={facultyShortName} />
                    <Button variant="outline-danger" size="sm" className="ms-2" onClick={() => onClickDeleteDistribution()}>
                        Расформировать
                    </Button>
                </h4>{
                    plans.map((plan, index) =>
                        <React.Suspense key={plan.speciality.directionName ?? plan.speciality.fullName}>
                            <DistributedPlan plan={plan} />
                            {index !== plans.length - 1 ? <hr /> : null}
                        </React.Suspense>
                    )}
            </React.Suspense>;
        }
    }

    if (!group || !plans) {
        return <React.Suspense>
            <Placeholder as="h1" animation="glow" className="text-center"><Placeholder xs={12} /></Placeholder>
            <hr />
            <div className="ps-lg-4 pe-lg-4 position-relative">
                <Placeholder as="p" animation="glow" className="text-center"><Placeholder xs={4} /></Placeholder>
                <TablePreloader />
                <hr />
            </div>
        </React.Suspense>
    }
    else {
        return (
            <React.Suspense>
                <h1 className="text-center">{group.name}</h1>
                <hr />
                <div className="ps-lg-4 pe-lg-4 position-relative">
                    <p className="text-center">
                        <b>Сроки приёма документов: {new Date(group.startDate).toLocaleDateString("ru-ru")} - {new Date(group.enrollmentDate).toLocaleDateString("ru-ru")}</b>
                        {_showIsCompleted()}
                    </p>
                    <DistributedRecruitmentPlansList plans={plans} reloadPlans={loadPlans} />
                    <hr />
                    {_showContent()}
                </div>
            </React.Suspense>
        );
    }
}