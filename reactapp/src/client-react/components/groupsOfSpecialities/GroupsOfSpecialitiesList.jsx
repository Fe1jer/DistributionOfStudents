import GroupOfSpecialities from './GroupOfSpecialities.jsx';

import Table from 'react-bootstrap/Table';
import Button from 'react-bootstrap/Button';

export default function GroupsOfSpecialitiesList({ facultyShortName, groups, onClickDelete, onClickEdit, onClickCreate }) {
    var buttons = null;
    if (groups.every(group => group.isCompleted == false)) {
        buttons = <th width="100" className="text-center align-middle">
            <Button variant="empty" className="p-0 text-success" onClick={onClickCreate}>
                <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" className="bi bi-plus-circle-fill suc" viewBox="0 0 16 16">
                    <path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zM8.5 4.5a.5.5 0 0 0-1 0v3h-3a.5.5 0 0 0 0 1h3v3a.5.5 0 0 0 1 0v-3h3a.5.5 0 0 0 0-1h-3v-3z" />
                </svg>
            </Button>
        </th>;
    }
    return <div className="card shadow">
        <Table responsive className="table mb-0">
            <thead>
                <tr>
                    <th>Название</th>
                    <th>Срок</th>
                    <th>Конкурс</th>
                    <th>Специальности (направления специальностей)</th>
                    {buttons}
                </tr>
            </thead>
            <tbody>{
                groups.map((item, index) =>
                    <GroupOfSpecialities key={item.name + index} group={item} onClickDelete={onClickDelete} facultyShortName={facultyShortName} onClickDelete={onClickDelete} onClickEdit={onClickEdit} />
                )}
            </tbody>
        </Table>
    </div>;
}