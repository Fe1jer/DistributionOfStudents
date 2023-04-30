const config = {
    api: '/api/AdmissionsApi/',
    options: {
        headers: { 'content-type': 'application/json' },
    },
};

const httpGetGroupAdmissions = (groupId, searhText, currentPage, pageLimit) => {
    return fetch(`${config.api}GroupAdmissions/${groupId}?searchStudents=${searhText}&page=${currentPage}&pageLimit=${pageLimit}`, {
        ...config.options,
    })
        .then((response) => handleResponse(response))
        .then((response) => response.json())
        .catch((error) => {
            console.error(error.message);
            throw Error(error);
        });
};

const httpGetById = (id) => {
    return fetch(`${config.api}${id}`, {
        ...config.options,
    })
        .then((response) => handleResponse(response))
        .then((response) => response.json())
        .catch((error) => {
            console.error(error.message);
            throw Error(error);
        });
};

const httpPost = (groupId, data) => {
    return fetch(`${config.api}${groupId}`, {
        method: 'post',
        body: data ? JSON.stringify(data) : null,
        ...config.options,
    })
        .then((response) => handleResponse(response))
        .then((response) => response)
        .catch((error) => {
            console.error(error.message);
            throw Error(error);
        });
};

const httpPut = (id, data) => {
    return fetch(`${config.api}${id}`, {
        method: 'put',
        body: data ? JSON.stringify(data) : null,
        ...config.options,
    })
        .then((response) => handleResponse(response))
        .then((response) => response)
        .catch((error) => {
            console.error(error.message);
            throw Error(error);
        });
};

const httpDelete = (id) => {
    return fetch(`${config.api}${id}`, {
        method: 'delete',
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
    httpGetById, httpPost, httpPut, httpDelete, httpGetGroupAdmissions
};

export default exportedObject;