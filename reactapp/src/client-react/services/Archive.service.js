import { fetchWrapper } from "../../_helpers/fetch-wrapper";

const config = {
    api: '/api/ArchiveApi/',
};

const httpGetYears = () => {
    return fetchWrapper.get(`${config.api}`)
};
const httpGetArchveForms = (year) => {
    return fetchWrapper.get(`${config.api}GetArchveFormsByYear/${year}`)
};
const httpGetArchveByYearAndForm = (year, form) => {
    return fetchWrapper.get(`${config.api}GetArchveByYearAndForm/${year}/${form}`)
};

const exportedObject = {
    httpGetYears, httpGetArchveForms, httpGetArchveByYearAndForm
};

export default exportedObject;