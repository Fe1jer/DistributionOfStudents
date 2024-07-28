import DeleteButton from '../adminButtons/DeleteButton';
import EditButton from '../adminButtons/EditButton';

export default function Subject({ subject, num, onClickEdit, onClickDelete }) {
    return <tr key={subject.id} className="align-middle">
        <td>{num}</td>
        <td>{subject.name}</td>
        <td className="text-center">
            <div className="d-inline-flex">
                <EditButton onClick={() => onClickEdit(subject.id)} />
                <DeleteButton onClick={() => onClickDelete(subject.id)} />
            </div>
        </td>
    </tr>;
}