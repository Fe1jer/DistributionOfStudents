import * as React from "react"
import EnrolledStudentsList from "../students/EnrolledStudentsList.jsx"

export default function DistributedPlan({ plan }) {
    return (
        <React.Suspense>
            <h5>{plan.specialityName}</h5>
            <EnrolledStudentsList enrolledStudents={plan.enrolledStudents} />
        </React.Suspense>
    );
}