import { fetchWrapper } from "../../_helpers/fetch-wrapper";

const config = {
    api: '/api/DistributionApi/',
};

const httpGet = (facultyName, groupId) => {
    return fetchWrapper.get(`${config.api}${facultyName}/${groupId}`)
};

const httpGetGroupCompetition = (facultyName, groupId) => {
    return fetchWrapper.get(`${config.api}${facultyName}/${groupId}/Competition`)
};

const httpPost = (facultyName, groupId, data) => {
    return fetchWrapper.post(`${config.api}${facultyName}/${groupId}/CreateDistribution`, JSON.stringify(data))
};

const httpPostConfirm = (facultyName, groupId, distributedPlans, notify) => {
    return fetchWrapper.post(`${config.api}${facultyName}/${groupId}/ConfirmDistribution?notify=${notify}`, JSON.stringify(distributedPlans))
};

const httpDelete = (facultyName, groupId) => {
    return fetchWrapper.delete(`${config.api}${facultyName}/${groupId}`)
};

const exportedObject = {
    httpGet, httpGetGroupCompetition, httpPost, httpPostConfirm, httpDelete
};

export default exportedObject;