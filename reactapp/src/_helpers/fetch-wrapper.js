import { store, authActions } from '../_store';

export const fetchWrapper = {
    get: request('GET'),
    post: request('POST'),
    put: request('PUT'),
    delete: request('DELETE')
};

function request(method) {
    return (url, body, contentType = { 'content-type': 'application/json' }) => {
        const requestOptions = {
            method,
            headers: { ...contentType, ...authHeader() },
        };
        if (body) {
            requestOptions.body = body;
        }
        return fetch(url, requestOptions)
            .then((response) => handleResponse(response))
            .then((response) => response)
            .catch((error) => {
                console.error(error);
                throw Error(error);
            });;
    }
}

// helper functions

function authHeader() {
    // return auth header with jwt if user is logged in and request is to the api url
    const token = authToken();
    const isLoggedIn = !!token;
    if (isLoggedIn) {
        return { Authorization: `Bearer ${token}` };
    } else {
        return {};
    }
}

function authToken() {
    return store.getState().auth.jwtToken;
}

function handleResponse(response) {
    return response.text().then(text => {
        const data = text ? JSON.parse(text) : null;
        if (!response.ok) {
            console.log(data)
            if (response.status === 400) {
                if (data.modelErrors) {
                    throw Error(JSON.stringify(data.modelErrors));
                }
            }
            else if ([401, 403].includes(response.status) && authToken()) {
                // auto logout if 401 Unauthorized or 403 Forbidden response returned from api
                const logout = () => store.dispatch(authActions.logout());
                logout();
            }
            const error = (data && data.message) || response.statusText;
            return Promise.reject(error);
        }

        return data;
    });
}