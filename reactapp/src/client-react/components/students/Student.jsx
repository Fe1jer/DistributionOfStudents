import { Link } from 'react-router-dom'

export default function Student({ student }) {
    return <tr key={student.id} className="align-middle">
        <td>
            <Link className="nav-link text-success p-0" to={"/Faculties/" + student.facultyName + "/" + student.groupId + "?searchStudents=" + student.fullName}>
                {student.fullName}
            </Link>
        </td>
        <td>{student.facultyName}</td>
        <td>{student.groupName}</td>
    </tr>;
}