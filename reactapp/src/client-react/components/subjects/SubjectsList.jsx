import Subject from './Subject.jsx';

export default function SubjectsList({ subjects }) {
    return (
        <div className="card shadow" >
            <table className="table mb-0">
                <thead>
                    <tr>
                        <th width="20">
                            №
                        </th>
                        <th>
                            Название
                        </th>
                        <th className="text-center" width="120">
                            <button type="button" className="btn btn-sm text-success ms-2" data-bs-toggle="modal" data-bs-target="#subjectUpdateForm" data-bs-subjectname="" data-bs-id="0" >
                                <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" className="bi bi-plus-circle-fill suc" viewBox="0 0 16 16">
                                    <path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zM8.5 4.5a.5.5 0 0 0-1 0v3h-3a.5.5 0 0 0 0 1h3v3a.5.5 0 0 0 1 0v-3h3a.5.5 0 0 0 0-1h-3v-3z" />
                                </svg>
                            </button >
                        </th>
                    </tr>
                </thead>
                <tbody>{
                    subjects.map((item, index) =>
                        <Subject num={index + 1} key={item.id + item.name} subject={item} />
                    )}
                </tbody>
            </table>
        </div >
    );
}