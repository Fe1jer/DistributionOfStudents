import CreateButton from '../adminButtons/CreateButton.jsx';
import GroupOfSpecialities from './GroupOfSpecialities.jsx';

import Table from 'react-bootstrap/Table';

export default function GroupsOfSpecialitiesList({ facultyShortName, groups, onClickDelete, onClickEdit, onClickCreate }) {
    var buttons = null;
    if (groups.every(group => group.isCompleted === false)) {
        buttons = <th width="100" className="text-center align-middle">
            <CreateButton onClick={onClickCreate} />
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
                    <GroupOfSpecialities key={item.name + index} group={item} onClickDelete={onClickDelete} facultyShortName={facultyShortName} onClickEdit={onClickEdit} />
                )}
            </tbody>
        </Table>
    </div>;
}