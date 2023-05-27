import Header from "./Header.jsx";
import Content from "./Content.jsx";

import React from "react";
import { HashRouter as Router} from 'react-router-dom'

export default function Body() {
    return (
        <Router>
            <Header />
            <main id="main" className="pb-5">
                <Content />
            </main>
        </Router>
    );
}