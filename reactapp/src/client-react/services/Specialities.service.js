import { fetchWrapper } from "../../_helpers/fetch-wrapper";

const config = {
    api: '/api/SpecialitiesApi/',
};

const httpGetFacultySpecialities = (facultyName) => {
    return fetchWrapper.get(`${config.api}FacultySpecialities/${facultyName}`)
};

const httpGetFacultyDisabledSpecialities = (facultyName) => {
    return fetchWrapper.get(`${config.api}FacultyDisabledSpecialities/${facultyName}`)
};

const httpGetGroupSpecialities = (groupId) => {
    return fetchWrapper.get(`${config.api}GroupSpecialities/${groupId}`)
};

const httpGetById = (id) => {
    return fetchWrapper.get(`${config.api}${id}`)
};

const httpPost = (facultyName, data) => {
    return fetchWrapper.post(`${config.api}${facultyName}`, JSON.stringify(data))
};

const httpPut = (id, data) => {
    return fetchWrapper.put(`${config.api}${id}`, JSON.stringify(data))
};

const httpDelete = (id) => {
    return fetchWrapper.delete(`${config.api}${id}`)
};

const exportedObject = {
    httpGetById, httpPost, httpPut, httpDelete, httpGetFacultySpecialities, httpGetFacultyDisabledSpecialities, httpGetGroupSpecialities
};

export default exportedObject;