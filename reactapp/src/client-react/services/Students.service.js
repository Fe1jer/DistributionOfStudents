const config = {
    api: '/api/StudentsApi/',
    options: {
        headers: { 'content-type': 'application/json' },
    },
};

const httpGet = (searhText, currentPage, pageLimit) => {
    return fetch(`${config.api}?searchStudents=${searhText}&page=${currentPage}&pageLimit=${pageLimit}`, {
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
    httpGet
};

export default exportedObject;