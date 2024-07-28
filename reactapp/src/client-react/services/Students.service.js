import { fetchWrapper } from "../../_helpers/fetch-wrapper";

const config = {
    api: '/api/StudentsApi',
};

const httpGet = (searhText, currentPage, pageLimit) => {
    return fetchWrapper.get(`${config.api}?search=${searhText}&page=${currentPage}&pageLimit=${pageLimit}`)
};

const exportedObject = {
    httpGet
};

export default exportedObject;