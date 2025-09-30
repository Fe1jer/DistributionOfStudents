import { store, authActions } from '../_store';

export const fetchWrapper = {
    get,
    post,
    put,
    delete: _delete
};

function authHeader() {
    const token = localStorage.getItem('accessToken');
    return token ? { Authorization: `Bearer ${token}` } : {};
}

function get(url) {
    return request(url, { method: 'GET' });
}

function post(url, body, contentType = { 'content-type': 'application/json' }) {
    return request(url, {
        method: 'POST',
        headers: { ...contentType },
        body: body
    });
}

function put(url, body, contentType = { 'content-type': 'application/json' }) {
    return request(url, {
        method: 'PUT',
        headers: { ...contentType },
        body: body
    });
}

function _delete(url) {
    return request(url, { method: 'DELETE' });
}

async function request(url, options) {
    const headers = {
        ...options.headers,
        ...authHeader()
    };

    const response = await fetch(url, { ...options, headers });

    if ([401, 403].includes(response.status)) {
        try {
            const refreshResult = await store.dispatch(authActions.refresh()).unwrap();
            const retryHeaders = {
                ...options.headers,
                Authorization: `Bearer ${refreshResult.accessToken}`
            };

            const retryResponse = await fetch(url, { ...options, headers: retryHeaders });
            return handleResponse(retryResponse);
        } catch {
            store.dispatch(authActions.logout());
            throw new Error('Unauthorized');
        }
    }

    return handleResponse(response);
}

async function handleResponse(response) {
    const text = await response.text();
    const data = text ? JSON.parse(text) : null;

    if (!response.ok) {
        const error = (data && data.message) || response.statusText;
        return Promise.reject(error);
    }

    return data;
}
