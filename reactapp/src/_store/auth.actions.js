import { createAsyncThunk } from '@reduxjs/toolkit';
import { fetchWrapper } from '../_helpers';

const config = {
    api: '/auth/api/UserTokensApi/',
};

// logout
export const logout = createAsyncThunk(
    'auth/logout',
    async (_, { getState }) => {
        const refreshToken = getState().auth.refreshToken; // берём из state
        return await fetchWrapper.post(`${config.api}sign-out`, JSON.stringify(refreshToken))
    }
);

// login
export const login = createAsyncThunk(
    'auth/login',
    async ({ username, password }) =>
        await fetchWrapper.post(
            `${config.api}/login`,
            JSON.stringify({ username, password, rememberMe: false })
        )
);

// refresh
export const refresh = createAsyncThunk(
    'auth/refresh', async (_, { getState }) => {
        const refreshToken = getState().auth.refreshToken; // берём из state
        return await fetchWrapper.post(`${config.api}/refresh-token`, JSON.stringify(refreshToken))
    }
);

export const authActions = { login, refresh, logout };