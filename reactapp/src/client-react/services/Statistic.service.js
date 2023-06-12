import { fetchWrapper } from "../../_helpers/fetch-wrapper";

const config = {
    api: '/api/StatisticApi/',
};

const httpGetPlansStatistic = (facultyName, groupId) => {
    return fetchWrapper.get(`${config.api}PlansStatisticChart?groupId=${groupId}&facultyName=${facultyName}`)
};
const httpGetGroupStatistic = (groupId) => {
    return fetchWrapper.get(`${config.api}GroupStatisticChart?groupId=${groupId}`)
};
const httpPutGroupStatisticUrl = (facultyName, groupId) => {
    return fetchWrapper.put(`${config.api}${facultyName}/${groupId}`)
};

const exportedObject = {
    httpGetPlansStatistic, httpGetGroupStatistic, httpPutGroupStatisticUrl
};

export default exportedObject;