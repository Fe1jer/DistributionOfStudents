export default function Student({ student}) {
    return <tr key={student.id} className="align-middle">
        <td>
            <a className="nav-link text-success p-0" href={"/Faculties/" + student.facultyName + "/" + student.groupId + "?searchStudents=" + student.fullName}>
                {student.fullName}
            </a>
        </td>
        <td>{student.groupName}</td>
        <td>{student.facultyName}</td>
    </tr>;
}