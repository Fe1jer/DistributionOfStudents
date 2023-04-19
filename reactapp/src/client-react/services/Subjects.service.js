const config = {
    api: '/api/SubjectsApi/',
    options: {
        headers: { 'content-type': 'application/json' },
    },
};

const httpGet = () => {
    return fetch(`${config.api}`, {
        ...config.options,
    })
        .then((response) => handleResponse(response))
        .then((response) => response.json())
        .catch((error) => {
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
            throw Error(error);
        });
};

const httpGetGroupSubjects = (groupId) => {
    return fetch(`${config.api}GroupSubjects/${groupId}`, {
        ...config.options,
    })
        .then((response) => handleResponse(response))
        .then((response) => response.json())
        .catch((error) => {
            throw Error(error);
        });
};

const httpPost = (data) => {
    return fetch(`${config.api}`, {
        method: 'post',
        body: data ? JSON.stringify(data) : null,
        ...config.options,
    })
        .then((response) => handleResponse(response))
        .then((response) => response)
        .catch((error) => {
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
            console.error(error);
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
            console.error(error);
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
    httpGet, httpGetById, httpPost, httpPut, httpDelete, httpGetGroupSubjects
};

export default exportedObject;