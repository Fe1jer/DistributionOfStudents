import { fetchWrapper } from "../../_helpers/fetch-wrapper";

const config = {
    api: '/api/RecruitmentPlansApi/',
};

const httpGet = () => {
    return fetchWrapper.get(`${config.api}`)
};
const httpGetById = (id) => {
    return fetchWrapper.get(`${config.api}${id}`)
};
const httpGetFacultyPlans = (facultyShortName, year) => {
    return fetchWrapper.get(`${config.api}${facultyShortName}/${year}`)
};
const httpGetGroupPlans = (facultyShortName, groupId) => {
    return fetchWrapper.get(`${config.api}${facultyShortName}/${groupId}/GroupPlans`)
};
const httpGetLastFacultyPlans = (facultyShortName) => {
    return fetchWrapper.get(`${config.api}${facultyShortName}/lastYear`)
};
const httpPost = (facultyShortName, year, data) => {
    return fetchWrapper.post(`${config.api}${facultyShortName}/${year}`, JSON.stringify(data))
};
const httpPut = (facultyShortName, year, data) => {
    return fetchWrapper.put(`${config.api}${facultyShortName}/${year}`, JSON.stringify(data))
};
const httpPutById = (id, data) => {
    return fetchWrapper.put(`${config.api}${id}`, JSON.stringify(data))
};
const httpDelete = (facultyShortName, year) => {
    return fetchWrapper.delete(`${config.api}${facultyShortName}/${year}`)
};

const exportedObject = {
    httpGet, httpGetById, httpGetFacultyPlans, httpGetGroupPlans, httpGetLastFacultyPlans, httpPost, httpPut, httpPutById, httpDelete
};

export default exportedObject;