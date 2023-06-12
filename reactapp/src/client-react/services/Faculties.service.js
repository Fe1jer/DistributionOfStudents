import { fetchWrapper } from "../../_helpers/fetch-wrapper";

const config = {
    api: '/api/FacultiesApi/',
};

const httpGet = () => {
    return fetchWrapper.get(`${config.api}`)
};

const httpGetByShortName = (shortName) => {
    return fetchWrapper.get(`${config.api}${shortName}`)
};

const httpPost = (data) => {
    return fetchWrapper.post(`${config.api}`, data, null)
};

const httpPut = (shortName, data) => {
    return fetchWrapper.put(`${config.api}${shortName}`, data, null)
};

const httpDelete = (shortName) => {
    return fetchWrapper.delete(`${config.api}${shortName}`)
};

const exportedObject = {
    httpGet, httpPost, httpPut, httpDelete, httpGetByShortName
};

export default exportedObject;