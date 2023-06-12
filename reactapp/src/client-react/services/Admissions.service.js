import { fetchWrapper } from "../../_helpers/fetch-wrapper";

const config = {
    api: '/api/AdmissionsApi/',
};

const httpGetGroupAdmissions = (groupId, searhText, currentPage, pageLimit) => {
    return fetchWrapper.get(`${config.api}GroupAdmissions/${groupId}?searchStudents=${searhText}&page=${currentPage}&pageLimit=${pageLimit}`)
};

const httpGetById = (id) => {
    return fetchWrapper.get(`${config.api}${id}`)
};

const httpPost = (groupId, data) => {
    return fetchWrapper.post(`${config.api}${groupId}`, JSON.stringify(data))
};

const httpPut = (id, data) => {
    return fetchWrapper.put(`${config.api}${id}`, JSON.stringify(data))
};

const httpDelete = (id) => {
    return fetchWrapper.delete(`${config.api}${id}`)
};

const exportedObject = {
    httpGetById, httpPost, httpPut, httpDelete, httpGetGroupAdmissions
};

export default exportedObject;