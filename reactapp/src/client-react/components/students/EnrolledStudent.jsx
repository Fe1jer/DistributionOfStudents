export default function EnrolledStudent({ index, student }) {
    return <tr className="align-middle">
        <td>{index + 1}</td>
        <td>{student.surname} {student.name} {student.patronymic}</td>
    </tr>;
}