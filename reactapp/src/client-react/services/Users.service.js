import { fetchWrapper } from "../../_helpers/fetch-wrapper";

const config = {
    api: '/api/UsersApi/',
};

const httpGet = () => {
    return fetchWrapper.get(`${config.api}`)
};

const httpGetById = (id) => {
    return fetchWrapper.get(`${config.api}${id}`)
};

const httpPost = (data) => {
    return fetchWrapper.post(`${config.api}`, data, null)
};

const httpPut = (id, data) => {
    return fetchWrapper.put(`${config.api}${id}`, data, null)
};

const httpDelete = (id) => {
    return fetchWrapper.delete(`${config.api}${id}`)
};

const exportedObject = {
    httpGet, httpGetById, httpPost, httpPut, httpDelete
};

export default exportedObject;