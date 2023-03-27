import TablePreloader from "../TablePreloader.jsx";

import React from 'react';

export default function FacultiesPlansPreloader() {
    return (
        <React.Suspense>
            <hr className="mt-4 mx-0" />
            <p className="placeholder-glow"><span className="placeholder w-25"></span></p>
            <TablePreloader />
        </React.Suspense>
    );
}