import { fetchWrapper } from "../../_helpers/fetch-wrapper";

const config = {
    api: '/api/SubjectsApi/',
};

const httpGet = () => {
    return fetchWrapper.get(`${config.api}`)
};

const httpGetById = (id) => {
    return fetchWrapper.get(`${config.api}${id}`)
};

const httpGetGroupSubjects = (groupId) => {
    return fetchWrapper.get(`${config.api}GroupSubjects/${groupId}`)
};

const httpPost = (data) => {
    return fetchWrapper.post(`${config.api}`, JSON.stringify(data))
};

const httpPut = (id, data) => {
    return fetchWrapper.put(`${config.api}${id}`, JSON.stringify(data))
};

const httpDelete = (id) => {
    return fetchWrapper.delete(`${config.api}${id}`)
};

const exportedObject = {
    httpGet, httpGetById, httpPost, httpPut, httpDelete, httpGetGroupSubjects
};

export default exportedObject;