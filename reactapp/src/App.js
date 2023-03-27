import React, { Component } from 'react';
import Body from './client-react/components/Body.jsx';

export default class App extends Component {
    static displayName = App.name;

    constructor(props) {
        super(props);
        this.state = { loading: true };
    }

    componentDidMount() {
        this.populateWeatherData();
    }

    static renderForecastsTable() {
        return (
            <Body />
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationreact">https://aka.ms/jspsintegrationreact</a> for more details.</em></p>
            : App.renderForecastsTable();

        return (
            <div id="body">
                {contents}
            </div>
        );
    }

    async populateWeatherData() {
        const response = await fetch('/api/FacultiesApi');
        const data = await response.json();
        this.setState({ loading: false });
    }
}
