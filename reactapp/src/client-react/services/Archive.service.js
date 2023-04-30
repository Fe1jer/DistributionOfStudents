const config = {
    api: '/api/ArchiveApi/',
    options: {
        headers: { 'content-type': 'application/json' },
    },
};

const httpGetYears = () => {
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
const httpGetArchveForms = (year) => {
    return fetch(`${config.api}GetArchveFormsByYear/${year}`, {
        ...config.options,
    })
        .then((response) => handleResponse(response))
        .then((response) => response.json())
        .catch((error) => {
            console.error(error.message);
            throw Error(error);
        });
};
const httpGetArchveByYearAndForm = (year, form) => {
    return fetch(`${config.api}GetArchveByYearAndForm/${year}/${form}`, {
        ...config.options,
    })
        .then((response) => handleResponse(response))
        .then((response) => response.json())
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
    httpGetYears, httpGetArchveForms, httpGetArchveByYearAndForm
};

export default exportedObject;