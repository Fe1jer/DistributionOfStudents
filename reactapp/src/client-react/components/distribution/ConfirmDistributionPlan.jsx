import DistributedPlan from "../distribution/DistributedPlan.jsx";

import React, { useState, useEffect } from 'react';

export default function ConfirmDistributionPlan({ plan, index, onChangeSelectedStudents }) {
    const [updatedSelectedStudents, setUpdatedSelectedStudents] = useState(null);

    const initializationSelectedStudents = () => {
        var initializatedSelectedStudents = plan.enrolledStudents.map((item) => {
            return { studentId: item.student.id, isDistributed: true }
        });

        setUpdatedSelectedStudents(initializatedSelectedStudents);
        onChangeSelectedStudents(index, initializatedSelectedStudents);
    };

    useEffect(() => {
        initializationSelectedStudents();
    }, [])

    return (
        <DistributedPlan plan={plan} />
    );
}