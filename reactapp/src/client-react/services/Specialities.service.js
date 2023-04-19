const config = {
    api: '/api/SpecialitiesApi/',
    options: {
        headers: { 'content-type': 'application/json' },
    },
};

const httpGetFacultySpecialities = (facultyName) => {
    return fetch(`${config.api}FacultySpecialities/${facultyName}`, {
        ...config.options,
    })
        .then((response) => handleResponse(response))
        .then((response) => response.json())
        .catch((error) => {
            console.error(error.message);
            throw Error(error);
        });
};

const httpGetGroupSpecialities = (groupId) => {
    return fetch(`${config.api}GroupSpecialities/${groupId}`, {
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

const httpPost = (facultyName, data) => {
    return fetch(`${config.api}${facultyName}`, {
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
    httpGetById, httpPost, httpPut, httpDelete, httpGetFacultySpecialities, httpGetGroupSpecialities
};

export default exportedObject;