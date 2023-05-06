const config = {
    api: '/api/DistributionApi/',
    options: {
        headers: { 'content-type': 'application/json' },
    },
};

const httpGet = (facultyName, groupId) => {
    return fetch(`${config.api}${facultyName}/${groupId}`, {
        ...config.options,
    })
        .then((response) => handleResponse(response))
        .then((response) => response.json())
        .catch((error) => {
            throw Error(error);
        });
};

const httpGetGroupCompetition = (facultyName, groupId) => {
    return fetch(`${config.api}${facultyName}/${groupId}/Competition`, {
        ...config.options,
    })
        .then((response) => handleResponse(response))
        .then((response) => response.json())
        .catch((error) => {
            throw Error(error);
        });
};

const httpPost = (facultyName, groupId, data) => {
    return fetch(`${config.api}${facultyName}/${groupId}/CreateDistribution`, {
        method: 'post',
        body: data ? JSON.stringify(data) : null,
        ...config.options,
    })
        .then((response) => handleResponse(response))
        .then((response) => response.json())
        .catch((error) => {
            console.error(error);
            throw Error(error);
        });
};

const httpPostConfirm = (facultyName, groupId, distributedPlans, notify) => {
    return fetch(`${config.api}${facultyName}/${groupId}/ConfirmDistribution?notify=${notify}`, {
        method: 'post',
        body: distributedPlans ? JSON.stringify(distributedPlans) : null,
        ...config.options,
    })
        .then((response) => handleResponse(response))
        .then((response) => response)
        .catch((error) => {
            console.error(error);
            throw Error(error);
        });
};

const httpDelete = (facultyName, groupId) => {
    return fetch(`${config.api}${facultyName}/${groupId}`, {
        method: 'delete',
        ...config.options,
    })
        .then((response) => handleResponse(response))
        .then((response) => response)
        .catch((error) => {
            console.error(error);
            throw Error(error);
        });
};

const handleResponse = async (response) => {
    if (response.status === 200) {
        return response;
    }
    else if (response.status === 400) {
        throw Error(JSON.stringify((await response.json())));
    }
    else {
        throw Error(response.json() | 'error');
    }
};

const exportedObject = {
    httpGet, httpGetGroupCompetition, httpPost, httpPostConfirm, httpDelete
};

export default exportedObject;