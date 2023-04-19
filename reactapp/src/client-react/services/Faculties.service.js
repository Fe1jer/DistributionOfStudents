const config = {
    api: '/api/FacultiesApi/',
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

const httpGetByShortName = (shortName) => {
    return fetch(`${config.api}${shortName}`, {
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
        body: data ? data : null,
    })
        .then((response) => handleResponse(response))
        .then((response) => response)
        .catch((error) => {
            console.error(error);
            throw Error(error);
        });
};

const httpPut = (shortName, data) => {
    return fetch(`${config.api}${shortName}`, {
        method: 'put',
        body: data ? data : null,
    })
        .then((response) => handleResponse(response))
        .then((response) => response)
        .catch((error) => {
            console.error(error);
            throw Error(error);
        });
};

const httpDelete = (shortName) => {
    return fetch(`${config.api}${shortName}`, {
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
        throw Error(JSON.stringify((await response.json()).modelErrors));
    }
    else {
        throw Error(response.json() | 'error');
    }
};

const exportedObject = {
    httpGet, httpPost, httpPut, httpDelete, httpGetByShortName
};

export default exportedObject;