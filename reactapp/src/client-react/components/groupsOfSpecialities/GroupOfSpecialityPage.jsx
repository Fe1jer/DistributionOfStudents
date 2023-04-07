import ModalWindowEdit from "./ModalWindows/ModalWindowEdit.jsx";
import DistribitedRecruitmentPlansList from "../recruitmentPlans/DistribitedRecruitmentPlansList.jsx";

import GroupsOfSpecialitiesApi from '../../api/GroupsOfSpecialitiesApi.js';
import RecruitmentPlansApi from '../../api/RecruitmentPlansApi.js';
import StatisticApi from '../../api/StatisticApi.js';

import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import Card from 'react-bootstrap/Card';
import Button from 'react-bootstrap/Button';

import { Chart as ChartJS, CategoryScale, LinearScale, PointElement, LineElement, Title, Tooltip, Legend, } from 'chart.js';
import { Line } from 'react-chartjs-2';
import getData from '../../../js/showStatistic';

import { getTodayTimeNull } from "../../../../src/js/datePicker.js"
import { Link, useParams, useNavigate } from 'react-router-dom'
import React, { useState } from 'react';
ChartJS.register(CategoryScale, LinearScale, PointElement, LineElement, Title, Tooltip, Legend);

export default function GroupOfSpecialityPage() {
    const params = useParams();
    const navigate = useNavigate();
    const facultyShortName = params.shortName;
    const groupId = params.groupId;

    const [loading, setLoading] = useState(true);
    const [group, setGroup] = useState(null);
    const [plans, setPlans] = useState([]);
    const [plansStatistic, setPlansStatistic] = useState(null);
    const [groupStatistic, setGroupStatistic] = useState(null);
    const [canDistribution, setCanDistribution] = useState('disabled');

    const loadGroup = () => {
        var xhr = new XMLHttpRequest();
        xhr.open("get", GroupsOfSpecialitiesApi.getGroupUrl(groupId), true);
        xhr.onload = function () {
            var data = JSON.parse(xhr.responseText);
            setGroup(data);
            setCanDistribution(new Date(getTodayTimeNull()) < new Date(data.enrollmentDate) ? 'disabled' : '');
            setLoading(false);
        }.bind(this);
        xhr.send();
    }
    const loadPlans = () => {
        var xhr = new XMLHttpRequest();
        xhr.open("get", RecruitmentPlansApi.getGroupRecruitmentPlansUrl(facultyShortName, groupId), true);
        xhr.onload = function () {
            var data = JSON.parse(xhr.responseText);
            setPlans(data);
        }.bind(this);
        xhr.send();
    }
    const loadPlansStatistic = () => {
        var xhr = new XMLHttpRequest();
        xhr.open("get", StatisticApi.getPlansStatisticUrl(facultyShortName, groupId), true);
        xhr.onload = function () {
            var data = JSON.parse(xhr.responseText);
            setPlansStatistic(data);
        }.bind(this);
        xhr.send();
    }
    const loadGroupStatistic = () => {
        var xhr = new XMLHttpRequest();
        xhr.open("get", StatisticApi.getGroupStatisticUrl(groupId), true);
        xhr.onload = function () {
            var data = JSON.parse(xhr.responseText);
            setGroupStatistic(data);
        }.bind(this);
        xhr.send();
    }
    const loadData = () => {
        loadGroup();
        loadPlans();
        loadPlansStatistic();
        loadGroupStatistic();
    }

    React.useEffect(() => {
        loadData();
    }, [])

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
        if (!group.isCompleted) {
            return <React.Suspense>
                <h4 className="d-flex">
                    Заявки абитуриентов
                    <Link type="button" to="#" className={'btn btn-sm btn-outline-success ' + canDistribution + ' ms-2'}>
                        Распределить
                    </Link>
                </h4>
                {_showStatistics()}
            </React.Suspense>;
        }
        else {
            return <React.Suspense>
                <h4 className="d-flex">
                    Зачисленные студенты
                    <Button variant="outline-danger" size="sm" className="ms-2">
                        Перераспределить
                    </Button>
                </h4>
            </React.Suspense>;
        }
    }

    if (loading) {

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
                    <DistribitedRecruitmentPlansList plans={plans} />
                    <hr className="mt-4" />
                    {_showContent()}
                </div>
            </React.Suspense>
        );
    }
}