﻿import SpecialitiesArchiveList from './SpecialitiesArchiveList.jsx';

export default function GroupOfSpecialitiesArchive({ facultyArchive }) {
    return <table className="table table-bordered mb-0">
        <thead className="align-middle">
            <tr>
                <th width="130">Код</th>
                <th>Специальность (направление специальности)</th>
                <th width="75" className="text-center">План приема</th>
                <th width="80" className="text-center">Конкурс</th>
                <th width="110" className="text-center">Проходной балл</th>
            </tr>
        </thead>
        <tbody>{
            facultyArchive.groupsOfSpecialities.map((groupArchive) =>
                <SpecialitiesArchiveList key={JSON.stringify(groupArchive)} groupArchive={groupArchive} />
            )}
        </tbody>
    </table>;
}