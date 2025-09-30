import { createSlice } from '@reduxjs/toolkit';
import { login, refresh, logout } from './auth.actions';
import { history } from '../_helpers';

const initialState = {
    user: JSON.parse(localStorage.getItem('user')),
    accessToken: localStorage.getItem('accessToken'),
    refreshToken: localStorage.getItem('refreshToken'),
    error: null
};

const authSlice = createSlice({
    name: 'auth',
    initialState,
    reducers: {},
    extraReducers: (builder) => {
        // logout
        builder.addCase(logout.fulfilled, (state) => {
            state.user = null;
            state.accessToken = null;
            state.refreshToken = null;
            localStorage.removeItem('user');
            localStorage.removeItem('accessToken');
            localStorage.removeItem('refreshToken');
            history.navigate('/login');
        });
        builder.addCase(logout.rejected, (state, action) => {
            state.error = action.error;
            state.user = null;
            state.accessToken = null;
            state.refreshToken = null;
            localStorage.removeItem('user');
            localStorage.removeItem('accessToken');
            localStorage.removeItem('refreshToken');
            history.navigate('/login');
        });

        // login
        builder.addCase(login.fulfilled, (state, action) => {
            const data = action.payload;
            localStorage.setItem('user', JSON.stringify(data.user));
            localStorage.setItem('accessToken', data.accessToken);
            localStorage.setItem('refreshToken', data.refreshToken);

            state.user = data.user;
            state.accessToken = data.accessToken;
            state.refreshToken = data.refreshToken;

            const { from } = history.location.state || { from: { pathname: '/' } };
            history.navigate(from);
        });
        builder.addCase(login.rejected, (state, action) => {
            state.error = action.error;
        });

        // refresh
        builder.addCase(refresh.fulfilled, (state, action) => {
            const data = action.payload;
            localStorage.setItem('accessToken', data.accessToken);
            localStorage.setItem('refreshToken', data.refreshToken);

            state.accessToken = data.accessToken;
            state.refreshToken = data.refreshToken;
        });
        builder.addCase(refresh.rejected, (state, action) => {
            state.error = action.error;
        });
    }
});

export const authReducer = authSlice.reducer;
