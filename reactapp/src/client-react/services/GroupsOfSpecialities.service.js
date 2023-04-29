const config = {
    api: '/api/GroupsOfSpecialtiesApi/',
    options: {
        headers: { 'content-type': 'application/json' },
    },
};

const httpGetFacultyGroups = (facultyShortName, year) => {
    return fetch(`${config.api}FacultyGroups/${facultyShortName}/${year}`, {
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

const httpPost = (facultyShortName, data) => {
    return fetch(`${config.api}${facultyShortName}`, {
        method: 'post',
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

const httpPut = (facultyShortName, data) => {
    return fetch(`${config.api}${facultyShortName}`, {
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

const httpDelete = (facultyShortName, id) => {
    return fetch(`${config.api}${facultyShortName}/${id}`, {
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
        console.log((await response.json()));
        throw Error(JSON.stringify((await response.json()).modelErrors));
    }
    else {
        throw Error(response.json() | 'error');
    }
};

const exportedObject = {
    httpGetFacultyGroups, httpPost, httpPut, httpDelete, httpGetById
};

export default exportedObject;