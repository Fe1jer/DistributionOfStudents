const config = {
    api: '/api/RecruitmentPlansApi/',
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
            console.error(error.message);
            throw Error(error);
        });
};
const httpGetFacultyRecruitmentPlans = (facultyShortName, year) => {
    return fetch(`${config.api}${facultyShortName}/${year}`, {
        ...config.options,
    })
        .then((response) => handleResponse(response))
        .then((response) => response.json())
        .catch((error) => {
            console.error(error.message);
            throw Error(error);
        });
};
const httpGetGroupRecruitmentPlans = (facultyShortName, groupId) => {
    return fetch(`${config.api}${facultyShortName}/${groupId}/GroupRecruitmentPlans`, {
        ...config.options,
    })
        .then((response) => handleResponse(response))
        .then((response) => response.json())
        .catch((error) => {
            console.error(error.message);
            throw Error(error);
        });
};
const httpGetFacultyLastYearRecruitmentPlas = (facultyShortName) => {
    return fetch(`${config.api}${facultyShortName}/lastYear`, {
        ...config.options,
    })
        .then((response) => handleResponse(response))
        .then((response) => response.json())
        .catch((error) => {
            console.error(error.message);
            throw Error(error);
        });
};
const httpPost = (facultyShortName, year, data) => {
    return fetch(`${config.api}${facultyShortName}/${year}`, {
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
const httpPut = (facultyShortName, year, data) => {
    return fetch(`${config.api}${facultyShortName}/${year}`, {
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
const httpDelete = (facultyShortName, year) => {
    return fetch(`${config.api}${facultyShortName}/${year}`, {
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
    httpGet, httpGetFacultyRecruitmentPlans, httpGetGroupRecruitmentPlans, httpGetFacultyLastYearRecruitmentPlas, httpPost, httpPut, httpDelete
};

export default exportedObject;