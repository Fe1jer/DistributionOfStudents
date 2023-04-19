const config = {
    api: '/api/StatisticApi/',
    options: {
        headers: { 'content-type': 'application/json' },
    },
};

const httpGetPlansStatistic = (facultyName, groupId) => {
    return fetch(`${config.api}PlansStatisticChart?groupId=${groupId}&facultyName=${facultyName}`, {
        ...config.options,
    })
        .then((response) => handleResponse(response))
        .then((response) => response.json())
        .catch((error) => {
            console.error(error.message);
            throw Error(error);
        });
};
const httpGetGroupStatistic = (groupId) => {
    return fetch(`${config.api}GroupStatisticChart?groupId=${groupId}`, {
        ...config.options,
    })
        .then((response) => handleResponse(response))
        .then((response) => response.json())
        .catch((error) => {
            console.error(error.message);
            throw Error(error);
        });
};
const httpPutGroupStatisticUrl = (facultyName, groupId) => {
    return fetch(`${config.api}${facultyName}/${groupId}`, {
        method: 'put',
        body: null,
        ...config.options,
    })
        .then((response) => handleResponse(response))
        .then((response) => response)
        .catch((error) => {
            console.error(error.message);
            throw Error(error);
        });
};

const handleResponse = async (response) => {
    if (response.status === 200) {
        return response;
    } else {
        throw Error(response.json() | 'error');
    }
};

const exportedObject = {
    httpGetPlansStatistic, httpGetGroupStatistic, httpPutGroupStatisticUrl
};

export default exportedObject;