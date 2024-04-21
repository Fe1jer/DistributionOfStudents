import { fetchWrapper } from "../../_helpers/fetch-wrapper";

const config = {
    api: '/api/GroupsOfSpecialitiesApi/',
};

const httpGetFacultyGroups = (facultyShortName, year) => {
    return fetchWrapper.get(`${config.api}FacultyGroups/${facultyShortName}/${year}`)
};

const httpGetById = (id) => {
    return fetchWrapper.get(`${config.api}${id}`)
};

const httpPost = (facultyShortName, data) => {
    return fetchWrapper.post(`${config.api}${facultyShortName}`, JSON.stringify(data))
};

const httpPut = (facultyShortName, data) => {
    return fetchWrapper.put(`${config.api}${facultyShortName}`, JSON.stringify(data))
};

const httpDelete = (facultyShortName, id) => {
    return fetchWrapper.delete(`${config.api}${facultyShortName}/${id}`)
};

const exportedObject = {
    httpGetFacultyGroups, httpPost, httpPut, httpDelete, httpGetById
};

export default exportedObject;